using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    // Enum to represent the different languages available in the game
    public enum Language : byte { English, Spanish }
    // Enum to represent the different types of texts available in the game
    public enum StoryText : byte { story_1, story_2, story_3 }
    // Enum to represent the different button texts available in the game
    public enum ButtonText : byte
    { 
        play,
        settings,
        lang_es,
        lang_en,
        help,
        credits,
        quit,
        back,
        skip,
        dif_easy,
        dif_normal,
        dif_hard
    }
    // Enum to represent the different ui texts available in the game
    public enum UIText : byte
    {
        game_name_title,
        help_scene_title,
        help_text,
        settings_scene_title,
        sound_toggle_text,
        music_toggle_text,
        language_title,
        difficulty_scene_title
    }
    // Enum to represent the different titles that appear in the credits
    public enum CreditsText : byte
    { 
        team_name,
        narrative_role,
        gdesign_role,
        art_role,
        audio_role,
        code_role,
        sthanks_title,
        sthanks_msj,
        vsthanks_title,
        vsthanks_msj
    }
    public static TextManager tm; // Static reference to the text manager
    private static Language language = Language.Spanish; // Language of the game
    private string[] storyTextEN; // List of texts contained in the game story in english
    private string[] buttonsTextEN; // List of texts contained in the game buttons in english
    private string[] uiTextEN; // List of texts contained in the game ui in english
    private string[] creditsTextEN; // List of text contained in the game credits in english
    private string[] storyTextES; // List of texts contained in the game story in spanish
    private string[] buttonsTextES; // List of texts contained in the game buttons in spanish
    private string[] uiTextES; // List of texts contained in the game ui in spanish
    private string[] creditsTextES; // List of text contained in the game credits in spanish
    private Button spanishToggle; // Reference to the spanish language toggle
    private Button englishToggle; // Reference to the english language toggle
    public Sprite toggleEnabled; // Enabled toggle sprite
    public Sprite toggleDisabled; // Disabled toggle sprite

    void Awake()
    {
        if (tm != null && tm != this)
        {
            Destroy(gameObject); // Only one instance of text manager allowed
        }
        else
        {
            tm = this; // Inicialise the text manager instance
        }
        // Initialize all the text lists in english
        storyTextEN = new string[] {
            "Story1...",
            "Story2...",
            "Story3..."
        };
        buttonsTextEN = new string[] {
            "PLAY",
            "SETTINGS",
            "SPANISH",
            "ENGLISH",
            "HELP",
            "CREDITS",
            "QUIT",
            "BACK",
            "SKIP",
            "EASY",
            "NORMAL",
            "HARD"
        };
        uiTextEN = new string[] {
            "GAME NAME",
            "HELP",
            "HOW TO PLAY\n...\n\nCONTROLS\n...",
            "SETTINGS",
            "SOUND",
            "MUSIC",
            "LANGUAGE",
            "DIFFICULTY"
        };
        creditsTextEN = new string[] {
            "TEAM NAME",
            "SCRIPT / NARRATIVE",
            "GRAPHIC DESIGN",
            "ART 2D",
            "AUDIO",
            "PROGRAMMING",
            "SPECIAL THANKS",
            "For organizing the event",
            "VERY SPECIAL THANKS",
            "TO YOU!\nfor playing the game!"
        };
        // Initialize all the text lists in spanish
        storyTextES = new string[] {
            "Historia1...",
            "Historia2...",
            "Historia3..."
        };
        buttonsTextES = new string[] {
            "JUGAR",
            "OPCIONES",
            "ESPAÑOL",
            "INGLÉS",
            "AYUDA",
            "CRÉDITOS",
            "SALIR",
            "ATRÁS",
            "SALTAR",
            "FÁCIL",
            "NORMAL",
            "DIFÍCIL"
        };
        uiTextES = new string[] {
            "NOMBRE DEL JUEGO",
            "AYUDA",
            "CÓMO JUGAR\n...\n\nCONTROLES\n...",
            "OPCIONES",
            "SONIDO",
            "MÚSICA",
            "IDIOMA",
            "DIFICULTAD"
        };
        creditsTextES = new string[] {
            "NOMBRE DEL EQUIPO",
            "GUIÓN / NARRATIVA",
            "DISEÑO GRÁFICO",
            "ARTE 2D",
            "AUDIO",
            "PROGRAMACIÓN",
            "AGRADECIMIENTOS ESPECIALES",
            "Por organizar el evento",
            "AGRADECIMIENTOS MUY ESPECIALES",
            "A VOS!\npor jugar el juego!"
        };
        // Set this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        // Subscribe to the scene loaded event to configure language when different scenes are loaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If the scene loaded is the settings
        if (scene.name == "SettingsScene")
        {
            // Set the language toggles
            SetLanguageToggles();
        }
    }

    public void SetLanguageToggles()
    {
        // Get the reference to the toggles
        spanishToggle = GameObject.FindGameObjectWithTag("ToggleES").GetComponent<Button>();
        englishToggle = GameObject.FindGameObjectWithTag("ToggleEN").GetComponent<Button>();
        // Depending on the selected language, activate and deactivate the corresponding toggles by changing the sprite
        if (language == Language.Spanish)
        {
            spanishToggle.image.sprite = toggleEnabled;
            englishToggle.image.sprite = toggleDisabled;
        }
        if (language == Language.English)
        {
            englishToggle.image.sprite = toggleEnabled;
            spanishToggle.image.sprite = toggleDisabled;
        }
    }

    public void SetLanguageEnglish()
    {
        // Set the language to english and update the toggles
        language = Language.English;
        englishToggle.image.sprite = toggleEnabled;
        spanishToggle.image.sprite = toggleDisabled;
    }

    public void SetLanguageSpanish()
    {
        // Set the language to spanish and update the toggles
        language = Language.Spanish;
        spanishToggle.image.sprite = toggleEnabled;
        englishToggle.image.sprite = toggleDisabled;
    }

    public Language GetLanguage()
    {
        // Get the current game language
        return language;
    }

    public string GetStoryText(StoryText textId)
    {
        // Get the corresponding text from the list of texts in the story
        if (language == Language.Spanish)
        {
            return storyTextES[(int)textId];
        }
        if (language == Language.English)
        {
            return storyTextEN[(int)textId];
        }
        return "-story-text-";
    }

    public string GetButtonText(ButtonText textId)
    {
        // Get the corresponding text from the list of texts in the buttons
        if (language == Language.Spanish)
        {
            return buttonsTextES[(int)textId];
        }
        if (language == Language.English)
        {
            return buttonsTextEN[(int)textId];
        }
        return "-button-text-";
    }

    public string GetUIText(UIText textId)
    {
        // Get the corresponding text from the list of texts in the ui
        if (language == Language.Spanish)
        {
            return uiTextES[(int)textId];
        }
        if (language == Language.English)
        {
            return uiTextEN[(int)textId];
        }
        return "-ui-text-";
    }

    public string GetCreditsText(CreditsText textId)
    {
        // Get the corresponding text from the list of texts in the credits
        if (language == Language.Spanish)
        {
            return creditsTextES[(int)textId];
        }
        if (language == Language.English)
        {
            return creditsTextEN[(int)textId];
        }
        return "-credits-text-";
    }
}