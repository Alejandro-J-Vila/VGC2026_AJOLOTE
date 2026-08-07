using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Tilemap[] groundLayerList; // List of all the map ground layer configurations
    public Tilemap[] wallLayerList; // List of all the map wall layer configurations
    public Tilemap[] obstacleLayerList; // List of all the map obstacle layer configurations
    public Tilemap[] objectiveLayerList; // List of all the map objective layer configurations
    public Tilemap[] itemLayerList; // List of all the map item layer configurations
    public Tilemap[] loreLayerList; // List of all the map lore layer configurations
    public Tilemap[] fogLayerList; // List of all the map fog layer configurations
    public ObstacleTile[] obstacleTilesList; // List of all the obstacle tiles

    private Skill[] skillList; // List of all the skills in the game
    public Sprite[] skillSprites; // List of all skill sprites

    private Skill[] skillSlots; // List of the skills the player has equipped
    private Skill[] skillSlotsBackup; // Backup list of the skills the player has equipped to restore when the player uses a regeneration item
    private int skillSlotsCount = 3; // Number of skill slots the player has
    public Image[] skillSlotImagesG; // List of the UI images that represent the skills the player has equipped in game
    public Image[] skillSlotImagesR; // List of the UI images that represent the skills the player has selected in the reset menu
    
    private Skill[] skillChoices; // List of the skills the player can choose from in the reset menu
    private int skillChoicesCount = 4; // Number of skill choices the player can choose from in the reset menu
    public Image[] skillChoiceImages; // Array of images that represent the skill choices in the reset menu
    public TMP_Text[] skillChoiceTexts; // Array of UI texts for the descriptions of the skill choices

    public GameObject resetPanel; // Reference to the reset panel
    public Button resetChoicesButton; // Reference to the reset choice selection button in the reset menu
    public Button confirmChoicesButton; // Reference to the confirm choices button in the reset menu

    public GameObject obstaclePanel; // Reference to the obstacle panel used when an obstacle is found
    public GameObject successPanel; // Reference to the success panel used when crossing an obstacle succesfully
    public GameObject failPanel; // Reference to the fail panel used when failing to cross an obstacle
    public GameObject gameOverPanel; // Reference to the game over panel used when the player reach the end of the map
    public TMP_Text scoreText; // Reference to the score text in the game over panel
    public GameObject informationPanel; // Reference to the information panel used to show the player information about the items they pick up
    public TMP_Text informationText; // Reference to the information text contained in the information panel
    public GameObject lorePanel; // Reference to the lore panel used to show the player the lore scrolls they pick up
    public TMP_Text loreText; // Reference to the lore text contained in the lore panel

    public PlayerController pc; // Reference to the player

    private Vector3Int[] startPositionList; // List of all the starting positions in the different map configurations
    private Vector3Int[] endPositionList; // List of all the ending positions in the different map configurations

    private Vector3Int startPosition; // Player starting position in the map
    private Vector3Int endPosition; // Player end position in the map

    private Vector3Int currentObstaclePosition; // Position of the current ecountered obstacle
    private Image skillUsed; // Skill slot of the skill used to cross an obstacle safely

    private int totalNumberOfLimbs = 3; // Total number of limbs
    private int limbsLeft = 3; // Number of limbs left

    public GameObject[] itemPrefabs; // Array of item prefabs to spawn in the map

    private float itemDuration = 10.0f; // Duration of the item effects in seconds
    public GameObject compass; // Game object to show the player and guide him towards the end of the level
    private bool compassOn = false; // Compass flag to indicate the compass activation
    public GameObject shield; // Game object to show to the player when a shield item is picked up
    private bool shieldOn = false; // Shield flag to indicate the shield activation

    private Tilemap objectiveLayer; // Layer of the map where the start and end positions are stored
    private Tilemap itemLayer; // Layer of the map where the items are spawned
    private Tilemap loreLayer; // Layer of the map where the lore scrolls are spawned
    private Tilemap obstacleLayer; // Layer of the map where all the obstacles are placed

    private int numberOfItems = 10; // Number of items to spawn in the map
    private int numberOfLoreScrolls = 5; // Number of lore scrolls to spawn in the map

    private int numberOfResets = 0; // Counter for the number of resets the player did
    private int safePassageProbability = 10; // Probability of passing an obstacle succesfully without the proper skill

    void Start()
    {
        // Select one of the maps to start the game
        SelectMap();
        // Randomize all the obstacles in the map
        RandomizeObstacles();
        // Spawn the items in the level
        SpawnItems(itemLayer, numberOfItems, false);
        // Spawn the lore scrolls in the level
        SpawnItems(loreLayer, numberOfLoreScrolls, true);
        // Create all the skills of the game
        InitializeSkillList();
        // Create the arrays for the skill slots, backup and choices
        skillSlots = new Skill[skillSlotsCount];
        skillSlotsBackup = new Skill[skillSlotsCount];
        skillChoices = new Skill[skillChoicesCount];
        // At the start of the game the player has no skills
        for (int i = 0; i < skillSlotsCount; i++)
        {
            skillSlots[i] = skillList[(int)Skill.Skills.NoSkill];
            skillSlotsBackup[i] = skillList[(int)Skill.Skills.NoSkill];
            skillSlotImagesG[i].sprite = skillSlots[i].icon;
        }
        // Setup all the components of the reset menu
        SetupResetMenu(false);
        // Hide all the panels used later
        resetPanel.SetActive(false);
        obstaclePanel.SetActive(false);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        gameOverPanel.SetActive(false);
        informationPanel.SetActive(false);
        lorePanel.SetActive(false);
        compass.SetActive(false); 
        shield.SetActive(false);
    }

    private void Update()
    {
        // If the compass is on
        if (compassOn)
        {
            // Rotate the marker to indicate the endgame position
            RotateTowardsEnd();
        }
    }

    private void RotateTowardsEnd()
    {
        // Get the position of the player and the end position
        Vector3 playerPos = pc.transform.position;
        Vector3 endPos = objectiveLayer.GetCellCenterWorld(endPosition);
        // Calculate the direction from the player to the end position
        Vector3 direction = (endPos - playerPos).normalized;
        // Calculate the angle in degrees between the player's forward direction and the direction to the end position
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Rotate the compass to point towards the end position
        compass.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
    }

    private void InitializeSkillList()
    {
        // Create the skill list
        skillList = new Skill[8];
        // Initialize the skills list with all the skills in the game
        skillList[(int)Skill.Skills.RedS] = new Skill(Skill.Skills.RedS, "Red skill", skillSprites[(int)Skill.Skills.RedS]);
        skillList[(int)Skill.Skills.GreenS] = new Skill(Skill.Skills.GreenS, "Green skill", skillSprites[(int)Skill.Skills.GreenS]);
        skillList[(int)Skill.Skills.BlueS] = new Skill(Skill.Skills.BlueS, "Blue skill", skillSprites[(int)Skill.Skills.BlueS]);
        skillList[(int)Skill.Skills.YellowS] = new Skill(Skill.Skills.YellowS, "Yellow skill", skillSprites[(int)Skill.Skills.YellowS]);
        skillList[(int)Skill.Skills.PinkS] = new Skill(Skill.Skills.PinkS, "Pink skill", skillSprites[(int)Skill.Skills.PinkS]);
        skillList[(int)Skill.Skills.OrangeS] = new Skill(Skill.Skills.OrangeS, "Orange skill", skillSprites[(int)Skill.Skills.OrangeS]);
        skillList[(int)Skill.Skills.NoSkill] = new Skill(Skill.Skills.NoSkill, "-", skillSprites[(int)Skill.Skills.NoSkill]);
        skillList[(int)Skill.Skills.NoLimb] = new Skill(Skill.Skills.NoLimb, "x", skillSprites[(int)Skill.Skills.NoLimb]);
    }

    private void SetupResetMenu(bool randomSkills)
    {
        // Progressively activate the skill selection slots when the player resets
        if (numberOfResets == 0)
        {
            // With no resets, only the first skill selection slot is active
            skillSlotImagesR[1].enabled = false;
            skillSlotImagesR[2].enabled = false;
        }
        else if (numberOfResets == 1)
        {
            // With one reset, the first and second skill selection slots are active
            skillSlotImagesR[1].enabled = true;
            skillSlotImagesR[2].enabled = false;
        }
        else if (numberOfResets >= 2)
        {
            // With 2 or more resets, all the skill selection slots are active
            skillSlotImagesR[1].enabled = true;
            skillSlotImagesR[2].enabled = true;
        }
        // Set the skill slot sprites to empty
        for (int i = 0; i < skillSlotsCount; i++)
        {
            skillSlotImagesR[i].sprite = skillSprites[(int)Skill.Skills.NoSkill];
        }
        // Set the skill selection slots to color white and enable the trigger, so they can be clicked
        for (int j = 0; j < skillChoicesCount; j++)
        {
            skillChoiceImages[j].color = Color.white;
            skillChoiceImages[j].GetComponent<EventTrigger>().enabled = true;
        }
        // If the random skills flag is on
        if (randomSkills)
        {
            // Select new skills to show
            ShowRandomSkills();
        }
        // Disable the reset and confirm choices buttons
        resetChoicesButton.enabled = false;
        confirmChoicesButton.enabled = false;
    }

    private void ShowRandomSkills()
    {
        // Initialize the skills flag list to make the selection
        bool [] selectedSkillFlags = new bool[Skill.totalNumberOfSkills];
        for (int i = 0; i < Skill.totalNumberOfSkills; i++)
        {
            // Set all the flags false to make them selectable
            selectedSkillFlags[i] = false;
        }
        for (int j = 0; j < skillChoicesCount; j++)
        {
            // Get a random number between 0 and the amount of skills
            int rndS = Random.Range(0, Skill.totalNumberOfSkills);
            bool notSelected = true;
            // While we didn't select a skill to show in the reset menu
            while (notSelected)
            {
                // If the skill selected is not previously selected
                if (!selectedSkillFlags[rndS])
                {
                    // Select the corresponding skill
                    Skill skill = skillList[rndS];
                    // Save the skill in the skill choices list
                    skillChoices[j] = skill;
                    // Set the corresponding flag to indicate the skill is selected
                    selectedSkillFlags[rndS] = true;
                    // Set the skill choice sprite to the corresponding skill sprite
                    skillChoiceImages[j].sprite = skill.icon;
                    // Set the skill choice description text to the corresponding skill description
                    skillChoiceTexts[j].SetText(skill.description);
                    // Update the flag to indicate we selected a skill
                    notSelected = false;
                }
                else
                {
                    // If the skill was selected previously, we try again with another selected at random
                    rndS = Random.Range(0, Skill.totalNumberOfSkills);
                }
            }
        }
    }

    /**
     * This method is called from the reset panel when the player ckicks on one of the skill choices
     */
    public void SelectSkill(int choice)
    {
        // Get the corresponding skill choice that was clicked by the player in the reset menu
        Skill selectedSkill = skillChoices[choice];
        // Depending on the number of resets, the amount of skills the player can select
        if (numberOfResets == 0)
        {
            // If the first skill slot is free
            if (skillSlots[0] == skillList[(int)Skill.Skills.NoLimb])
            {
                // Setup the skill choice
                SetupSkillChoiceUI(0, choice, selectedSkill);
                // Activate the reset choices button, so the player can change his choices if they want
                resetChoicesButton.enabled = true;
                // Activate the confirm choices button
                confirmChoicesButton.enabled = true;
            }
            // Set the remaining skill slots to empty
            skillSlots[1] = skillList[(int)Skill.Skills.NoSkill];
            skillSlots[2] = skillList[(int)Skill.Skills.NoSkill];
        }
        else if (numberOfResets == 1)
        {
            // If the first skill slot is free
            if (skillSlots[0] == skillList[(int)Skill.Skills.NoLimb])
            {
                // Setup the skill choice
                SetupSkillChoiceUI(0, choice, selectedSkill);
                // Activate the reset choices button, so the player can change his choices if they want
                resetChoicesButton.enabled = true;
            }
            // If the second skill slot is free
            else if (skillSlots[1] == skillList[(int)Skill.Skills.NoLimb])
            {
                // Setup the skill choice
                SetupSkillChoiceUI(1, choice, selectedSkill);
                // Activate the confirm choices button
                confirmChoicesButton.enabled = true;
            }
            // Set the remaining skill slots to empty
            skillSlots[2] = skillList[(int)Skill.Skills.NoSkill];
        }
        else if (numberOfResets >= 2)
        {
            // If the first skill slot is free
            if (skillSlots[0] == skillList[(int)Skill.Skills.NoLimb])
            {
                // Setup the skill choice
                SetupSkillChoiceUI(0, choice, selectedSkill);
                // Activate the reset choices button, so the player can change his choices if they want
                resetChoicesButton.enabled = true;
            }
            // If the second skill slot is free
            else if (skillSlots[1] == skillList[(int)Skill.Skills.NoLimb])
            {
                // Setup the skill choice
                SetupSkillChoiceUI(1, choice, selectedSkill);
            }
            // If the third skill slot is free
            else if (skillSlots[2] == skillList[(int)Skill.Skills.NoLimb])
            {
                // Setup the skill choice
                SetupSkillChoiceUI(2, choice, selectedSkill);
                // Activate the confirm choices button
                confirmChoicesButton.enabled = true;
            }
        }
    }

    private void SetupSkillChoiceUI(int skSlot, int skChoice, Skill skill)
    {
        // Assign the currently selected skill to the corresponding slot
        skillSlots[skSlot] = skill;
        // Save the skill in the backup to restore later
        skillSlotsBackup[skSlot] = skill;
        // Set the skill slot sprite in the reset menu with the selected skill slot sprite
        skillSlotImagesR[skSlot].sprite = skill.icon;
        // Change the color of the skill choice slot to indicate it was used
        skillChoiceImages[skChoice].color = Color.gray;
        // Disable the trigger for the skill choice slot to prevent further use
        skillChoiceImages[skChoice].GetComponent<EventTrigger>().enabled = false;
    }

    /**
     * This method is called from the reset panel when the player ckicks on the reset choices button
     */
    public void ResetChoices()
    {
        // Reset the skill slots to no limb, so the player can select new skills
        for (int i = 0; i < skillSlotsCount; i++)
        {
            skillSlots[i] = skillList[(int)Skill.Skills.NoLimb];
            skillSlotsBackup[i] = skillList[(int)Skill.Skills.NoLimb];
        }
        // Setup the reset menu, without rerolling the skills showed
        SetupResetMenu(false);
    }

    /**
     * This method is called from the reset panel when the player ckicks on the confirm and respawn button
     */
    public void ConfirmAndRespawn()
    {
        // Set the in-game skill slots sprites to the selected skills sprites
        for (int i = 0; i < skillSlotsCount; i++)
        {
            skillSlotImagesG[i].sprite = skillSlots[i].icon;
        }
        // Hide the reset menu
        resetPanel.SetActive(false);
        // Signal the player to indicate the respawn is over
        pc.SetOnRespawn(false);
        // Increase the number of resets
        numberOfResets++;
    }

    /**
     * This method is called from the player controller when the player reaches an obstacle
     */
    public bool ManageObstacle(ObstacleTile obstacle, Vector3Int obstaclePos)
    {
        // If the player picked up the shield item
        if (shieldOn)
        {
            // He can ignore the obstacles
            return true;
        }
        // Save the current obstacle position
        currentObstaclePosition = obstaclePos;
        // Get the obstacle type
        ObstacleTile.Obstacles obs = obstacle.obstacleName;
        // Check if the player has the skill to pass the obstacle
        bool canMove = CheckSkills(obs);
        if (!canMove)
        {
            // If the player don't have the skill, show the obstacle panel to let them decide what to do
            obstaclePanel.SetActive(true);
            // Signal the player to stop moving while deciding
            pc.SetOnObstacle(true);
        }
        else
        {
            // If the player has the skill, let him pass
            pc.SetObstaclePassed();
        }
        // Return the flag to move/don't move
        return canMove;
    }

    private bool CheckSkills(ObstacleTile.Obstacles obstacle)
    {
        // Check if the player has the skill to pass the obstacle
        for (int i = 0; i < skillSlotsCount; i++)
        {
            // Compare the skills with the obstacles
            if ((byte)skillSlots[i].name == (byte)obstacle)
            {
                // If the player has the skill, save the skill slot used
                skillUsed = skillSlotImagesG[i];
                return true;
            }
        }
        return false;
    }

    /**
     * This method is called from the obstacle panel when the player clicks the continue button
     */
    public void CrossObstacle()
    {
        // Hide the obstacle panel
        obstaclePanel.SetActive(false);
        // Signal the player that the decision is over, so it can move again
        pc.SetOnObstacle(false);
        // If the player has any limb left
        if (limbsLeft > 0)
        {
            // Set the player position on the obstacle to cross it
            pc.CrossObstacle(currentObstaclePosition);
            // Get a random number between 1 and 100
            int rnd = Random.Range(1, 101);
            // If it's 0, the player loses a limb
            if (rnd > safePassageProbability)
            {
                // The order of limb loosing is from first to last
                // If the limb exists
                if (skillSlots[0] != skillList[(byte)Skill.Skills.NoLimb])
                {
                    // Its lost
                    skillSlots[0] = skillList[(byte)Skill.Skills.NoLimb];
                    // And set the skill sprite to no limb to indicate is off
                    skillSlotImagesG[0].sprite = skillSlots[0].icon;
                }
                // Same with the others...
                else if (skillSlots[1] != skillList[(byte)Skill.Skills.NoLimb])
                {
                    skillSlots[1] = skillList[(byte)Skill.Skills.NoLimb];
                    skillSlotImagesG[1].sprite = skillSlots[1].icon;
                }
                else if (skillSlots[2] != skillList[(byte)Skill.Skills.NoLimb])
                {
                    skillSlots[2] = skillList[(byte)Skill.Skills.NoLimb];
                    skillSlotImagesG[2].sprite = skillSlots[2].icon;
                }
                // Discount one limb to the counter
                limbsLeft--;
                // Show the fail crossing panel to indicate the player, he crosses at the cost of one limb
                StartCoroutine(ShowAndHidePanel(failPanel));
            }
            // If it's 1, the player is lucky and can cross without loosing a limb
            else
            {
                // Show the success crossing panel to indicate the player, he crosses safely
                StartCoroutine(ShowAndHidePanel(successPanel));
            }
        }
        // If the player has no limbs left
        else
        {
            // Setup the reset panel with new random skills
            SetupResetMenu(true);
            // Show the reset panel, to let them choose the new skills to respawn
            resetPanel.SetActive(true);
            // Signal the player stop moving while the reset is on
            pc.SetOnRespawn(true);
            // Reset the player position to the beggining of the map
            pc.Retreat();
            // Restore all the limbs lost
            limbsLeft = totalNumberOfLimbs;
        }
    }

    /**
     * This method is called from the obstacle panel when the player clicks the go back button
     */
    public void GoBack()
    {
        // The player stays in the same position, in front of the obstacle
        // Hide the obstacle panel
        obstaclePanel.SetActive(false);
        // Let the player move
        pc.SetOnObstacle(false);
    }

    /**
     * This method is called from the player controller when the player passes an obstacle using a skill
     */
    public void ObstaclePassed()
    {
        // Change the skill slot color for a second to indicate it was used
        StartCoroutine(FlashSkillUsed(skillUsed));
        // Show the obstacle crossed successfully panel
        StartCoroutine(ShowAndHidePanel(successPanel));
    }

    private IEnumerator ShowAndHidePanel(GameObject targetPanel)
    {
        // Show the target panel
        targetPanel.SetActive(true);
        // Wait for an amount of seconds
        yield return new WaitForSeconds(2.0f);
        // Hide the target panel
        targetPanel.SetActive(false);
    }

    private IEnumerator FlashSkillUsed(Image target)
    {
        // Change the color of the target image
        target.color = Color.black;
        // Wait for an amount of seconds
        yield return new WaitForSeconds(0.5f);
        // Change the color of the target image to its original color
        target.color = Color.white;
    }

    public void GameOver()
    {
        // Show the game over panel
        gameOverPanel.SetActive(true);
        // Show the score
        scoreText.SetText("" + numberOfResets);
    }

    public void PickItemUp(Item item)
    {
        // Show the information panel with the item description
        informationText.SetText(item.GetDescription());
        StartCoroutine(ShowAndHidePanel(informationPanel));
        // Depending on the type of item, apply the corresponding effect
        switch (item.itemName)
        {
            case Item.Items.CompassFood:
                StartCoroutine(ShowCompass());
                break;
            case Item.Items.RegenerationFood:
                RegenerateLimb();
                break;
            case Item.Items.ExplorationFood:
                StartCoroutine(IncreaseExplorationRadius());
                break;
            case Item.Items.ShieldFood:
                StartCoroutine(ShowShield());
                break;
            case Item.Items.LoreScroll:
                pc.SetOnLore(true);
                lorePanel.SetActive(true);
                loreText.SetText(item.GetLore());
                break;
        }
    }

    private IEnumerator ShowCompass()
    {
        // Activate the compass
        compassOn = true;
        // Show the compass
        compass.SetActive(true);
        // Wait for the item duration time
        yield return new WaitForSeconds(itemDuration);
        // Deactivate the compass
        compassOn = false;
        // Hide the compass
        compass.SetActive(false);
    }

    private IEnumerator ShowShield()
    {
        // Activate the shield
        shieldOn = true;
        // Show the shield
        shield.SetActive(true);
        // Wait for the item duration time
        yield return new WaitForSeconds(itemDuration);
        // Deactivate the shield
        shieldOn = false;
        // Hide the shield
        shield.SetActive(false);
    }

    private IEnumerator IncreaseExplorationRadius()
    {
        // Save the original exploration radius of the player
        int originalRadius = pc.fogRevealRadius;
        // Increase the exploration radius of the player
        pc.SetExplorationRadius(5);
        // Wait for the item duration time
        yield return new WaitForSeconds(itemDuration);
        // Restore the exploration radius of the player to its original value
        pc.SetExplorationRadius(originalRadius);
    }

    private void RegenerateLimb()
    {
        // The order of regeneration is from last to first
        // We check if the limb is lost
        if (skillSlots[2] == skillList[(byte)Skill.Skills.NoLimb])
        {
            // Restore the skill that was selected in that limb
            skillSlots[2] = skillSlotsBackup[2];
            // And set the skill sprite to the corresponding one
            skillSlotImagesG[2].sprite = skillSlotsBackup[2].icon;
            // Increase the limb counter
            limbsLeft++;
        }
        // Same with the others...
        else if (skillSlots[1] == skillList[(byte)Skill.Skills.NoLimb])
        {
            skillSlots[1] = skillSlotsBackup[1];
            skillSlotImagesG[1].sprite = skillSlotsBackup[1].icon;
            limbsLeft++;
        }
        else if (skillSlots[0] == skillList[(byte)Skill.Skills.NoLimb])
        {
            skillSlots[0] = skillSlotsBackup[0];
            skillSlotImagesG[0].sprite = skillSlotsBackup[0].icon;
            limbsLeft++;
        }
    }

    public void CloseLorePanel()
    {
        // Hide the lore panel and signal the player to move again
        lorePanel.SetActive(false);
        pc.SetOnLore(false);
    }

    private void SpawnItems(Tilemap layer, int spawnQuantity, bool scroll)
    {
        // Get the boundaries of the layer
        BoundsInt layerBounds = layer.cellBounds;
        // Get the positions, tiles and amount of the markers where the items can be spawned
        int itemPositionsCount = layer.GetTiles(layerBounds, out Tilemap.PositionArray positions, out Tilemap.TileArray tiles);
        // We create a hash set to store the positions of the spawned items, to avoid spawning multiple items in the same one
        HashSet<Vector3Int> spawnedItems = new HashSet<Vector3Int>();
        // We will only spawn a limited number of items
        for (int i = 0; i < spawnQuantity; i++)
        {
            // Get a random index of the positions array
            int rndIndex = Random.Range(0, itemPositionsCount);
            // Set up a flag for repeating the random selection until we find a free position
            bool spawned = false;
            // Get the cell position of the selected index
            Vector3Int itemCellPos = positions[rndIndex];
            // While we haven't found a free position to spawn the item
            while (!spawned)
            {
                // If the position is not already used, we can spawn the item there
                if (!spawnedItems.Contains(itemCellPos))
                {
                    spawned = true;
                    spawnedItems.Add(itemCellPos);
                }
                // If the position is already used, we select another random position
                else
                {
                    rndIndex = Random.Range(0, itemPositionsCount);
                    itemCellPos = positions[rndIndex];
                }
            }
            // Convert the selected cell position to world position
            Vector3 itemWorldPos = layer.GetCellCenterWorld(itemCellPos);
            // If we are spawning lore scrolls
            if (scroll)
            {
                // Instantiate it in that position
                Instantiate(itemPrefabs[(int)Item.Items.LoreScroll], itemWorldPos, Quaternion.identity);
            }
            else
            {
                // Instantiate a random item in that position
                Instantiate(itemPrefabs[GetRandomItem()], itemWorldPos, Quaternion.identity);
            }
            
        }
        // Dispose the positions and tiles arrays to free memory
        positions.Dispose();
        tiles.Dispose();
    }

    private void RandomizeObstacles()
    {
        // Decide if randomize or use the manual setting of obstacles
        int rnd = Random.Range(0, 2);
        if (rnd == 0)
        {
            return;
        }
        // Get the boundaries of the layer
        BoundsInt layerBounds = obstacleLayer.cellBounds;
        // Get the positions, tiles and amount of obstacles
        int obstaclePositionsCount = obstacleLayer.GetTiles(layerBounds, out Tilemap.PositionArray positions, out Tilemap.TileArray tiles);
        // Iterate trough the obstacle positions
        for (int i = 0; i < obstaclePositionsCount; i++)
        {
            // Get a random index of the obstacle tiles list
            int rndIndex = Random.Range(0, obstacleTilesList.Length);
            // Set the random obstacle in the obstacle layer position
            obstacleLayer.SetTile(positions[i], obstacleTilesList[rndIndex]);
        }
        // Dispose the positions and tiles arrays to free memory
        positions.Dispose();
        tiles.Dispose();
    }

    private int GetRandomItem()
    {
        // Get a random number between 0 and the total spawn chance of all the items
        float rnd = Random.Range(0f, Item.totalSpawnChance);
        float weightSum = 0f;
        // Iterate through the items to select one based on its spawn chance
        for (int i = 0; i < Item.totalItemCount; i++)
        {
            // Add the spawn chance of the current item to the weight sum
            weightSum += itemPrefabs[i].GetComponent<Item>().GetSpawnChance();
            // If the random number is less than or equal to the weight sum, return the index of the current item
            if (rnd <= weightSum)
            {
                return i;
            }
        }
        // If no item was selected, return -1 (not going to happen)
        return -1;
    }

    public void SelectMap()
    {
        // Initialize the objective positions list
        startPositionList = new Vector3Int[3];
        endPositionList = new Vector3Int[3];
        // Save the starting positions for the different map configurations
        startPositionList[0] = new Vector3Int(-1, -9, 0);
        startPositionList[1] = new Vector3Int(2, -9, 0);
        startPositionList[2] = new Vector3Int(5, -11, 0);
        // Save the ending positions for the different map configurations
        endPositionList[0] = new Vector3Int(2, 44, 0);
        endPositionList[1] = new Vector3Int(5, 44, 0);
        endPositionList[2] = new Vector3Int(35, 34, 0);
        // Select a random number to choose one of the map configurations
        int rnd = Random.Range(0, 3);
        // Setup the player layers with the corresponding configuration
        pc.SetupLayers(groundLayerList[rnd], obstacleLayerList[rnd], wallLayerList[rnd], fogLayerList[rnd], objectiveLayerList[rnd]);
        // Setup the used layers with the corresponding configuration
        obstacleLayer = obstacleLayerList[rnd];
        objectiveLayer = objectiveLayerList[rnd];
        itemLayer = itemLayerList[rnd];
        loreLayer = loreLayerList[rnd];
        // Setup the corresponding starting and ending positions
        startPosition = startPositionList[rnd];
        endPosition = endPositionList[rnd];
        // Set the player start and end positions
        pc.SetObjectivePositions(startPosition, endPosition);
    }

    public bool LostLimb()
    {
        return limbsLeft < totalNumberOfLimbs;
    }
}
