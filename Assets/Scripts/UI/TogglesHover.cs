using UnityEngine;
using UnityEngine.UI;

public class TogglesHover : MonoBehaviour
{
    //public Sprite audioToggleHover; // Reference to the audio toggle hover sprite
    public Sprite languageToggleHover; // Reference to the language toggle hover sprite

    public void HoverOnAudioToggle()
    {
        // Change the audio toggle sprite when hover is on
        //gameObject.GetComponent<Button>().image.sprite = audioToggleHover;
    }

    public void HoverOnLanguageToggle()
    {
        // Change the language toggle sprite when hover is on
        gameObject.GetComponent<Button>().image.sprite = languageToggleHover;
    }

    public void HoverOffAudioToggle()
    {
        // Set the audio toggle sprite to the default one when hover is off
        //SoundManager.sm.SetToggles();
    }

    public void HoverOffLanguageToggle()
    {
        // Set the language toggle sprite to the default one when hover is off
        TextManager.tm.SetLanguageToggles();
    }
}
