using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Skill[] skillList; // List of all the skills in the game
    public Sprite[] skillSprites; // List of all skill sprites

    private Skill[] skillSlots; // List of the skills the player has equipped
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

    public PlayerController pc; // Reference to the player

    private Vector3Int currentObstaclePosition; // Position of the current ecountered obstacle
    private Image skillUsed; // Skill slot of the skill used to cross an obstacle safely

    private int limbsLeft = 3; // Number of limbs left

    public GameObject compass;
    private bool compassOn = true;

    void Start()
    {
        // Create all the skills of the game
        InitializeSkillList();
        // Create the arrays for the skill slots and skill choices
        skillSlots = new Skill[skillSlotsCount];
        skillChoices = new Skill[skillChoicesCount];
        // At the start of the game the player has no skills
        for (int i = 0; i < skillSlotsCount; i++)
        {
            skillSlots[i] = skillList[(int)Skill.Skills.NoSkill];
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
    }

    private void Update()
    {
        if (compassOn)
        {
            RotateTowardsEnd();
        }
    }

    private void RotateTowardsEnd()
    {
        // Get the position of the player and the end position
        Vector3 playerPos = pc.transform.position;
        Vector3 endPos = pc.objectiveLayer.GetCellCenterWorld(pc.endPosition);
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
        // If the second skill slot is free
        else if (skillSlots[2] == skillList[(int)Skill.Skills.NoLimb])
        {
            // Setup the skill choice
            SetupSkillChoiceUI(2, choice, selectedSkill);
            // Activate the confirm choices button
            confirmChoicesButton.enabled = true;
        }
    }

    private void SetupSkillChoiceUI(int skSlot, int skChoice, Skill skill)
    {
        // Assign the currently selected skill to the corresponding slot
        skillSlots[skSlot] = skill;
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
    }

    /**
     * This method is called from the player controller when the player reaches an obstacle
     */
    public bool ManageObstacle(ObstacleTile obstacle, Vector3Int obstaclePos)
    {
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
            // Get a random number between 0 and 1
            int rnd = Random.Range(0, 2);
            // If it's 0, the player loses a limb
            if (rnd == 0)
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
            limbsLeft = 3;
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
    }

    public void PickItemUp()
    {

    }
}
