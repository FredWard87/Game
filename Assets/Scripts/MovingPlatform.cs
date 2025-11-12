using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    [Tooltip("Velocidad de movimiento de la plataforma.")]
    public float moveSpeed = 5f;

    [Tooltip("Distancia total que la plataforma recorrerá desde su posición inicial.")]
    public float travelDistance = 10f;

    [Tooltip("Si está activado, la plataforma se moverá horizontalmente (eje X). Si está desactivado, se moverá verticalmente (eje Y).")]
    public bool isHorizontal = true; 

    [Tooltip("Tiempo que la plataforma esperará en cada extremo antes de cambiar de dirección.")]
    public float waitTime = 0.1f; 

    // Variables privadas para la lógica del script
    private Vector3 startPosition; // Posición inicial de la plataforma
    private Vector3 endPosition;   // Posición final o de destino de la plataforma
    private float currentWaitTimer; // Temporizador para el tiempo de espera
    private bool movingToEnd = true; // Indica si la plataforma se está moviendo hacia endPosition (true) o startPosition (false)

    void Start()
    {
        startPosition = transform.position; // Guarda la posición inicial al inicio del juego

        // Define la posición final (endPosition) basándose en si el movimiento es horizontal o vertical
        if (isHorizontal)
        {
            // Movimiento horizontal: Suma 'travelDistance' al eje X de la posición inicial
            endPosition = startPosition + new Vector3(travelDistance, 0, 0);
        }
        else
        {
            // Movimiento vertical: Suma 'travelDistance' al eje Y de la posición inicial
            endPosition = startPosition + new Vector3(0, travelDistance, 0);
        }

        currentWaitTimer = waitTime; // Inicializa el temporizador de espera
    }

    void Update()
    {
        // Si hay tiempo de espera restante, la plataforma no se mueve
        if (currentWaitTimer > 0)
        {
            currentWaitTimer -= Time.deltaTime; // Reduce el temporizador
            return; // Sale de la función Update sin mover la plataforma
        }

        // Lógica de movimiento principal: Mueve la plataforma entre startPosition y endPosition
        if (movingToEnd)
        {
            // Mueve la plataforma hacia endPosition
            transform.position = Vector3.MoveTowards(transform.position, endPosition, moveSpeed * Time.deltaTime);

            // Si la plataforma ha llegado a endPosition
            if (transform.position == endPosition)
            {
                movingToEnd = false;          // Cambia la dirección: ahora se moverá hacia startPosition
                currentWaitTimer = waitTime;  // Reinicia el temporizador de espera
            }
        }
        else
        {
            // Mueve la plataforma hacia startPosition
            transform.position = Vector3.MoveTowards(transform.position, startPosition, moveSpeed * Time.deltaTime);

            // Si la plataforma ha llegado a startPosition
            if (transform.position == startPosition)
            {
                movingToEnd = true;           // Cambia la dirección: ahora se moverá hacia endPosition
                currentWaitTimer = waitTime;  // Reinicia el temporizador de espera
            }
        }
    }

    // Método que se llama cuando un Collider entra en el trigger de la plataforma
    private void OnTriggerEnter(Collider other)
    {
        // Comprueba si el objeto que entró en el trigger tiene la etiqueta "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Hace que el objeto del jugador sea "hijo" de la plataforma.
            // Esto asegura que el jugador se mueva junto con la plataforma.
            other.gameObject.transform.SetParent(transform);
        }
    }

    // Método que se llama cuando un Collider sale del trigger de la plataforma
    private void OnTriggerExit(Collider other)
    {
        // Comprueba si el objeto que salió del trigger tiene la etiqueta "Player"
        if (other.gameObject.CompareTag("Player"))
        {
            // Elimina la relación de padre-hijo, el jugador ya no está sobre la plataforma.
            other.gameObject.transform.SetParent(null);

            // Opcional: Esto evita posibles problemas de escalado si la plataforma tiene escalado no uniforme.
            // Si tu jugador se encoge o se agranda al bajar de la plataforma, puedes necesitar esto.
            // other.gameObject.transform.localScale = Vector3.one; 
        }
    }
}