using TMPro;
using UnityEngine;

public class CreditsTextUpdate : MonoBehaviour
{
    private TMP_Text text; // Reference to the text component in the game object
    public TextManager.CreditsText creditsTextId;

    void Start()
    {
        // Get the reference to the text component
        text = this.GetComponent<TMP_Text>();
        // Set the default text of the game object
        text.SetText(TextManager.tm.GetCreditsText(creditsTextId));
    }

    public void UpdateText()
    {
        // Get the reference to the text component
        text = this.GetComponent<TMP_Text>();
        // Update the text of the game object
        text.SetText(TextManager.tm.GetCreditsText(creditsTextId));
    }
}
