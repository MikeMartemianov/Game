using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

public class MazeGenerator : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject playerPrefab;
    public GameObject finishPrefab;
    public GameObject[] itemPrefabs; // Префабы объектов (яйца, укусы, монеты, морковки)
    public int[] itemCounts; // Количество каждого объекта
    private int[,] maze = new int[21, 21]; // 0 - проход, 1 - стена
    private GameObject[,] wallObjects = new GameObject[21, 21];
    private List<Vector2Int> openCells = new List<Vector2Int>();
    private bool isMazeFullyRevealed;
    private GameObject finish;

    void Start()
    {
        GenerateMaze();
        PlaceWalls();
        PlacePlayer();
        PlaceItems();
    }

    void GenerateMaze()
    {
        // Инициализация: всё поле — проходы (0)
        for (int x = 0; x < 21; x++)
            for (int y = 0; y < 21; y++)
                maze[x, y] = 0;

        // Создаём внешние стены
        for (int x = 0; x < 21; x++)
        {
            maze[x, 0] = 1;
            maze[x, 20] = 1;
        }
        for (int y = 0; y < 21; y++)
        {
            maze[0, y] = 1;
            maze[20, y] = 1;
        }

        // Генерация лабиринта с помощью рекурсивного разделения
        Divide(1, 1, 19, 19);

        // Сохраняем все проходы
        openCells.Clear();
        for (int x = 0; x < 21; x++)
            for (int y = 0; y < 21; y++)
                if (maze[x, y] == 0)
                    openCells.Add(new Vector2Int(x, y));
    }

    void Divide(int x1, int y1, int x2, int y2)
    {
        if (x2 - x1 < 2 || y2 - y1 < 2) return;

        // Выбираем, делить горизонтально или вертикально
        bool horizontal = Random.value > 0.5f;
        int split;
        int passage;

        if (horizontal)
        {
            split = Random.Range(y1 + 1, y2); // Где ставим стену
            passage = Random.Range(x1, x2 + 1); // Где ставим проход

            // Создаём горизонтальную стену с одним проходом
            for (int x = x1; x <= x2; x++)
                maze[x, split] = 1;
            maze[passage, split] = 0;
        }
        else
        {
            split = Random.Range(x1 + 1, x2); // Где ставим стену
            passage = Random.Range(y1, y2 + 1); // Где ставим проход

            // Создаём вертикальную стену с одним проходом
            for (int y = y1; y <= y2; y++)
                maze[split, y] = 1;
            maze[split, passage] = 0;
        }

        // Рекурсивно делим две области
        if (horizontal)
        {
            Divide(x1, y1, x2, split - 1); // Верхняя часть
            Divide(x1, split + 1, x2, y2); // Нижняя часть
        }
        else
        {
            Divide(x1, y1, split - 1, y2); // Левая часть
            Divide(split + 1, y1, x2, y2); // Правая часть
        }
    }

    void PlaceWalls()
    {
        for (int x = 0; x < 21; x++)
        {
            for (int y = 0; y < 21; y++)
            {
                if (maze[x, y] == 1)
                {
                    Vector3 pos = new Vector3(x - 10, y - 10, 0); // Центрируем лабиринт
                    GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
                    SpriteRenderer sr = wall.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.color = new Color(1, 1, 1, 0); // Невидимая стена
                    }
                    else
                    {
                        Debug.LogError($"SpriteRenderer отсутствует на объекте {wall.name}");
                    }
                    wallObjects[x, y] = wall;
                }
            }
        }
    }

    void PlacePlayer()
    {
        if (openCells.Count == 0) Debug.LogError("Нет доступных клеток для игрока!");
        Vector2Int randomCell = openCells[Random.Range(0, openCells.Count)];
        Vector3 pos = new Vector3(randomCell.x - 10, randomCell.y - 10, 0);
        Instantiate(playerPrefab, pos, Quaternion.identity);
    }

    void PlaceItems()
    {
        List<Vector2Int> availableCells = new List<Vector2Int>(openCells);
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            for (int j = 0; j < itemCounts[i] && availableCells.Count > 0; j++)
            {
                int index = Random.Range(0, availableCells.Count);
                Vector2Int cell = availableCells[index];
                Vector3 pos = new Vector3(cell.x - 10, cell.y - 10, 0);
                Instantiate(itemPrefabs[i], pos, Quaternion.identity);
                availableCells.RemoveAt(index);
            }
        }
    }

    public void RevealWall(int x, int y)
    {
        if (x >= 0 && x < 21 && y >= 0 && y < 21 && wallObjects[x, y] != null)
        {
            SpriteRenderer sr = wallObjects[x, y].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.color = new Color(1, 1, 1, 1); // Делаем стену видимой
            }
        }
        CheckMazeRevealed();
    }

    void CheckMazeRevealed()
    {
        isMazeFullyRevealed = true;
        for (int x = 0; x < 21; x++)
        {
            for (int y = 0; y < 21; y++)
            {
                if (maze[x, y] == 1 && wallObjects[x, y] != null)
                {
                    SpriteRenderer sr = wallObjects[x, y].GetComponent<SpriteRenderer>();
                    if (sr != null && sr.color.a == 0)
                    {
                        isMazeFullyRevealed = false;
                        return;
                    }
                }
            }
        }
    }

    public void TrySpawnFinish(int coins, int bites)
    {
        if (isMazeFullyRevealed && coins >= 5 && bites == 0 && finish == null)
        {
            Vector2Int randomCell = openCells[Random.Range(0, openCells.Count)];
            Vector3 pos = new Vector3(randomCell.x - 10, randomCell.y - 10, 0);
            finish = Instantiate(finishPrefab, pos, Quaternion.identity);
        }
    }
}