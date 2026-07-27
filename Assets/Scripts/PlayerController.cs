using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public InputAction moveAction; // Reference to the InputAction for movement
    public float moveSpeed = 5f; // Speed at which the player moves
    private bool isMoving = false; // Flag to check if the player is currently moving
    public Tilemap groundLayer; // Reference to the ground tilemap
    public Tilemap obstacleLayer; // Reference to the obstacle tilemap
    public Tilemap wallLayer; // Reference to the wall tilemap
    public Tilemap fogLayer; // Reference to the fog tilemap
    public int fogRevealRadius = 2; // Radius around the player to reveal fog
    private Vector3Int startingPosition; // Starting position of the player in grid coordinates
    private Vector3Int endPosition; // Endgame position of the player in grid coordinates

    void Start()
    {
        // Snap the player to the center of the starting tile
        startingPosition = groundLayer.WorldToCell(transform.position);
        // Set the position of the player to the center of the starting tile
        transform.position = groundLayer.GetCellCenterWorld(startingPosition);
    }

    void Update()
    {
        // Check if the player is already moving
        if (isMoving)
        {
            return;
        }
        // Get the movement direction from the input action
        Vector3 targetDirection = GetMovementDirection();
        // If there is a valid movement direction, start the movement coroutine
        if (targetDirection != Vector3.zero)
        {
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

    private IEnumerator ConstantGridMovement(Vector3 direction)
    {
        // Set the isMoving flag to true to indicate that the player is moving
        isMoving = true;
        Vector3 currentDirection = direction;
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
            // Reveal the fog around the player after moving
            RevealFog(targetCell);
            // Get the new movement direction from the input action
            currentDirection = GetMovementDirection();
        }
        // Set the isMoving flag to false to indicate that the player has stopped moving
        isMoving = false;
    }

    private bool CanMoveTo(Vector3Int cell)
    {
        // Check if the target cell is within the bounds of the ground layer and not blocked by walls
        if (!groundLayer.HasTile(cell) || wallLayer.HasTile(cell))
        {
            return false;
        }
        // Check if the target cell has an obstacle and handle it accordingly
        if (obstacleLayer.HasTile(cell))
        {
            CheckObstacleType(cell);
            return true;
        }
        // If the target cell is valid and not blocked, allow movement
        return true;
    }

    private void CheckObstacleType(Vector3Int cell)
    {
        // Get the obstacle tile at the specified cell
        TileBase obstacleTile = obstacleLayer.GetTile(cell);
        // Log the name of the obstacle tile, here we manage the obstacle mechanic, later
        Debug.Log("Obstacle found: " + obstacleTile.name);
    }

    private void RevealFog(Vector3Int radiusCenter)
    {
        // We calculate a radius around the player to reveal fog
        for (int x = -fogRevealRadius; x <= fogRevealRadius; x++)
        {
            for (int y = -fogRevealRadius; y <= fogRevealRadius; y++)
            {
                // Reveal each cell inside that radius
                Vector3Int fogCell = new Vector3Int(radiusCenter.x + x, radiusCenter.y + y, radiusCenter.z);
                fogLayer.SetTile(fogCell, null);
            }
        }
    }
}
