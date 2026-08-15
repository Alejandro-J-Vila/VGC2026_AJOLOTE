using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    private int levelSceneIndex = 2; // Level scene index
    public void EasyDifficulty()
    {
        // Set the game difficulty to easy
        DataManager.difficulty = DataManager.Difficulty.Easy;
        // Load the level
        LoadLevel();
    }

    public void NormalDifficulty()
    {
        // Set the game difficulty to normal
        DataManager.difficulty = DataManager.Difficulty.Normal;
        // Load the level
        LoadLevel();
    }

    public void HardDifficulty()
    {
        // Set the game difficulty to hard
        DataManager.difficulty = DataManager.Difficulty.Hard;
        // Load the level
        LoadLevel();
    }

    private void LoadLevel()
    {
        // Load the level scene
        SceneManager.LoadScene(levelSceneIndex);
    }
}
