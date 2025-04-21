using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;

public class LangControlller : MonoBehaviour
{
    [SerializeField] private string key; // Для TextMeshProUGUI
    private TextMeshProUGUI textMesh;
    private TMP_Dropdown dropdown;
    private static Dictionary<string, Dictionary<string, string>> translations;
    private static string currentLanguage = "en";

    [System.Serializable]
    private class GameConfig
    {
        public string lang;
        public int coins;
    }

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        dropdown = GetComponent<TMP_Dropdown>();

        if (textMesh == null && dropdown == null)
        {
            Debug.LogError($"No TextMeshProUGUI or TMP_Dropdown found on {gameObject.name}");
            return;
        }

        // Загрузка языка из data.json
        string jsonPath = Path.Combine(Application.dataPath, "Data/data.json");
        if (File.Exists(jsonPath))
        {
            try
            {
                string json = File.ReadAllText(jsonPath);
                GameConfig config = JsonUtility.FromJson<GameConfig>(json);
                if (!string.IsNullOrEmpty(config.lang))
                    currentLanguage = config.lang.ToLower();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to parse data.json: {e.Message}");
            }
        }

        // Загрузка переводов
        if (translations == null)
        {
            translations = new Dictionary<string, Dictionary<string, string>>();
            string dataFolder = Path.Combine(Application.dataPath, "Data");
            foreach (string file in Directory.GetFiles(dataFolder, "*.lang"))
            {
                string langCode = Path.GetFileNameWithoutExtension(file).ToLower();
                var langTranslations = new Dictionary<string, string>();
                foreach (string line in File.ReadAllLines(file))
                {
                    if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
                    int sep = line.IndexOf('=');
                    if (sep > 0)
                    {
                        string k = line.Substring(0, sep).Trim();
                        string v = line.Substring(sep + 1).Trim();
                        langTranslations[k] = v;
                    }
                }
                translations[langCode] = langTranslations;
            }
        }
    }

    void Start()
    {
        UpdateText();
        LanguageSwitcher.OnLanguageChanged += UpdateText;
    }

    void OnDestroy()
    {
        LanguageSwitcher.OnLanguageChanged -= UpdateText;
    }

    void UpdateText()
    {
        currentLanguage = LanguageSwitcher.CurrentLanguage ?? currentLanguage;

        if (textMesh != null)
        {
            // Локализация для TextMeshProUGUI
            if (translations != null && translations.TryGetValue(currentLanguage, out var langTranslations))
            {
                if (langTranslations.TryGetValue(key, out string translation))
                    textMesh.text = translation;
                else
                    textMesh.text = $"[{key}]";
            }
            else
            {
                textMesh.text = $"[{key}]";
            }
            Debug.Log($"LangControlller updated TextMeshProUGUI: key={key}, text={textMesh.text}");
        }
        else if (dropdown != null)
        {
            // Локализация для TMP_Dropdown (все элементы)
            if (translations != null && translations.TryGetValue(currentLanguage, out var langTranslations))
            {
                for (int i = 0; i < dropdown.options.Count; i++)
                {
                    string optionKey = $"dropdown_{i}";
                    if (langTranslations.TryGetValue(optionKey, out string translation))
                        dropdown.options[i].text = translation;
                    else
                        dropdown.options[i].text = $"[{optionKey}]";
                }
            }
            else
            {
                for (int i = 0; i < dropdown.options.Count; i++)
                {
                    dropdown.options[i].text = $"[dropdown_{i}]";
                }
            }
            dropdown.RefreshShownValue();
            Debug.Log($"LangControlller updated Dropdown: {dropdown.options.Count} options translated");
        }
    }

    private void OnDropdownValueChanged(int index)
    {
        // Опционально: Менять язык игры при выборе элемента
        // Уберите или настройте, если не нужно
        string[] langs = { "ru", "en", "fi" };
        if (index >= 0 && index < langs.Length)
        {
            FindObjectOfType<LanguageSwitcher>()?.SetLanguage(langs[index]);
        }
    }

    public static string GetTranslation(string key)
    {
        currentLanguage = LanguageSwitcher.CurrentLanguage ?? currentLanguage;
        if (translations != null && translations.TryGetValue(currentLanguage, out var langTranslations))
        {
            if (langTranslations.TryGetValue(key, out string translation))
                return translation;
            return $"[{key}]";
        }
        return $"[{key}]";
    }
}