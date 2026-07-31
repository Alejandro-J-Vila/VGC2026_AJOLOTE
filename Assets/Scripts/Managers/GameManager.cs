using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Enum that represents all of the different obstacles in game
    public enum Obstacle : byte { RedO, GreenO, BlueO, YellowO, VioletO, OrangeO }
    // Enum that represent all of the different skills in game
    public enum Skill : byte { RedS, GreenS, BlueS, YellowS, VioletS, OrangeS, NoSkill, NoLimb }

    private Skill skill_1; // First player skill
    private Skill skill_2; // Second player skill
    private Skill skill_3; // Third player skill

    public Image skillImage_1; // UI image that represent the first skill the player has in game
    public Image skillImage_2; // UI image that represent the second skill the player has in game
    public Image skillImage_3; // UI image that represent the third skill the player has in game

    public Image selectedSkillImage_1; // Image that represent the first selected skill in the reset menu
    public Image selectedSkillImage_2; // Image that represent the second selected skill in the reset menu
    public Image selectedSkillImage_3; // Image that represent the third selected skill in the reset menu

    private bool slot1Free; // Flag to indicate if the first skill slot has no skill
    private bool slot2Free; // Flag to indicate if the second skill slot has no skill
    private bool slot3Free; // Flag to indicate if the third skill slot has no skill

    private int skillCount = 6; // Total number of skills in the game
    private bool[] selectedSkills; // Flag list for selecting random skills in the reset menu

    private Skill skillChoice_1; // First skill choice
    private Skill skillChoice_2; // Second skill choice
    private Skill skillChoice_3; // Third skill choice
    private Skill skillChoice_4; // FOurth skill choice

    public Image skillChoiceImage_1; // Image that represent the first skill choice that the player can make in the reset menu
    public Image skillChoiceImage_2; // Image that represent the second skill choice that the player can make in the reset menu
    public Image skillChoiceImage_3; // Image that represent the third skill choice that the player can make in the reset menu
    public Image skillChoiceImage_4; // Image that represent the fourth skill choice that the player can make in the reset menu

    public TMP_Text skillChoiceText_1; // UI text for the description of the first skill choice
    public TMP_Text skillChoiceText_2; // UI text for the description of the second skill choice
    public TMP_Text skillChoiceText_3; // UI text for the description of the third skill choice
    public TMP_Text skillChoiceText_4; // UI text for the description of the fourth skill choice

    private Skill currentSelectedSkill; // Current selected skill by the player in the reset menu 

    // Temporary list of skill descriptions, this will go to the text manager later
    private string[] skillDescriptions = new string[] { "Red skill", "Green skill", "Blue skill", "Yellow skill", "Violet skill", "Orange skill" };
    public Sprite[] skillSprites; // List of all skill sprites

    public GameObject resetPanel; // Reference to the reset panel
    public Button resetChoicesButton; // Reference to the reset choice selection button in the reset menu
    public Button confirmChoicesButton; // Reference to the confirm choices button in the reset menu

    public GameObject obstaclePanel; // Reference to the obstacle panel used when an obstacle is found
    public GameObject successPanel; // Reference to the success panel used when crossing an obstacle succesfully
    public GameObject failPanel; // Reference to the fail panel used when failing to cross an obstacle
    public GameObject gameOverPanel; // Reference to the game over panel used when the player reach the end of the map

    public PlayerController pc; // Reference to the player

    private Obstacle obstacleEncountered; // Type of the current obstacle encountered
    private Vector3Int currentObstaclePosition; // Position of the current ecountered obstacle
    private Image skillUsed; // Skill slot of the skill used to cross an obstacle safely

    private int limbsLeft = 3; // Number of limbs left
    private bool limb1OK = true; // Limb 1 status
    private bool limb2OK = true; // Limb 2 status
    private bool limb3OK = true; // Limb 3 status

    void Start()
    {
        // At the start of the game the player has no skills
        skill_1 = Skill.NoSkill;
        skill_2 = Skill.NoSkill;
        skill_3 = Skill.NoSkill;
        skillImage_1.sprite = skillSprites[(int)skill_1];
        skillImage_2.sprite = skillSprites[(int)skill_2];
        skillImage_3.sprite = skillSprites[(int)skill_3];
        // Setup all the components of the reset menu
        SetupResetMenu(false);
        // Hide all the panels used later
        resetPanel.SetActive(false);
        obstaclePanel.SetActive(false);
        successPanel.SetActive(false);
        failPanel.SetActive(false);
        gameOverPanel.SetActive(false);
    }

    private void SetupResetMenu(bool randomSkills)
    {
        // Set the free skill slots flag to indicate there are no skills selected
        slot1Free = true;
        slot2Free = true;
        slot3Free = true;
        // Set the skill slot sprites to empty
        selectedSkillImage_1.sprite = skillSprites[(int)Skill.NoSkill];
        selectedSkillImage_2.sprite = skillSprites[(int)Skill.NoSkill];
        selectedSkillImage_3.sprite = skillSprites[(int)Skill.NoSkill];
        // Set the skill selection slots to color white and enable the trigger, so they can be clicked
        skillChoiceImage_1.color = Color.white;
        skillChoiceImage_1.GetComponent<EventTrigger>().enabled = true;
        skillChoiceImage_2.color = Color.white;
        skillChoiceImage_2.GetComponent<EventTrigger>().enabled = true;
        skillChoiceImage_3.color = Color.white;
        skillChoiceImage_3.GetComponent<EventTrigger>().enabled = true;
        skillChoiceImage_4.color = Color.white;
        skillChoiceImage_4.GetComponent<EventTrigger>().enabled = true;
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
        selectedSkills = new bool[skillCount];
        for (int i = 0; i < skillCount; i++)
        {
            // Set all the flags false to make them selectable
            selectedSkills[i] = false;
        }
        // Select random skills to show in the reset menu
        SelectRandomSkill(ref skillChoice_1, skillChoiceImage_1, skillChoiceText_1);
        SelectRandomSkill(ref skillChoice_2, skillChoiceImage_2, skillChoiceText_2);
        SelectRandomSkill(ref skillChoice_3, skillChoiceImage_3, skillChoiceText_3);
        SelectRandomSkill(ref skillChoice_4, skillChoiceImage_4, skillChoiceText_4);
    }

    /**
     * This method is called from the reset panel when the player ckicks on one of the selection skills
     */
    public void SelectSkill(int choice)
    {
        // Get the corresponding skill choice that was clicked by the player in the reset menu
        Image skillChoice = GetSkillChoice(choice);
        // If the first skill slot is free
        if (slot1Free)
        {
            // Assign the currently selected skill to the first slot
            skill_1 = currentSelectedSkill;
            // Set the skill slot to contain the selected skill and update the flag
            slot1Free = SetupSkillChoiceUI(selectedSkillImage_1, skillChoice);
            // Activate the reset choices button, so the player can change his choices if they want
            resetChoicesButton.enabled = true;
            return;
        }
        // If the second skill slot is free
        if (slot2Free)
        {
            // We do the same as before, but without the reset button because it's already active
            skill_2 = currentSelectedSkill;
            slot2Free = SetupSkillChoiceUI(selectedSkillImage_2, skillChoice);
            return;
        }
        // And the same with the third, but enabling the confirm choices button
        if (slot3Free)
        {
            skill_3 = currentSelectedSkill;
            slot3Free = SetupSkillChoiceUI(selectedSkillImage_3, skillChoice);
            confirmChoicesButton.enabled = true;
            return;
        }
    }

    private bool SetupSkillChoiceUI(Image skSlot, Image selSlot)
    {
        // Set the skill slot sprite with the selected skill slot sprite
        skSlot.sprite = selSlot.sprite;
        // Change the color of the selected skill slot to indicate it was used
        selSlot.color = Color.gray;
        // Disable the trigger for the selected skill slot to prevent further use
        selSlot.GetComponent<EventTrigger>().enabled = false;
        // Return false to indicate the skill slot is no longer free
        return false;
    }

    /**
     * This method is called from the reset panel when the player ckicks on the reset choices button
     */
    public void ResetChoices()
    {
        // Setup the reset menu, without rerolling the skills showed
        SetupResetMenu(false);
    }

    /**
     * This method is called from the reset panel when the player ckicks on the confirm and respawn button
     */
    public void ConfirmAndRespawn()
    {
        // Set the in-game skill slots sprites to the selected skills sprites
        skillImage_1.sprite = selectedSkillImage_1.sprite;
        skillImage_2.sprite = selectedSkillImage_2.sprite;
        skillImage_3.sprite = selectedSkillImage_3.sprite;
        // Hide the reset menu
        resetPanel.SetActive(false);
        // Signal the player to indicate the respawn is over
        pc.SetOnRespawn(false);
    }

    private Image GetSkillChoice(int id)
    {
        // Depending on the given id
        switch (id)
        {
            // Save the currently selected skill and return the corresponding skill choice image
            case 1:
                {
                    currentSelectedSkill = skillChoice_1;
                    return skillChoiceImage_1;
                }   
            case 2:
                {
                    currentSelectedSkill = skillChoice_2;
                    return skillChoiceImage_2;
                }
            case 3:
                {
                    currentSelectedSkill = skillChoice_3;
                    return skillChoiceImage_3;
                }
            case 4:
                {
                    currentSelectedSkill = skillChoice_4;
                    return skillChoiceImage_4;
                }
        }
        // If the id is not valid, return null
        return null;
    }

    private void SelectRandomSkill(ref Skill skill, Image skillImg, TMP_Text skillTxt)
    {
        // Get a random number between 0 and the amount of skills
        int rndS = Random.Range(0, skillCount);
        bool notSelected = true;
        // While we didn't select a skill to show in the reset menu
        while (notSelected)
        {
            // If the skill selected is not previously selected
            if (!selectedSkills[rndS])
            {
                // Select the corresponding skill, using the skills enum value
                skill = (Skill)rndS;
                // Set the corresponding flag to indicate the skill is selected
                selectedSkills[rndS] = true;
                // Set the skill choice sprite to the corresponding skill sprite
                skillImg.sprite = skillSprites[rndS];
                // Set the skill choice description text to the corresponding skill description
                skillTxt.SetText(skillDescriptions[rndS]);
                // Update the flag to indicate we selected a skill
                notSelected = false;
            }
            else
            {
                // If the skill was selected previously, we try again with another selected at random
                rndS = Random.Range(0, skillCount);
            }
        }
    }

    /**
     * This method is called from the player controller when the player reaches an obstacle
     */
    public bool ManageObstacle(string obsName, Vector3Int obsPos)
    {
        // Save the current obstacle position
        currentObstaclePosition = obsPos;
        // Get the obstacle type
        GetObstacleType(obsName);
        // Check if the player has the skill to pass the obstacle
        bool canMove = CheckSkills();
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

    private void GetObstacleType(string name)
    {
        switch (name)
        {
            // Depending on the obstacle name, set the corresponding obstacle type
            case "obstacle_1":
                {
                    obstacleEncountered = Obstacle.RedO;
                    break;
                }
            case "obstacle_2":
                {
                    obstacleEncountered = Obstacle.GreenO;
                    break;
                }
            case "obstacle_3":
                {
                    obstacleEncountered = Obstacle.BlueO;
                    break;
                }
            case "obstacle_4":
                {
                    obstacleEncountered = Obstacle.YellowO;
                    break;
                }
            case "obstacle_5":
                {
                    obstacleEncountered = Obstacle.VioletO;
                    break;
                }
            case "obstacle_7":
                {
                    obstacleEncountered = Obstacle.OrangeO;
                    break;
                }
        }
    }

    private bool CheckSkills()
    {
        // Get the underline representation of the skills and obstacles
        byte s1 = (byte)skill_1;
        byte s2 = (byte)skill_2;
        byte s3 = (byte)skill_3;
        byte obs = (byte)obstacleEncountered;
        // Compare each skill with the obstacle
        // If one of the skills is the same type as the obstacle
        if (s1 == obs)
        {
            // Save the skill for later use, to mark it when the obstacle is passed
            skillUsed = skillImage_1;
            return true;
        }
        if (s2 == obs)
        {
            skillUsed = skillImage_2;
            return true;
        }
        if (s3 == obs)
        {
            skillUsed = skillImage_3;
            return true;
        }
        // If there is no skill of the same type as the obstacle, return false
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
                if (limb1OK)
                {
                    // Set it off
                    limb1OK = false;
                    // Set the skill that the limb had off
                    skill_1 = Skill.NoLimb;
                    // And set the skill sprite to no limb to indicate is off
                    skillImage_1.sprite = skillSprites[(int)skill_1];
                }
                // Same with the other...
                else if (limb2OK)
                {
                    limb2OK = false;
                    skill_2 = Skill.NoLimb;
                    skillImage_2.sprite = skillSprites[(int)skill_2];
                }
                else if (limb3OK)
                {
                    limb3OK = false;
                    skill_3 = Skill.NoLimb;
                    skillImage_3.sprite = skillSprites[(int)skill_3];
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
            limb1OK = true;
            limb2OK = true;
            limb3OK = true;
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
}
