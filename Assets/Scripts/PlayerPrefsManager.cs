using UnityEngine;

public class PlayerPrefsManager {

	const string SOUND_TOGGLE_KEY = "sound_toggle";
	const string MUSIC_TOGGLE_KEY = "music_toggle";

    const string HIGHSCORE_KEY = "highscore";

	// Sound toggle
    public static int SoundToggle { 
        get { return PlayerPrefs.HasKey(SOUND_TOGGLE_KEY) ? PlayerPrefs.GetInt(SOUND_TOGGLE_KEY) : 1; }
        set { PlayerPrefs.SetInt(SOUND_TOGGLE_KEY, value); }    
    }

    public static int MusicToggle { 
        get { return PlayerPrefs.HasKey(MUSIC_TOGGLE_KEY) ? PlayerPrefs.GetInt(MUSIC_TOGGLE_KEY) : 1; }
        set { PlayerPrefs.SetInt(MUSIC_TOGGLE_KEY, value); }    
    }

    public static int HighScore { 
        get { return PlayerPrefs.HasKey(HIGHSCORE_KEY) ? PlayerPrefs.GetInt(HIGHSCORE_KEY) : 0; }
        set { PlayerPrefs.SetInt(HIGHSCORE_KEY, value); }    
    }
}
