using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Enum that represents all of the different obstacles in game
    public enum Obstacles : byte { RedO, GreenO, BlueO, YellowO, VioletO, OrangeO }
    // Enum that represent all of the different skills in game
    public enum Skills : byte { RedS, GreenS, BlueS, YellowS, VioletS, OrangeS }
    public Image skillSlot_1; // UI image that represent the first skill the player has in game
    public Image skillSlot_2; // UI image that represent the second skill the player has in game
    public Image skillSlot_3; // UI image that represent the third skill the player has in game
    // Temporary list of skill descriptions, this will go to the text manager later
    public string[] skillDescriptions = new string[] { "Red skill", "Green skill", "Blue skill", "Yellow skill", "Violet skill", "Orange skill" };
    public Sprite[] skillSprites; // List of all skill sprites
    public Sprite emptySkillSlot; // Sprite that represent an empty skill slot
    public Image skillSelectionSlot_1; // UI image that represent the first skill choice that the player can make in the reset menu
    public Image skillSelectionSlot_2; // UI image that represent the second skill choice that the player can make in the reset menu
    public Image skillSelectionSlot_3; // UI image that represent the third skill choice that the player can make in the reset menu
    public Image skillSelectionSlot_4; // UI image that represent the fourth skill choice that the player can make in the reset menu
    public Image skillSlot_1R; // UI image that represent the first selected skill in the reset menu
    public Image skillSlot_2R; // UI image that represent the second selected skill in the reset menu
    public Image skillSlot_3R; // UI image that represent the third selected skill in the reset menu
    public TMP_Text skillSelectionText_1; // UI text for the description of the first skill choice
    public TMP_Text skillSelectionText_2; // UI text for the description of the second skill choice
    public TMP_Text skillSelectionText_3; // UI text for the description of the third skill choice
    public TMP_Text skillSelectionText_4; // UI text for the description of the fourth skill choice
    public Button resetChoicesButton; // Reference to the reset choice selection button in the reset menu
    public Button confirmAndRespawnButton; // Reference to the confirm and respawn button in the reset menu
    public GameObject resetPanel; // Reference to the reset panel
    private bool slot1Free; // Flag to indicate if the first skill slot has no skill
    private bool slot2Free; // Flag to indicate if the second skill slot has no skill
    private bool slot3Free; // Flag to indicate if the third skill slot has no skill
    private int skillCount = 6; // Total number of skills in the game
    private bool[] selectedSkills; // Flag list for selecting random skills in the reset menu

    void Start()
    {
        SetupResetMenu();
    }

    private void SetupResetMenu()
    {
        // Set the free skill slots flag to indicate there are no skills selected
        slot1Free = true;
        slot2Free = true;
        slot3Free = true;
        // Initialize the flag list to indicate there are no skills selected
        selectedSkills = new bool[skillCount];
        for (int i = 0; i < skillCount; i++)
        {
            selectedSkills[i] = false;
        }
        // Select random skills to show in the reset menu
        SelectRandomSkill(skillSelectionSlot_1, skillSelectionText_1);
        SelectRandomSkill(skillSelectionSlot_2, skillSelectionText_2);
        SelectRandomSkill(skillSelectionSlot_3, skillSelectionText_3);
        SelectRandomSkill(skillSelectionSlot_4, skillSelectionText_4);
        // Disable the reset choices and confirm and respawn buttons
        resetChoicesButton.enabled = false;
        confirmAndRespawnButton.enabled = false;
        // Disable the reset panel
        //resetPanel.SetActive(false);
    }

    /**
     * This method is called from the reset panel when the player ckicks on one of the selection skills
     */
    public void SelectSkill(int slot)
    {
        // Get the corresponding skill selection slot that was clicked by the player in the reset menu
        Image selectionSlot = GetSelectionSlot(slot);
        // If the first skill slot is free
        if (slot1Free)
        {
            // Set the slot to contain the selected skill and update the flag
            slot1Free = SetupSkillSelectionUI(skillSlot_1R, selectionSlot);
            // Activate the reset choices button, so the player can change his choices if they want
            resetChoicesButton.enabled = true;
            return;
        }
        // If the second skill slot is free
        if (slot2Free)
        {
            // We do the same as before, but without the reset button because it's already active
            slot2Free = SetupSkillSelectionUI(skillSlot_2R, selectionSlot);
            return;
        }
        // And the same with the third, but enabling the confirm and respawn button
        if (slot3Free)
        {
            slot3Free = SetupSkillSelectionUI(skillSlot_3R, selectionSlot);
            confirmAndRespawnButton.enabled = true;
            return;
        }
    }

    private bool SetupSkillSelectionUI(Image slotR, Image slotS)
    {
        // Set the skill slot sprite with the selected skill slot sprite
        slotR.sprite = slotS.sprite;
        // Change the color of the selected skill slot to indicate it was used
        slotS.color = Color.gray;
        // Disable the trigger for the selected skill slot to prevent further use
        slotS.GetComponent<EventTrigger>().enabled = false;
        // Return false to indicate the skill slot is no longer free
        return false;
    }

    /**
     * This method is called from the reset panel when the player ckicks on the reset choices button
     */
    public void ResetChoices()
    {
        // Set the free skill slots flag to indicate there are no skills selected
        slot1Free = true;
        slot2Free = true;
        slot3Free = true;
        // Set the skill slot sprites to empty
        skillSlot_1R.sprite = emptySkillSlot;
        skillSlot_2R.sprite = emptySkillSlot;
        skillSlot_3R.sprite = emptySkillSlot;
        // Set the skill selection slots to color white and enable the trigger, so they can be clicked again
        skillSelectionSlot_1.color = Color.white;
        skillSelectionSlot_1.GetComponent<EventTrigger>().enabled = true;
        skillSelectionSlot_2.color = Color.white;
        skillSelectionSlot_2.GetComponent<EventTrigger>().enabled = true;
        skillSelectionSlot_3.color = Color.white;
        skillSelectionSlot_3.GetComponent<EventTrigger>().enabled = true;
        skillSelectionSlot_4.color = Color.white;
        skillSelectionSlot_4.GetComponent<EventTrigger>().enabled = true;
        // Disable the reset choices and confirm and respawn buttons
        resetChoicesButton.enabled = false;
        confirmAndRespawnButton.enabled = false;
    }

    /**
     * This method is called from the reset panel when the player ckicks on the confirm and respawn button
     */
    public void ConfirmAndRespawn()
    {
        // Set the in-game skill slots sprites to the selected skills sprites
        skillSlot_1.sprite = skillSlot_1R.sprite;
        skillSlot_2.sprite = skillSlot_2R.sprite;
        skillSlot_3.sprite = skillSlot_3R.sprite;
        // Hide the reset menu
        resetPanel.SetActive(false);
    }

    private Image GetSelectionSlot(int id)
    {
        // Depending on the given id
        switch (id)
        {
            // Return the corresponding skill selection slot
            case 1: return skillSelectionSlot_1;
            case 2: return skillSelectionSlot_2;
            case 3: return skillSelectionSlot_3;
            case 4: return skillSelectionSlot_4;
        }
        // If the id is not valid, return null
        return null;
    }

    private void SelectRandomSkill(Image slot, TMP_Text text)
    {
        // Get a random number between 0 and the amount of skills
        int skill = Random.Range(0, skillCount);
        bool notSelected = true;
        // While we didn't select a skill to show in the reset menu
        while (notSelected)
        {
            // If the skill selected is not previously selected
            if (!selectedSkills[skill])
            {
                // Set the corresponding flag to indicate the skill is selected
                selectedSkills[skill] = true;
                // Set the selection slot sprite to skill sprite
                slot.sprite = skillSprites[skill];
                // Set the skill description text to the selected skill description
                text.SetText(skillDescriptions[skill]);
                // Update the flag to indicate we selected a skill
                notSelected = false;
            }
            else
            {
                // If the skill was selected previously, we try again with another selected at random
                skill = Random.Range(0, skillCount);
            }
        }
    }
}
