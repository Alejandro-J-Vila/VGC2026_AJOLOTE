using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public void PlayClickSound()
    {
        // Play sound when clicking on the button
        SoundManager.sm.PlaySoundOneShot(SoundManager.SoundTrack.Button_Click);
    }

    public void PlayHoverSound()
    {
        // Play sound when hovering on the button
        SoundManager.sm.PlaySoundOneShot(SoundManager.SoundTrack.Button_Hover);
    }
}
