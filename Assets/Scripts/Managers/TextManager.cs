using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    // Enum to represent the different languages available in the game
    public enum Language : byte { English, Spanish }
    // Enum to represent the different types of texts available in the game
    public enum StoryText : byte { story_intro, story_1, story_2, story_3, story_4, story_5, story_6, story_7, story_8, story_9, story_10, story_11, story_12 }
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
        dif_hard,
        game_menu,
        game_help,
        reset_choices,
        confirm_respawn,
        cross_obstacle,
        go_back,
        close_panel,
        retry,
        quit_to_main_menu
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
        difficulty_scene_title,
        ingame_menu_title,
        ingame_help_title,
        reset_panel_title,
        reset_panel_msj,
        obstacle_panel_title,
        obstacle_msj,
        success_msj,
        lore_panel_title,
        limb_animation_regen,
        limb_animation_lost,
        end_game_title,
        score_msj,
        end_game_msj,
        tutorial_msj,
        lore_collected_msj
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
    private string[] creditsTextEN; // List of texts contained in the game credits in english
    private string[] itemDescriptionsEN; // List of texts for the item descriptions in english
    private string[] obstacleDescriptionsEN; // List of texts for the obstacles descriptions in english
    private string[] skillDescriptionsEN; // List of texts for the skill descriptions in english
    private string[] storyTextES; // List of texts contained in the game story in spanish
    private string[] buttonsTextES; // List of texts contained in the game buttons in spanish
    private string[] uiTextES; // List of texts contained in the game ui in spanish
    private string[] creditsTextES; // List of text contained in the game credits in spanish
    private string[] itemDescriptionsES; // List of texts for the item descriptions in spanish
    private string[] obstacleDescriptionsES; // List of texts for the obstacles descriptions in spanish
    private string[] skillDescriptionsES; // List of texts for the skill descriptions in spanish
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
            "You're the last male Axolotl in the world, and pollution has turned the lake you once called home into a dangerous place.\n\nTo ensure your survival and that of your species, you must escape, reunite with your mate, and find a new home together, far away from the pollution.",
            "Axolotls always sleep with their eyes open because they don't have eyelids.",
            "Axolotls can live for up to 15 years and retain their larval traits throughout their entire lives.",
            "Axolotls have some ability to change color to camouflage themselves.",
            "Axolotls have regenerative abilities. They can completely regrow lost limbs, vital organs, and even parts of their brains.",
            "Axolotls have the largest genome ever sequenced, it is ten times larger than the human genome.",
            "Axolotls can survive out of water, but only for about an hour.",
            "Axolotls have a varied diet that includes small fish and crustaceans.",
            "Male Axolotls court females with a dance called 'hula' shaking their tails, and the female responds by nudging the male with her snout.",
            "Female Axolotls can lay up to 1,000 eggs.",
            "Axolotls are native to the Valley of Mexico.",
            "Axolotls live in lakes or shallow-water canals with abundant aquatic vegetation.",
            "Axolotls are critically endangered species, with pollution of their habitat being the main cause."
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
            "START",
            "EASY",
            "NORMAL",
            "HARD",
            "MENU (Z)",
            "HELP (E)",
            "RESET (E)",
            "CONFIRM (Q)",
            "CROSS (Q)",
            "GO BACK (E)",
            "CLOSE (Q)",
            "TRY AGAIN (E)",
            "QUIT"
        };
        uiTextEN = new string[] {
            "GAME NAME",
            "HELP",
            "Try to reach the end of the laberinth.\n\nDon't be afraid to lose some of your limbs when crossing obstacles, you can regenerate them.\n\nWhen you regenerate, you can choose adaptations that let you cross obstacles safely.\n\nPay attention to the obstacles you cross to choose your adaptations wisely.\n\nWhen you don't have adaptations, you have a little chance of crossing obstacles without harm.\n\nYou will find useful items scattered across the laberinth that will help you.\n\nW A S D -- Move\nLMB (or shortcuts) -- Click buttons",
            "SETTINGS",
            "SOUND",
            "MUSIC",
            "LANGUAGE",
            "DIFFICULTY",
            "MAIN MENU",
            "HOW TO PLAY",
            "You lost too many limbs and had to retreat.\nUse your superpower to regenerate and adapt.",
            "Select your new limb adaptations.",
            "OBSTACLE ENCOUNTERED",
            "You are not adapted to cross it safely.",
            "You crossed safely!",
            "Axolotl's curious facts",
            "You regenerated a limb!!",
            "You lost a limb!!",
            "CONGRATULATIONS!!",
            "NUMBER OF RESPAWNS",
            "You managed to escape and reunite with your mate\nNow you can begin a new life away from pollution",
            "W A S D -- Movement",
            "CONGRATULATIONS! You found all the lore scrolls!"
        };
        creditsTextEN = new string[] {
            "TEAM AXOLOTL",
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
        itemDescriptionsEN = new string[] {
            "This special item gives you a better sense of orientation",
            "This special item allows you to instantly regenerate a lost limb",
            "This special item enhances your vision allowing to see further",
            "This special item provides temporary protection from harm",
            "This ancient scroll contains valuable knowledge"
        };
        obstacleDescriptionsEN = new string[] {
            "A pool of RED toxic chemicals",
            "A pool of GREEN toxic chemicals",
            "A pool of BLUE toxic chemicals",
            "A pool of YELLOW toxic chemicals",
            "A pool of PINK toxic chemicals",
            "A pool of ORANGE toxic chemicals"
        };
        skillDescriptionsEN = new string[] {
            "RED adaptation",
            "GREEN adaptation",
            "BLUE adaptation",
            "YELLOW adaptation",
            "PINK adaptation",
            "ORANGE adaptation"
        };
        // Initialize all the text lists in spanish
        storyTextES = new string[] {
            "Eres el último Ajolote macho del mundo y la contaminación ha convertido la laguna que una vez llamaste hogar, en un lugar peligroso para seguir viviendo.\n\nPara asegurar tu supervivencia y la de tu especie, debes escapar para reencontrarte con tu pareja y buscar un nuevo hogar juntos, lejos de la contaminación.",
            "Los Ajolotes siempre duermen con los ojos abiertos porque no tienen párpados.",
            "Los Ajolotes viven hasta 15 años y conservan sus rasgos larvales toda su vida.",
            "Los Ajolotes tienen cierta capacidad de cambiar de color para camuflarse.",
            "Los Ajolotes poseen la capacidad de regeneración. Puede reconstruir por completo extremidades perdidas, órganos vitales e incluso segmentos del cerebro.",
            "Los Ajolotes tienen el genoma más grande que alguna vez se haya secuenciado, su tamaño es diez veces mayor que el de los humanos.",
            "Los Ajolotes pueden sobrevivir fuera del agua, pero solo durante una hora aproximadamente.",
            "Los Ajolotes tienen una dieta variada que incluye pequeños peces y crustáceos.",
            "Los Ajolotes macho, cortejan a las hembras mediante una danza llamada “hula” sacudiendo la cola y la hembra responde dando un golpe con el hocico.",
            "Las hembras Ajolote pueden poner hasta 1000 huevos.",
            "Los Ajolotes son nativos del Valle de México.",
            "Los Ajolotes viven en lagos o canales de agua poco profunda y mucha vegetación acuática.",
            "Los Ajolotes se encuentran en peligro crítico de extinción y la principal causa es la contaminación de su hábitat."
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
            "EMPEZAR",
            "FÁCIL",
            "NORMAL",
            "DIFÍCIL",
            "MENÚ (Z)",
            "AYUDA (E)",
            "RESTABLECER (E)",
            "CONFIRMAR (Q)",
            "CRUZAR (Q)",
            "VOLVER (E)",
            "CERRAR (Q)",
            "VOLVER A INTENTAR (E)",
            "SALIR"
        };
        uiTextES = new string[] {
            "NOMBRE DEL JUEGO",
            "AYUDA",
            "Tratá de llegar al final del laberinto.\n\nNo tengas miedo de perder extremidades al cruzar obstáculos, las podés regenerar.\n\nCuando te regenerás, podés elegir adaptaciones que te permitan cruzar los obstáculos de forma segura.\n\nPrestá atención a los obstáculos que encontrás, así podes elegir mejor las adaptaciones.\n\nCuando no tenés adaptaciones, hay una pequeña chance de cruzar un obstáculo sin recibir daño.\n\nVas a encontrar ítems útiles distribuidos en el laberinto que te van a ayudar.\n\nW A S D -- Movimiento\nBIM (o atajos) -- Clickear botones",
            "OPCIONES",
            "SONIDO",
            "MÚSICA",
            "IDIOMA",
            "DIFICULTAD",
            "MENÚ PRINCIPAL",
            "CÓMO JUGAR",
            "Perdiste demasiadas extremidades y tuviste que retroceder.\nUsa tu superpoder para regenerarte y adaptarte.",
            "Selecciona las adaptaciones para tus nuevas extremidades.",
            "OBSTÁCULO ENCONTRADO",
            "No estás adaptado para cruzar de forma segura.",
            "Cruzaste sin problemas!",
            "Datos curiosos de los Ajolotes",
            "Regeneraste una extremidad!!",
            "Perdiste una extremidad!!",
            "FELICITACIONES!!",
            "CANTIDAD DE REAPARICIONES",
            "Lograste escapar y reencontrarte con tu pareja\nAhora pueden comenzar una nueva vida lejos de la contaminación",
            "W A S D -- Movimiento",
            "FELICITACIONES! Encontraste todos los pergaminos de lore!"
        };
        creditsTextES = new string[] {
            "EQUIPO AJOLOTE",
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
        itemDescriptionsES = new string[] {
            "Este objeto especial te da un mejor sentido de orientación",
            "Este objeto especial te permite regenerar instantaneamente una extremidad perdida",
            "Este objeto especial mejora tu visión y te permite ver mas lejos",
            "Este objeto especial te otorga protección temporal contra cualquier daño",
            "Este pergamino antiguo contiene conocimiento valioso"
        };
        obstacleDescriptionsES = new string[] {
            "Un charco de químicos tóxicos de color ROJO",
            "Un charco de químicos tóxicos de color VERDE",
            "Un charco de químicos tóxicos de color AZUL",
            "Un charco de químicos tóxicos de color AMARILLO",
            "Un charco de químicos tóxicos de color ROSA",
            "Un charco de químicos tóxicos de color NARANJA"
        };
        skillDescriptionsES = new string[] {
            "Adaptación ROJA",
            "Adaptación VERDE",
            "Adaptación AZUL",
            "Adaptación AMARILLA",
            "Adaptación ROSA",
            "Adaptación NARANJA"
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

    public string GetItemText(int textId)
    {
        // Get the corresponding text from item descriptions
        if (language == Language.Spanish)
        {
            return itemDescriptionsES[textId];
        }
        if (language == Language.English)
        {
            return itemDescriptionsEN[textId];
        }
        return "-item-text-";
    }

    public string GetObstacleText(int textId)
    {
        // Get the corresponding text from the obstacle descriptions
        if (language == Language.Spanish)
        {
            return obstacleDescriptionsES[textId];
        }
        if (language == Language.English)
        {
            return obstacleDescriptionsEN[textId];
        }
        return "-obstacle-text-";
    }

    public string GetSkillText(int textId)
    {
        // Get the corresponding text from the skill descriptions
        if (language == Language.Spanish)
        {
            return skillDescriptionsES[textId];
        }
        if (language == Language.English)
        {
            return skillDescriptionsEN[textId];
        }
        return "-skill-text-";
    }
}