using TMPro;
using UnityEngine;

public class UITextUpdate : MonoBehaviour
{
    private TMP_Text text; // Reference to the text component in the game object
    public TextManager.UIText uiTextId;

    void Start()
    {
        // Get the reference to the text component
        text = this.GetComponent<TMP_Text>();
        // Set the default text of the game object
        text.SetText(TextManager.tm.GetUIText(uiTextId));
    }

    public void UpdateText()
    {
        // Update the text of the game object
        text.SetText(TextManager.tm.GetUIText(uiTextId));
    }
}
