using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneByIndex : MonoBehaviour
{
    public void LoadScene(int sceneIndex)
    {
        // Unpause the game
        Time.timeScale = 1;
        // Load a scene based on build index
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadDifficultyScene()
    {
        // Depending on the difficulty, load the corresponding level
        switch (DataManager.difficulty)
        {
            case DataManager.Difficulty.Easy:
                SceneManager.LoadScene(7);
                return;
            case DataManager.Difficulty.Normal:
                SceneManager.LoadScene(8);
                return;
            case DataManager.Difficulty.Hard:
                SceneManager.LoadScene(3);
                return;
        }
    }
}
