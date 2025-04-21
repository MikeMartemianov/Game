using UnityEngine;
using UnityEngine.UI;

public class CoinsMenu : MonoBehaviour
{
    [SerializeField] private RectTransform menuPanel;
    [SerializeField] private Button toggleButton;
    [SerializeField] private float animationSpeed = 5f;

    private Vector2 closedPosition;
    private Vector2 openPosition;
    private bool isOpen = false;

    void Awake()
    {
        if (menuPanel == null)
        {
            Debug.LogError("Menu Panel not assigned!");
            return;
        }
        if (toggleButton == null)
        {
            Debug.LogError("Toggle Button not assigned!");
            return;
        }

        // Открытая позиция - текущая позиция панели
        openPosition = menuPanel.anchoredPosition;
        // Закрытая позиция - ниже на высоту панели
        closedPosition = openPosition + new Vector2(0, -menuPanel.rect.height);
        menuPanel.anchoredPosition = closedPosition;
    }

    void Start()
    {
        toggleButton.onClick.AddListener(ToggleMenu);
    }

    void Update()
    {
        menuPanel.anchoredPosition = Vector2.Lerp(
            menuPanel.anchoredPosition,
            isOpen ? openPosition : closedPosition,
            Time.deltaTime * animationSpeed
        );
    }

    void OnDestroy()
    {
        toggleButton.onClick.RemoveListener(ToggleMenu);
    }

    public void ToggleMenu()
    {
        isOpen = !isOpen;
        Debug.Log(isOpen ? "Settings Menu opened" : "Settings Menu closed");
    }
}