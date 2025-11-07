using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Referencia al jugador")]
    public Transform player;

    [Header("Ajustes de cámara")]
    public Vector3 offset = new Vector3(0f, 5f, -10f);  // Posición de la cámara respecto al jugador
    public float smoothSpeed = 0.125f;                   // Velocidad de suavizado

    [Header("Rotación de cámara (opcional)")]
    public Vector3 rotationOffset = new Vector3(10f, 0f, 0f); // Ángulo adicional opcional

    void LateUpdate()
    {
        if (player == null) return;

        // 1️⃣ Calcula la posición deseada de la cámara
        Vector3 desiredPosition = player.position + offset;

        // 2️⃣ Mueve la cámara suavemente hacia esa posición
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        // 3️⃣ Calcula la rotación deseada mirando al jugador
        Quaternion desiredRotation = Quaternion.LookRotation(player.position - transform.position);

        // 4️⃣ Aplica una rotación extra si se desea (opcional)
        desiredRotation *= Quaternion.Euler(rotationOffset);

        // 5️⃣ Suaviza la rotación
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, smoothSpeed);
    }
}
