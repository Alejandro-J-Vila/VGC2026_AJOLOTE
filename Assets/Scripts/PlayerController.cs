using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public GameManager gm; // Reference to the game manager
    public InputAction moveAction; // Reference to the InputAction for movement

    private float maxMoveSpeed = 3f; // Maximun speed at which the player can move
    private float moveSpeed = 3f; // Current speed at which the player moves
    private bool isMoving = false; // Flag to check if the player is currently moving

    private Tilemap groundLayer; // Reference to the ground tilemap
    private Tilemap obstacleLayer; // Reference to the obstacle tilemap
    private Tilemap wallLayer; // Reference to the wall tilemap
    private Tilemap fogLayer; // Reference to the fog tilemap
    private Tilemap objectiveLayer; // Reference to the objective tilemap

    private int fogRevealRadius = 2; // Radius around the player to reveal fog
    private float fogFadeDuration = 0.3f; // Fog reveal duration
    private Vector3Int startingPosition; // Starting position of the player in grid coordinates
    private Vector3Int endPosition; // End position of the player in grid coordinates

    public Sprite playerUp; // Sprite for the player facing up
    public Sprite playerDown; // Sprite for the player facing down
    public Sprite playerLeft; // Sprite for the player facing left
    public Sprite playerRight; // Sprite for the player facing right
    private Vector3 playerDirection; // Facing direction of the player at the start of the game
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component

    private bool onObstacle = false; // Flag to indicate the player is deciding on an obstacle
    private bool onRespawn = false; // Flag to indicate the player is selecting skills on respawn
    private bool onLore = false; // Flag to indicate the player is reading a lore scroll
    private bool onLimbAction = false; // Flag to indicate the player is watching the lost/recover limb animation
    private bool obstaclePassed = false; // Flag to indicate the player passed an obstacle
    private bool gameOver = false; // Flag to indicate the player reached the end of the map

    void Start()
    {
        // Get the sprite renderer component attached to the player
        spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    void Update()
    {
        // Check if the player can move
        if (isMoving || onObstacle || onRespawn || onLore || onLimbAction || gameOver)
        {
            return;
        }
        // Get the movement direction from the input action
        Vector3 targetDirection = GetMovementDirection();
        // If there is a valid movement direction
        if (targetDirection != Vector3.zero)
        {
            // Set the apropriate sprite for the player based on the direction
            SetPlayerSprite(targetDirection);
            // Start the movement coroutine
            StartCoroutine(ConstantGridMovement(targetDirection));
        }
    }

    private void OnEnable()
    {
        // Enable the move action when the script is enabled
        moveAction.Enable();
    }

    private void OnDisable()
    {
        // Disable the move action when the script is disabled
        moveAction.Disable();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player collides with an item
        Item item = collision.GetComponent<Item>();
        // Check if the item is regeneration and the player has all the limbs, don't pick it up
        if (item.itemName == Item.Items.RegenerationFood && !gm.LostLimb())
        {
            gm.ItemInUse(TextManager.UIText.regen_not_needed);
            return;
        }
        if (item.itemName == Item.Items.ShieldFood && gm.ShieldOn())
        {
            gm.ItemInUse(TextManager.UIText.shield_in_use);
            return;
        }
        if (item.itemName == Item.Items.CompassFood && gm.CompassOn())
        {
            gm.ItemInUse(TextManager.UIText.compass_in_use);
            return;
        }
        if (item.itemName == Item.Items.ExplorationFood && gm.ExplorationOn())
        {
            gm.ItemInUse(TextManager.UIText.exploration_in_use);
            return;
        }
        // Let the game manager handle the item pickup
        gm.PickItemUp(item);
        // Destroy the item game object after pickup
        Destroy(collision.gameObject);
    }

    private Vector3 GetMovementDirection()
    {
        // Read the input value from the move action
        Vector2 input = moveAction.ReadValue<Vector2>();
        float horizontal = 0;
        float vertical = 0;
        // Determine the movement direction based on the input value
        if (Mathf.Abs(input.x) > 0.5f)
        {
            horizontal = Mathf.Sign(input.x);
        }
        else if (Mathf.Abs(input.y) > 0.5f)
        {
            vertical = Mathf.Sign(input.y);
        }
        // Return the movement direction as a Vector3
        if (horizontal != 0 || vertical != 0)
        {
            return new Vector3(horizontal, vertical, 0f);
        }
        // If there is no valid movement direction, return Vector3.zero
        return Vector3.zero;
    }

    private void SetPlayerSprite(Vector3 direction)
    {
        // Set the sprite of the player based on the movement direction
        if (direction == Vector3.up)
        {
            spriteRenderer.sprite = playerUp;
        }
        else if (direction == Vector3.down)
        {
            spriteRenderer.sprite = playerDown;
        }
        else if (direction == Vector3.left)
        {
            spriteRenderer.sprite = playerLeft;
        }
        else if (direction == Vector3.right)
        {
            spriteRenderer.sprite = playerRight;
        }
    }

    private IEnumerator ConstantGridMovement(Vector3 direction)
    {
        // Set the isMoving flag to true to indicate that the player is moving
        isMoving = true;
        Vector3 currentDirection = direction;
        // Play the movement sound
        SoundManager.sm.PlaySoundLoop(SoundManager.SoundTrack.Walk);
        // Continue moving in the current direction
        while (currentDirection != Vector3.zero)
        {
            // Calculate the target position based on the current direction
            Vector3 targetPosition = transform.position + currentDirection;
            // Convert the target position to grid coordinates
            Vector3Int targetCell = groundLayer.WorldToCell(targetPosition);
            // Check if the player can move to the target cell
            if (!CanMoveTo(targetCell))
            {
                break;
            }
            // Snap the player to the center of the target cell
            Vector3 snapDestination = groundLayer.GetCellCenterWorld(targetCell);
            // Transition the player towards the snap destination
            while (Vector3.Distance(transform.position, snapDestination) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, snapDestination, moveSpeed * Time.deltaTime);
                yield return null;
            }
            // Ensure the player is exactly at the snap destination
            transform.position = snapDestination;
            PassedObstacle();
            // Reveal the fog around the player after moving
            RevealFog(targetCell);
            // Get the new movement direction from the input action
            currentDirection = GetMovementDirection();
            // Set the apropriate sprite for the player based on the direction
            SetPlayerSprite(currentDirection);
        }
        // Set the isMoving flag to false to indicate that the player has stopped moving
        isMoving = false;
        // Stop the movement sound
        SoundManager.sm.StopSoundLoop();
    }

    private bool CanMoveTo(Vector3Int cell)
    {
        // Check if the target cell is within the bounds of the ground layer and not blocked by walls
        if (!groundLayer.HasTile(cell) || wallLayer.HasTile(cell))
        {
            return false;
        }
        // If the target cell is the end position of the map
        if (endPosition == cell && objectiveLayer.HasTile(cell))
        {
            // Game over
            gameOver = true;
            // Signal the game manager to show the game over screen
            gm.GameOver();
            return true;
        }
        // Check if the target cell has an obstacle and handle it accordingly
        if (obstacleLayer.HasTile(cell))
        {
            // Get the obstacle tile at the specified cell
            ObstacleTile obstacleTile = (ObstacleTile)obstacleLayer.GetTile(cell);
            // Let the game manager handle the obstacle
            return gm.ManageObstacle(obstacleTile, cell);
        }
        // If the target cell is valid and not blocked, allow movement
        return true;
    }

    private void RevealFog(Vector3Int radiusCenter)
    {
        // Create a hash to store the cells we already started revealing to avoid starting them again
        HashSet<Vector3Int> processedTiles = new HashSet<Vector3Int>();
        // We calculate a radius around the player to reveal fog
        for (int x = -fogRevealRadius; x <= fogRevealRadius; x++)
        {
            for (int y = -fogRevealRadius; y <= fogRevealRadius; y++)
            {
                // Reveal each cell inside that radius
                Vector3Int fogCell = new Vector3Int(radiusCenter.x + x, radiusCenter.y + y, radiusCenter.z);
                // If the cell has a tile and is not already being revealed
                if (fogLayer.HasTile(fogCell) && !processedTiles.Contains(fogCell))
                {
                    // Add the cell to the processed tiles
                    processedTiles.Add(fogCell);
                    // Start revealing it
                    StartCoroutine(FadeFog(fogCell, fogFadeDuration));
                }
            }
        }
    }

    private IEnumerator FadeFog(Vector3Int fogPosition, float fadeDuration)
    {
        // Get the color of the cell
        Color startColor = fogLayer.GetColor(fogPosition);
        // Time variable for the loop
        float elapsedTime = 0f;
        // While the fade duration lasts
        while (elapsedTime < fadeDuration)
        {
            // Add elapsed time
            elapsedTime += Time.deltaTime;
            // Calculate the amount of alpha to apply to the cell sprite
            float newAlpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            // Set the new color with the new alpha to the sprite
            fogLayer.SetColor(fogPosition, new Color(startColor.r, startColor.g, startColor.b, newAlpha));
            yield return null;
        }
        // When the fade is complete, remove the tile
        fogLayer.SetTile(fogPosition, null);
    }

    public void SetOnObstacle(bool obs)
    {
        // Set the flag to indicate the player is deciding on an obstacle
        onObstacle = obs;
    }

    public void SetOnRespawn(bool res)
    {
        // Set the flag to indicate the player is deciding on respawn
        onRespawn = res;
    }

    public void SetOnLore(bool lore)
    {
        // Set the flag to indicate the player is reading a lore scroll
        onLore = lore;
    }

    public void SetOnLimbAction(bool limb)
    {
        // Set the flag to indicate the player is watching the lost/recover limb animation
        onLimbAction = limb;
    }

    public void SetObstaclePassed()
    {
        // Set the flag to indicate the obstacle was crossed
        obstaclePassed = true;
    }

    public void CrossObstacle(Vector3Int pos)
    {
        // Set the player position to the obstacle position to cross it
        transform.position = groundLayer.GetCellCenterWorld(pos);
    }

    public void Retreat()
    {
        // Set the position of the player to the center of the starting position to respawn
        transform.position = groundLayer.GetCellCenterWorld(startingPosition);
        // Update the sprite to face up after respawning
        SetPlayerSprite(playerDirection);
    }

    private void PassedObstacle()
    {
        // If the player crosses an obstacle
        if (obstaclePassed)
        {
            // Reset the flag, to use it later on another obstacle
            obstaclePassed = false;
            // Signal the game manager to show the corresponding panel and messages
            gm.ObstaclePassed();
        }
    }

    public void SetObjectivePositions(Vector3Int start, Vector3Int end)
    {
        // Set the starting and ending positions of the map
        startingPosition = start;
        endPosition = end;
        // Set the position of the player to the center of the starting tile
        transform.position = groundLayer.GetCellCenterWorld(startingPosition);
        // Reveal the fog on the starting position
        RevealFog(startingPosition);
        // Set the player facing direction
        SetPlayerSprite(playerDirection);
    }

    public int GetExplorationRadius()
    {
        // Return the fog reveal radius
        return fogRevealRadius;
    }

    public void SetExplorationRadius(int radius)
    {
        // Set the fog reveal radius to the new value
        fogRevealRadius = radius;
        // Reveal the fog with the new radius
        RevealFog(groundLayer.WorldToCell(transform.position));
    }

    public void SetupLayers(Tilemap groundL, Tilemap obstacleL, Tilemap wallL, Tilemap fogL, Tilemap objectiveL)
    {
        // Setup the selected map layers
        groundLayer = groundL;
        obstacleLayer = obstacleL;
        wallLayer = wallL;
        fogLayer = fogL;
        objectiveLayer = objectiveL;
    }

    public void SetPlayerDirection(Vector3 direction)
    {
        // Setup the player facing direction
        playerDirection = direction;
    }

    public void SetMovementSpeed(bool increase, bool reset)
    {
        // If the reset flag is on
        if (reset)
        {
            // Reset the movement speed to its original value
            moveSpeed = maxMoveSpeed;
        }
        else
        {
            // Depending on the increase flag, increase or decrease the movement speed by 1
            if (increase)
            {
                moveSpeed += 0.35f;
            }
            else
            {
                moveSpeed -= 0.35f;
            }
        }
    }
}
