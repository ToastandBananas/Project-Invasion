using UnityEngine;

public class PlayerPrefsController : MonoBehaviour
{
    const string MASTER_VOLUME_KEY = "Master Volume";
    const float  MIN_MASTER_VOLUME = -60f;
    const float  MAX_MASTER_VOLUME = 0f;

    const string MUSIC_VOLUME_KEY = "Music Volume";
    const float MIN_MUSIC_VOLUME = -60f;
    const float MAX_MUSIC_VOLUME = 0f;

    const string EFFECTS_VOLUME_KEY = "Effects Volume";
    const float MIN_EFFECTS_VOLUME = -60f;
    const float MAX_EFFECTS_VOLUME = 0f;

    const string VOICE_VOLUME_KEY = "Effects Volume";
    const float MIN_VOICE_VOLUME = -40f;
    const float MAX_VOICE_VOLUME = 0f;

    const string DIFFICULTY_KEY = "Difficulty";
    const float  MIN_DIFFICULTY = 0f;
    const float  MAX_DIFFICULTY = 2f;

    const string DAMAGE_POPUPS_KEY = "Damage Popups";

    public static void SetMasterVolume(float volume)
    {
        if (volume >= MIN_MASTER_VOLUME && volume <= MAX_MASTER_VOLUME)
        {
            // Debug.Log("Master volume set to " + volume);
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }
        else
            Debug.LogError("Master volume is out of range");
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 0f);
    }

    public static void SetMusicVolume(float volume)
    {
        if (volume >= MIN_MUSIC_VOLUME && volume <= MAX_MUSIC_VOLUME)
        {
            // Debug.Log("Music volume set to " + volume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
        }
        else
            Debug.LogError("Music volume is out of range");
    }

    public static float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0f);
    }

    public static void SetEffectsVolume(float volume)
    {
        if (volume >= MIN_EFFECTS_VOLUME && volume <= MAX_EFFECTS_VOLUME)
        {
            // Debug.Log("Effects volume set to " + volume);
            PlayerPrefs.SetFloat(EFFECTS_VOLUME_KEY, volume);
        }
        else
            Debug.LogError("Effects volume is out of range");
    }

    public static float GetEffectsVolume()
    {
        return PlayerPrefs.GetFloat(EFFECTS_VOLUME_KEY, 0f);
    }

    public static void SetVoiceVolume(float volume)
    {
        if (volume >= MIN_VOICE_VOLUME && volume <= MAX_VOICE_VOLUME)
        {
            // Debug.Log("Voice volume set to " + volume);
            PlayerPrefs.SetFloat(VOICE_VOLUME_KEY, volume);
        }
        else
            Debug.LogError("Voice volume is out of range");
    }

    public static float GetVoiceVolume()
    {
        return PlayerPrefs.GetFloat(VOICE_VOLUME_KEY, 0f);
    }

    public static void SetDifficulty(float difficulty)
    {
        if (difficulty >= MIN_DIFFICULTY && difficulty <= MAX_DIFFICULTY)
        {
            // Debug.Log("Master difficulty set to " + difficulty);
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);

            if (CastleHealth.instance != null)
                CastleHealth.instance.OnDifficultyChanged();
        }
        else
            Debug.LogError("Difficulty setting is out of range.");
    }

    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY, 0f);
    }

    public static float GetDifficultyMultiplier_CastleHealth()
    {
        // Castle health will be multiplied by the number returned
        if (PlayerPrefs.GetFloat(DIFFICULTY_KEY) == 0f) // Easy difficulty
            return 1f;
        else if (PlayerPrefs.GetFloat(DIFFICULTY_KEY) == 1f) // Medium difficulty
            return 0.75f;
        else
            return 0.5f; // Hard difficulty
    }

    public static float GetDifficultyMultiplier_EnemyAttackDamage()
    {
        // Enemy attack damage will be multiplied by the number returned
        if (PlayerPrefs.GetFloat(DIFFICULTY_KEY) == 0f) // Easy difficulty
            return 0.75f;
        else if (PlayerPrefs.GetFloat(DIFFICULTY_KEY) == 1f) // Medium difficulty
            return 1f;
        else
            return 1.25f; // Hard difficulty
    }

    public static void SetDamagePopups(bool popupsEnabled)
    {
        if (popupsEnabled)
            PlayerPrefs.SetInt(DAMAGE_POPUPS_KEY, 1);
        else
            PlayerPrefs.SetInt(DAMAGE_POPUPS_KEY, 0);
    }

    public static bool DamagePopupsEnabled()
    {
        if (PlayerPrefs.GetInt(DAMAGE_POPUPS_KEY) == 1)
            return true;
        else
            return false;
    }
}
