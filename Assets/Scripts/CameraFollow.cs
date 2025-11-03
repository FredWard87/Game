using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // El personaje a seguir
    public Vector3 offset = new Vector3(0, 2, -5); // Distancia de la cámara
    public float rotationSpeed = 2f;
    public float followSpeed = 5f;
    
    private float currentRotationX = 0f;
    private float currentRotationY = 0f;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Rotación de la cámara con mouse
        if (Input.GetMouseButton(1)) // Botón derecho del mouse
        {
            currentRotationX += Input.GetAxis("Mouse X") * rotationSpeed;
            currentRotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;
            currentRotationY = Mathf.Clamp(currentRotationY, -30, 60);
        }

        // Calcular rotación
        Quaternion rotation = Quaternion.Euler(currentRotationY, currentRotationX, 0);
        
        // Posición de la cámara
        Vector3 desiredPosition = target.position + rotation * offset;
        
        // Suavizar movimiento
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f); // Mirar al personaje
    }
}