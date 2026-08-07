using UnityEngine;

public class Item : MonoBehaviour
{
    // Enum that represent all of the different items in game
    public enum Items : byte { CompassFood, RegenerationFood, ExplorationFood, ShieldFood, LoreScroll }
    public static float totalSpawnChance = 1.6f; // Total spawn chance for all items;
    public static int totalItemCount = 4; // Total number of items in the game (not counting the lore scrolls)
    public Items itemName; // Name of the item
    private string description; // Description of the item
    private string lore = ""; // Lore of the item

    void Start()
    {
        // Set the description and lore based on the item name
        switch (itemName)
        {
            case Items.CompassFood:
                description = "This special food gives you a better sense of orientation.";
                break;
            case Items.RegenerationFood:
                description = "This special food allows you to instantly regenerate a lost limb.";
                break;
            case Items.ExplorationFood:
                description = "This special food enhances your vision allowing to see further.";
                break;
            case Items.ShieldFood:
                description = "This special food provides temporary protection from harm.";
                break;
            case Items.LoreScroll:
                description = "This ancient scroll contains valuable knowledge.";
                lore = "Placeholder lore";
                break;
        }
    }

    void Update()
    {
        // Simple rotation animation for the item
        transform.Rotate(0, 100f * Time.deltaTime, 0);
    }

    public string GetDescription()
    {
        // Get the item description
        return description;
    }

    public string GetLore()
    {
        // Get the item lore
        return lore;
    }

    public float GetSpawnChance()
    {
        // Depending on the item name, return the spawn chance for that item
        switch (itemName)
        {
            case Items.CompassFood:
                return 0.7f;
            case Items.RegenerationFood:
                return 0.2f;
            case Items.ExplorationFood:
                return 0.4f;
            case Items.ShieldFood:
                return 0.3f;
            default:
                return 0f; // LoreScroll has no spawn chance because it's always spawned
        }
    }
}
