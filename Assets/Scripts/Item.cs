using UnityEngine;

public class Item : MonoBehaviour
{
    // Enum that represent all of the different items in game
    public enum Items : byte { CompassFood, TeleportFood, ExplorationFood, ShieldFood, LoreScroll }
    public Items itemName; // Name of the item
    private string description; // Description of the item

    void Start()
    {
        if (itemName == Items.CompassFood)
        {
            description = "This special food gives you a better sense of orientation.";
        }
        else if (itemName == Items.TeleportFood)
        {
            description = "This special food allows you to teleport to a random location.";
        }
        else if (itemName == Items.ExplorationFood)
        {
            description = "This special food enhances your vision allowing to see further.";
        }
        else if (itemName == Items.ShieldFood)
        {
            description = "This special food provides temporary protection from harm.";
        }
        else if (itemName == Items.LoreScroll)
        {
            description = "This ancient scroll contains valuable knowledge.";
        }
    }

    void Update()
    {
        transform.Rotate(0, 100f * Time.deltaTime, 0);
    }

    public string GetDescription()
    {
        return description;
    }
}
