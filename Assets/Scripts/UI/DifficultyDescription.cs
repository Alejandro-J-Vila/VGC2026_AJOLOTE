using TMPro;
using UnityEngine;

public class DifficultyDescription : MonoBehaviour
{
    public TMP_Text difficultyText;

    void Start()
    {
        difficultyText.enabled = false;
    }

    public void ShowEasyDescription()
    {
        difficultyText.enabled = true;
        difficultyText.SetText(TextManager.tm.GetUIText(TextManager.UIText.easy_description));
    }

    public void ShowNormalDescription()
    {
        difficultyText.enabled = true;
        difficultyText.SetText(TextManager.tm.GetUIText(TextManager.UIText.normal_description));
    }

    public void ShowHardDescription()
    {
        difficultyText.enabled = true;
        difficultyText.SetText(TextManager.tm.GetUIText(TextManager.UIText.hard_description));
    }

    public void HideDescription()
    {
        difficultyText.enabled = false;
    }
}
