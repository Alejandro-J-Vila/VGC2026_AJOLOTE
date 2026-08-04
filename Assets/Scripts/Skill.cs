using UnityEngine;

public class Skill
{
    // Enum that represent all of the different skills in game
    public enum Skills : byte { RedS, GreenS, BlueS, YellowS, PinkS, OrangeS, NoSkill, NoLimb }
    public static int totalNumberOfSkills { get; private set; } = 6; // Total number of skills in the game
    public Skills name { get; private set; } // Name of the skill
    public string description { get; private set; } // Description of the skill
    public Sprite icon { get; private set; } // Icon of the skill

    public Skill(Skills skillName, string skillDescription, Sprite skillIcon)
    {
        // Constructor to initialize the skill with its name, description, and icon
        name = skillName;
        description = skillDescription;
        icon = skillIcon;
    }
}
