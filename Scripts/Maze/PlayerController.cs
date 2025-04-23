using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2f;           // Скорость движения игрока
    public Button upButton, downButton, leftButton, rightButton; // Кнопки управления
    public GameObject gameOverPanel;       // Панель окончания игры
    public Text resultText, coinsText;     // Тексты результата и монет
    public Button menuButton;              // Кнопка возврата в меню

    private Rigidbody2D rb;                // Компонент Rigidbody2D игрока
    private Vector2 currentDirection;      // Текущее направление движения
    private string controlMode;            // Режим управления: "buttons", "ac", "swipe"
    private Vector2 swipeStart;            // Начальная точка смахивания
    private bool isSwiping;                // Флаг смахивания
    private float swipeMinDistance = 50f;  // Минимальная дистанция для смахивания
    private int coins = 0;                 // Количество монет
    private int bites = 0;                 // Количество укусов

    void Start()
    {
        // Инициализация компонентов
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D не найден на объекте игрока!");
            return;
        }
        rb.gravityScale = 0; // Отключаем гравитацию
        rb.isKinematic = true; // Устанавливаем кинематический тип

        // Загрузка данных
        LoadControlSettings();

        // Настройка кнопок
        SetupButtonControls();

        // Скрываем панель окончания игры
        gameOverPanel.SetActive(false);
        menuButton.onClick.AddListener(() => SceneManager.LoadScene(0));

        Debug.Log($"Инициализация завершена. Режим управления: {controlMode}");
    }

    void LoadControlSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "settings.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            GameSettings settings = JsonUtility.FromJson<GameSettings>(json);
            controlMode = settings.controlMode;
            coins = settings.coins;
            Debug.Log($"Загружены настройки: controlMode = {controlMode}, coins = {coins}");
        }
        else
        {
            controlMode = "buttons"; // По умолчанию кнопки
            coins = 0;
            SaveControlSettings();
            Debug.Log("Файл настроек не найден, установлены значения по умолчанию.");
        }
    }

    void SaveControlSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "settings.json");
        GameSettings settings = new GameSettings { controlMode = controlMode, coins = coins };
        string json = JsonUtility.ToJson(settings);
        File.WriteAllText(filePath, json);
        Debug.Log($"Настройки сохранены: {filePath}");
    }

    void SetupButtonControls()
    {
        if (controlMode == "buttons")
        {
            if (upButton != null && downButton != null && leftButton != null && rightButton != null)
            {
                upButton.gameObject.SetActive(true);
                downButton.gameObject.SetActive(true);
                leftButton.gameObject.SetActive(true);
                rightButton.gameObject.SetActive(true);

                // Привязка событий к кнопкам
                upButton.onClick.AddListener(() => currentDirection = Vector2.up);
                downButton.onClick.AddListener(() => currentDirection = Vector2.down);
                leftButton.onClick.AddListener(() => currentDirection = Vector2.left);
                rightButton.onClick.AddListener(() => currentDirection = Vector2.right);

                Debug.Log("Кнопки управления активированы.");
            }
            else
            {
                Debug.LogError("Одна или несколько кнопок не назначены в инспекторе!");
            }
        }
        else
        {
            // Отключаем кнопки для других режимов
            if (upButton != null) upButton.gameObject.SetActive(false);
            if (downButton != null) downButton.gameObject.SetActive(false);
            if (leftButton != null) leftButton.gameObject.SetActive(false);
            if (rightButton != null) rightButton.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Обработка ввода в зависимости от режима
        ProcessInput();

        // Применение движения
        MovePlayer();

        // Отладочная информация
        Debug.Log($"Скорость игрока: {rb.velocity}, Направление: {currentDirection}, Скорость: {moveSpeed}");

        // Проверка условий окончания игры
        if (bites >= 10)
        {
            EndGame(false);
        }
    }

    void ProcessInput()
    {
        if (controlMode == "ac")
        {
            // Управление акселерометром
            currentDirection = new Vector2(Input.acceleration.x, Input.acceleration.y).normalized;
            if (currentDirection.magnitude < 0.1f) currentDirection = Vector2.zero; // Мёртвая зона
            Debug.Log($"Акселерометр: {currentDirection}");
        }
        else if (controlMode == "swipe")
        {
            // Управление смахиванием
            if (Input.GetMouseButtonDown(0))
            {
                swipeStart = Input.mousePosition;
                isSwiping = true;
                Debug.Log($"Начало смахивания: {swipeStart}");
            }
            else if (Input.GetMouseButtonUp(0) && isSwiping)
            {
                Vector2 swipeEnd = Input.mousePosition;
                Vector2 swipeDelta = swipeEnd - swipeStart;

                if (swipeDelta.magnitude > swipeMinDistance)
                {
                    // Определяем направление
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        currentDirection = swipeDelta.x > 0 ? Vector2.right : Vector2.left;
                    }
                    else
                    {
                        currentDirection = swipeDelta.y > 0 ? Vector2.up : Vector2.down;
                    }
                    Debug.Log($"Смахивание завершено. Направление: {currentDirection}");
                }
                else
                {
                    currentDirection = Vector2.zero;
                    Debug.Log("Смахивание слишком короткое.");
                }
                isSwiping = false;
            }
        }
        // Для кнопок направление устанавливается напрямую через события
    }

    void MovePlayer()
    {
        // Устанавливаем скорость напрямую через Rigidbody2D
        rb.velocity = currentDirection * moveSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            coins++;
            Destroy(other.gameObject);
            Debug.Log($"Монета собрана. Всего монет: {coins}");
        }
        else if (other.CompareTag("Bite"))
        {
            bites++;
            Destroy(other.gameObject);
            Debug.Log($"Укус! Всего укусов: {bites}");
        }
        else if (other.CompareTag("Finish"))
        {
            EndGame(true);
        }
    }

    void EndGame(bool isVictory)
    {
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        resultText.text = isVictory ? "Победа!" : "Поражение!";
        coinsText.text = $"Монеты: {coins}";
        SaveControlSettings();
        Debug.Log($"Игра окончена. Результат: {(isVictory ? "Победа" : "Поражение")}");
    }
}

[System.Serializable]
public class GameSettings
{
    public string controlMode;
    public int coins;
}