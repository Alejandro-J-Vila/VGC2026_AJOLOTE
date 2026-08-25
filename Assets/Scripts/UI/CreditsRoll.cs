using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsRoll : MonoBehaviour
{
    // Enum to define the type of panel that is going to scroll
    public enum PanelType : byte { Credits, Victory, Defeat, Story }
    public float scrollSpeed = 120f; // Rolling panel speed
    public PanelType panelType; // Type of panel that is going to scroll
    private RectTransform rollingPanel; // Panel that is going to scroll
    private int nextSceneIndex = 2; // Level scene index

    void Start()
    {
        // Get the reference to the scrolling panel
        rollingPanel = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        // Depending on the type of panel, perform specific tasks
        if (panelType == PanelType.Credits)
        {
            RollCredits();
        }
        if (panelType == PanelType.Victory)
        {
            RollVictory();
        }
        if (panelType == PanelType.Defeat)
        {
            RollDefeat();
        }
        if (panelType == PanelType.Story)
        {
            RollStory();
        }

    }

    private void RollCredits()
    {
        // Move the panel up
        rollingPanel.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        // When the panel reach the final position, reset it to the initial position
        if (rollingPanel.anchoredPosition.y >= 3526)
        {
            rollingPanel.anchoredPosition -= new Vector2(0, 4585);
        }
    }

    private void RollStory()
    {
        // Move the panel up
        rollingPanel.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        // When the panel reach the final position, load the game scene
        if (rollingPanel.anchoredPosition.y >= 2400)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    private void RollVictory()
    {
        // When the panel reach the final position, stop moving
        if (rollingPanel.anchoredPosition.y < 77)
        {
            // Move the panel up
            rollingPanel.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }
    
    private void RollDefeat()
    {
        // When the panel reach the final position, stop moving
        if (rollingPanel.anchoredPosition.y < 116)
        {
            // Move the panel up
            rollingPanel.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }
}
