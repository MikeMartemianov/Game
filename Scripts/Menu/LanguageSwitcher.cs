using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class LanguageSwitcher : MonoBehaviour
{
    public static string CurrentLanguage { get; private set; }
    public static System.Action OnLanguageChanged;
    private string jsonPath;
    private List<string> langs = new List<string> { "ru", "en", "fi" };
    private int currentIndex = 0;

    [System.Serializable]
    private class GameConfig
    {
        public string lang;
        public int coins;
    }

    void Awake()
    {
        jsonPath = Path.Combine(Application.dataPath, "Data/data.json");
        LoadLanguage();
    }

    private void LoadLanguage()
    {
        if (File.Exists(jsonPath))
        {
            try
            {
                string json = File.ReadAllText(jsonPath);
                GameConfig config = JsonUtility.FromJson<GameConfig>(json);
                CurrentLanguage = config.lang.ToLower();
                currentIndex = langs.IndexOf(CurrentLanguage);
                if (currentIndex == -1)
                {
                    currentIndex = 0;
                    CurrentLanguage = langs[currentIndex];
                }
                Debug.Log($"Loaded language from data.json: {CurrentLanguage}, index: {currentIndex}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to parse data.json: {e.Message}");
                CurrentLanguage = "en";
                currentIndex = langs.IndexOf(CurrentLanguage);
            }
        }
        else
        {
            Debug.LogError($"data.json not found at {jsonPath}");
            CurrentLanguage = "en";
            currentIndex = 0;
        }
    }

    private void SaveLanguage()
    {
        try
        {
            GameConfig config;
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                config = JsonUtility.FromJson<GameConfig>(json);
            }
            else
            {
                config = new GameConfig { lang = "en", coins = 0 };
            }

            config.lang = CurrentLanguage;
            string updatedJson = JsonUtility.ToJson(config, true);
            File.WriteAllText(jsonPath, updatedJson);
            Debug.Log($"Saved language to data.json: {CurrentLanguage}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data.json: {e.Message}");
        }
    }

    public void SetLanguage(string language)
    {
        language = language.ToLower();
        if (!langs.Contains(language))
        {
            Debug.LogWarning($"Unsupported language: {language}");
            return;
        }

        if (CurrentLanguage != language)
        {
            CurrentLanguage = language;
            currentIndex = langs.IndexOf(language);
            SaveLanguage();
            OnLanguageChanged?.Invoke();
            Debug.Log($"Language switched to: {language}, index: {currentIndex}");
        }
    }

    public void SwitchLanguage()
    {
        currentIndex = (currentIndex + 1) % langs.Count;
        string language = langs[currentIndex];

        if (CurrentLanguage != language)
        {
            CurrentLanguage = language;
            SaveLanguage();
            OnLanguageChanged?.Invoke();
            Debug.Log($"Language switched to: {language}, index: {currentIndex}");
        }
    }
}