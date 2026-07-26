using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // Enum for the different sound effects in the game
    public enum SoundTrack : byte { Button_Hover, Button_Click }
    // Enum for the different music tracks in the game
    public enum MusicTrack : byte { Main_Menu, Level_One }
    public static SoundManager sm; // Static reference of the sound manager
    public AudioClip[] musicList; // List of music clips of the game to play in the different scenes
    public AudioClip[] soundList; // List of sound clips of the game to play in the different situations
    private AudioSource soundsAudioSource; // Reference to the audio source that play sound effects in the scene
    private AudioSource soundsLoopAudioSource; // Reference to the audio source that play loop sound effects in the scene
    private AudioSource musicAudioSource; // Reference to the audio source that play music in the scene
    private AudioSource musicLoopAudioSource; // Reference to the audio source that play loop music in the scene
    private static bool soundOn = true; // Flag for sound on/off
    private static bool musicOn = true; // Flag for music on/off
    private static bool inMenus = false; // Flag for not starting over the music when navigating menus
    private Button soundOnOffBut; // Reference to the sound setting on/off button
    private Button musicOnOffBut; // Reference to the music setting on/off button
    public Sprite audioEnabled; // Reference to the audio enable sprite for the settings buttons
    public Sprite audioDisabled; // Reference to the audio disable sprite for the settings buttons

    void Awake()
    {
        if (sm != null && sm != this)
        {
            Destroy(gameObject); // Only one instance of sound manager allowed
        }
        else
        {
            sm = this; // Initialise the sound manager instance
        }
        // Get the audio sources for sounds and music.
        soundsAudioSource = GameObject.FindWithTag("SoundPlayer").GetComponent<AudioSource>();
        musicAudioSource = GameObject.FindWithTag("MusicPlayer").GetComponent<AudioSource>();
        soundsLoopAudioSource = GameObject.FindWithTag("SoundPlayerLoop").GetComponent<AudioSource>();
        musicLoopAudioSource = GameObject.FindWithTag("MusicPlayerLoop").GetComponent<AudioSource>();
        // Set this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        // Subscribe to the scene loaded event to configure music when different scenes are loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the scene loaded is the main menu
        if (scene.name == "MainMenuScene")
        {
            // If we are not in menus
            if (!inMenus)
            {
                // Set the in menus flag to true because we enter the menus coming from the game or just started the game
                inMenus = true;
                // Play the main menu music loop
                //PlayMusicLoop(MusicTrack.Main_Menu);
            }
        }
        // If the scene loaded is the settings
        if (scene.name == "SettingsScene")
        {
            // Set the sound and music toggles
            SetToggles();
        }
        // If the scene loaded is the level
        if (scene.name == "LevelOneScene")
        {
            // Play the level music
            //PlayMusicLoop(MusicTrack.Level_One);
            // Set the sound and music toggles from the pause panel
            //SetToggles();
        }
        // If the scene loaded is the story scene
        if (scene.name == "StoryScene")
        {
            // Set the in menus flag to false because we are leaving the menus, and this is the only scene that leaves the menus
            inMenus = false;
        }
    }

    private void PlayMusicLoop(MusicTrack music)
    {
        // Stop the current music clip
        musicLoopAudioSource.Stop();
        // Select the music clip
        musicLoopAudioSource.clip = musicList[(int)music];
        // Activate clip loop
        musicLoopAudioSource.loop = true;
        // Play the music clip
        musicLoopAudioSource.Play();
    }

    public void PlayMusicOneShot(MusicTrack music)
    {
        // Stop the current clip
        musicAudioSource.Stop();
        // Play a music clip selected with index from a list of clips stored in the manager
        musicAudioSource.PlayOneShot(musicList[(int)music]);
    }

    public void PlaySoundOneShot(SoundTrack sound)
    {
        // Play sound clip selected using the corresponding index
        soundsAudioSource.PlayOneShot(soundList[(int)sound]);
    }

    public void PlaySoundLoop(SoundTrack sound)
    {
        // Stop the current sound clip
        soundsLoopAudioSource.Stop();
        // Activate clip loop
        soundsLoopAudioSource.loop = true;
        // Select the sound clip using the corresponding index
        soundsLoopAudioSource.clip = soundList[(int)sound];
        // Play sound clip selected 
        soundsLoopAudioSource.Play();
    }

    public void StopSoundLoop()
    {
        // Stop the current sound clip loop
        soundsLoopAudioSource.Stop();
    }

    public void SetToggles()
    {
        // Get the reference to the toggles
        soundOnOffBut = GameObject.FindGameObjectWithTag("SoundToggle").GetComponent<Button>();
        musicOnOffBut = GameObject.FindGameObjectWithTag("MusicToggle").GetComponent<Button>();
        // If sound is on or off, set the correct image to the sound setting button
        if (soundOn)
        {
            soundOnOffBut.image.sprite = audioEnabled;
        }
        else
        {
            soundOnOffBut.image.sprite = audioDisabled;
        }
        // If music is on or off, set the correct image to the music setting button
        if (musicOn)
        {
            musicOnOffBut.image.sprite = audioEnabled;
        }
        else
        {
            musicOnOffBut.image.sprite = audioDisabled;
        }
    }

    public void SoundOnOff()
    {
        if(soundOn)
        {
            // Turn off sound flag
            soundOn = false;
            // Mute the sound audio sources
            soundsAudioSource.mute = true;
            soundsLoopAudioSource.mute = true;
            // Set the disabled image to the sound setting button
            soundOnOffBut.image.sprite = audioDisabled;
        }
        else
        {
            // Turn on sound flag
            soundOn = true;
            // Unmute the sound audio sources
            soundsAudioSource.mute = false;
            soundsLoopAudioSource.mute = false;
            // Set the enabled image to the sound setting button
            soundOnOffBut.image.sprite = audioEnabled;
        }
        
        
    }

    public void MusicOnOff()
    {
        if(musicOn)
        {
            // Turn music flag off
            musicOn = false;
            // Mute the music audio sources
            musicAudioSource.mute = true;
            musicLoopAudioSource.mute = true;
            // Set the disabled image to the music setting button
            musicOnOffBut.image.sprite = audioDisabled;
        }
        else
        {
            // Turn music flag on
            musicOn = true;
            // Unmute the music audio sources
            musicAudioSource.mute = false;
            musicLoopAudioSource.mute = false;
            // Set the enabled image to the music setting button
            musicOnOffBut.image.sprite = audioEnabled;
        }
    }

    public bool SoundState()
    {
        // Return the state of the sound
        return soundOn;
    }

    public bool MusicState()
    {
        // Return the state of the music
        return musicOn;
    }
}
