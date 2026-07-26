using TMPro;
using UnityEngine;

public class ButtonTextUpdate : MonoBehaviour
{
    private TMP_Text text; // Reference to the text component in the game object
    public TextManager.ButtonText buttonTextId;

    void Start()
    {
        // Get the reference to the text component
        text = this.GetComponent<TMP_Text>();
        // Set the default text of the game object
        text.SetText(TextManager.tm.GetButtonText(buttonTextId));
    }

    public void UpdateText()
    {
        // Update the text of the game object
        text.SetText(TextManager.tm.GetButtonText(buttonTextId));
    }
}
