using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Цель (игрок)
    public float smoothSpeed = 0.125f; // Скорость сглаживания
    public Vector3 offset = new Vector3(0, 0, -10); // Смещение

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("CameraFollow: Target is not assigned!");
            return;
        }

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

    }
}