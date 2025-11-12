using UnityEngine;

public class VolcanoLauncher : MonoBehaviour
{
    [Header("Configuración de la Roca")]
    [Tooltip("Arrastra aquí el Prefab de la roca (ej. Rock_05)")]
    public GameObject rockPrefab;

    [Tooltip("Fuerza con la que se lanzará la roca.")]
    public float launchForce = 15f;

    [Tooltip("Tiempo mínimo y máximo entre lanzamientos de rocas.")]
    public Vector2 launchInterval = new Vector2(2f, 5f); // Min y Max segundos

    [Tooltip("Inclinación vertical del lanzamiento (0 = recto, 45 = ángulo).")]
    [Range(0, 90)] // Permite valores entre 0 y 90 grados
    public float launchAngle = 45f; 

    [Header("Configuración Opcional")]
    [Tooltip("Radio del área de donde se lanzarán las rocas desde el volcán.")]
    public float spawnRadius = 0.5f;

    private float nextLaunchTime;

    void Start()
    {
        // Establece el primer tiempo de lanzamiento aleatorio
        SetNextLaunchTime();
    }

    void Update()
    {
        // Si ha pasado el tiempo para el próximo lanzamiento
        if (Time.time >= nextLaunchTime)
        {
            LaunchBurst();       // <<-- AHORA LLAMA A LA FUNCIÓN DE EXPLOSIÓN
            SetNextLaunchTime(); // Programa el próximo lanzamiento
        }
    }

    void SetNextLaunchTime()
    {
        // Calcula un tiempo de espera aleatorio dentro del intervalo definido
        nextLaunchTime = Time.time + Random.Range(launchInterval.x, launchInterval.y);
    }
    
    // ====================================================================
    // NUEVA FUNCIÓN: LANZA 4 ROCAS
    // ====================================================================
    void LaunchBurst()
    {
        // Bucle que ejecuta la función de lanzamiento individual 4 veces.
        for (int i = 0; i < 2; i++)
        {
            LaunchSingleRock();
        }
    }
    // ====================================================================

    // CAMBIO DE NOMBRE: LaunchRock A LaunchSingleRock
    void LaunchSingleRock()
    {
        if (rockPrefab == null)
        {
            Debug.LogWarning("El Prefab de roca no está asignado en VolcanoLauncher.", this);
            return;
        }

        // 1. Calcula la posición de spawn
        // Para que no salgan todas del mismo punto exacto, añade un pequeño offset aleatorio
        Vector3 spawnOffset = Random.insideUnitSphere * spawnRadius;
        spawnOffset.y = Mathf.Abs(spawnOffset.y); // Asegura que las rocas spawneen ligeramente por encima o a nivel del origen
        Vector3 spawnPosition = transform.position + spawnOffset;

        // 2. Instancia la roca
        GameObject newRock = Instantiate(rockPrefab, spawnPosition, Quaternion.identity);

        // 3. Aplica la fuerza de lanzamiento
        Rigidbody rockRigidbody = newRock.GetComponent<Rigidbody>();
        if (rockRigidbody != null)
        {
            // Calcula la dirección de lanzamiento
            // Por defecto, se lanzará hacia arriba y un poco hacia adelante (depende de la rotación del GameObject del launcher)
            // Para una inclinación específica, podemos usar un vector calculado
            Vector3 launchDirection = (transform.forward + transform.up * (Mathf.Tan(launchAngle * Mathf.Deg2Rad))).normalized;
            
            // Aplica la fuerza
            rockRigidbody.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("La roca lanzada no tiene un Rigidbody.", newRock);
        }

        // Opcional: Destruir la roca después de un tiempo para no acumular demasiados objetos
        Destroy(newRock, 10f); // La roca se destruirá después de 10 segundos
    }

    // Para visualizar el punto de spawn y la dirección de lanzamiento en el editor
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius); // Dibuja una esfera para el radio de spawn

        // Dibuja una flecha para la dirección de lanzamiento
        Vector3 launchDir = (transform.forward + transform.up * (Mathf.Tan(launchAngle * Mathf.Deg2Rad))).normalized;
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, launchDir * launchForce * 0.5f); // La longitud de la flecha es proporcional a la fuerza
    }
}