using UnityEngine;

public class AudioToggle : MonoBehaviour
{
    public void SoundOnOff()
    {
        // Turn the sound on or off
        SoundManager.sm.SoundOnOff();
    }

    public void MusicOnOff()
    {
        // Turn the music on
        SoundManager.sm.MusicOnOff();
    }
}
