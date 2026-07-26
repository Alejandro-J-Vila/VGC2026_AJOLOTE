using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public void QuitG()
    {
        // Quit the game
        Application.Quit();
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
