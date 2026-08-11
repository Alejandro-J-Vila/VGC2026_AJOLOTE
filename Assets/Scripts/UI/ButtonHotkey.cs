using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ButtonHotkey : MonoBehaviour
{
    public Key hotkey; // Hotkey to use
    private Button targetButton; // Reference to the button that will be triggered by the hotkey

    void Awake()
    {
        // Get the button component
        targetButton = GetComponent<Button>();
    }

    void Update()
    {
        // Ensure the button can be interacted and properly configured
        if (targetButton == null || !targetButton.enabled || hotkey == Key.None)
        {
            return;
        }
        // Check if the hotkey was pressed
        if (Keyboard.current != null && Keyboard.current[hotkey].wasPressedThisFrame)
        {
            // Trigger the asociated button
            TriggerButton();
        }
    }

    private void TriggerButton()
    {
        // Highlight the button
        targetButton.Select();
        // Invoke the listeners bound to the button
        targetButton.onClick.Invoke();
    }
}
