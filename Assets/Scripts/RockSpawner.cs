using UnityEngine;

public class IntenseRockSpawner : MonoBehaviour
{
    public GameObject rockPrefab;
    public float spawnHeight = 10f;
    
    // --- CONTROL DE FRECUENCIA (Ahora más bajo para ser más rápido) ---
    public float spawnInterval = 1.5f; // ¡Cae una nueva tanda cada 1.5 segundos!
    
    // --- CONTROL DE CANTIDAD (Nuevo) ---
    [Range(1, 10)] // Permite ajustar fácilmente de 1 a 10 rocas en el Inspector
    public int rocksPerSpawn = 10; // ¡3 rocas por intervalo de tiempo!
    
    public bool spawnOnStart = true;
    public string platformTag = "Ground"; // Tag para buscar plataformas

    [Header("Estrategia de Spawn")]
    public bool spawnOnlyOnOnePlatform = false; // Lo cambiamos a 'false' por defecto para que caiga en todas
    public bool centerSpawnOnly = false; 

    private float nextSpawnTime = 0f;

    void Start()
    {
        if (spawnOnStart)
        {
            SpawnRocksBasedOnStrategy();
        }
    }

    void Update()
    {
        // Spawn continuo
        if (Time.time >= nextSpawnTime)
        {
            SpawnRocksBasedOnStrategy();
            nextSpawnTime = Time.time + spawnInterval;
        }
    }

// ----------------------------------------------------------------------
// MODIFICACIONES CLAVE AQUÍ: La función principal ahora usa un bucle.
// ----------------------------------------------------------------------

    void SpawnRocksBasedOnStrategy()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag(platformTag);

        if (platforms.Length == 0)
        {
            Debug.LogWarning($"No se encontraron plataformas con el tag: {platformTag}");
            return;
        }

        if (spawnOnlyOnOnePlatform)
        {
            // Estrategia: Elegir una plataforma aleatoria
            int randomIndex = Random.Range(0, platforms.Length);
            Transform platform = platforms[randomIndex].transform;
            
            // Bucle para generar MÚLTIPLES rocas en la misma plataforma
            for (int i = 0; i < rocksPerSpawn; i++)
            {
                SpawnRockAbove(platform);
            }
        }
        else
        {
            // Estrategia: Spawnea la cantidad deseada en TODAS las plataformas
            foreach (GameObject platform in platforms)
            {
                for (int i = 0; i < rocksPerSpawn; i++)
                {
                    SpawnRockAbove(platform.transform);
                }
            }
        }
    }
    
// ----------------------------------------------------------------------
// La función SpawnRockAbove se mantiene igual para asegurar la posición.
// ----------------------------------------------------------------------

    void SpawnRockAbove(Transform platform)
    {
        Vector3 spawnPos = platform.position;
        Collider collider = platform.GetComponent<Collider>();

        if (collider != null)
        {
            // 1. Determinar la altura de spawn
            spawnPos.y = collider.bounds.max.y + spawnHeight;

            // 2. Determinar la posición XZ
            if (centerSpawnOnly)
            {
                // Solo en el centro de la plataforma
                spawnPos.x = platform.position.x;
                spawnPos.z = platform.position.z;
            }
            else
            {
                // Posición aleatoria dentro de la plataforma
                float randomX = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
                float randomZ = Random.Range(collider.bounds.min.z, collider.bounds.max.z);
                spawnPos.x = randomX;
                spawnPos.z = randomZ;
            }
        }
        else
        {
            spawnPos.y += spawnHeight;
        }

        // Instanciar la roca
        Instantiate(rockPrefab, spawnPos, Quaternion.identity);
    }
}