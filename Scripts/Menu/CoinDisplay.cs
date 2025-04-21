using UnityEngine;
using TMPro;
using System.IO;

public class CoinDisplay : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    private int coins;
    private string jsonPath;

    [System.Serializable]
    private class GameConfig
    {
        public string lang;
        public int coins;
    }

    void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
        if (textMesh == null)
        {
            Debug.LogError($"No TextMeshProUGUI found on {gameObject.name}");
            return;
        }

        jsonPath = Path.Combine(Application.dataPath, "Data/data.json");
    }

    void Start()
    {
        // Загрузка coins из data.json
        LoadCoins();
        UpdateText();
    }

    void OnApplicationQuit()
    {
        SaveCoins();
    }

    void OnDestroy()
    {
        SaveCoins();
    }

    private void LoadCoins()
    {
        if (File.Exists(jsonPath))
        {
            try
            {
                string json = File.ReadAllText(jsonPath);
                GameConfig config = JsonUtility.FromJson<GameConfig>(json);
                coins = config.coins;
                Debug.Log($"Loaded coins from data.json: {coins}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to parse data.json: {e.Message}");
                coins = 0;
            }
        }
        else
        {
            Debug.LogError($"data.json not found at {jsonPath}");
            coins = 0;
        }
    }

    private void SaveCoins()
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

            config.coins = coins;
            string updatedJson = JsonUtility.ToJson(config, true);
            File.WriteAllText(jsonPath, updatedJson);
            Debug.Log($"Saved coins to data.json: {coins}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save data.json: {e.Message}");
        }
    }

    private void UpdateText()
    {
        textMesh.text = $"{coins}";
        Debug.Log($"Coin display updated: Coins: {coins}");
    }

    // Для теста или вызова из других скриптов
    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateText();
        Debug.Log($"Coins updated: {coins}");
    }

    public void SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            UpdateText();
            Debug.Log($"Coins updated: {coins}");
        }
        else
        {
            Debug.Log($"Not enough coins! Need: {amount}, Have: {coins}");
        }
    }
}