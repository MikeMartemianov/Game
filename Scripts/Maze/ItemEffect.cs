using UnityEngine;

public class ItemEffect : MonoBehaviour
{
    public string itemType; // "Egg", "Bite", "Coin", "Carrot"

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (itemType)
            {
                case "Egg":
                    Debug.Log("Собрано яйцо!");
                    break;
                case "Bite":
                    Debug.Log("Получен укус!");
                    break;
                case "Coin":
                    Debug.Log("Собрана монета!");
                    break;
                case "Carrot":
                    Debug.Log("Собрана морковка!");
                    break;
            }
        }
    }
}