using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class generateCoins : MonoBehaviour
{
    public GameObject powerup;
    public GameObject obstacle;
    public float powerupMinHeight = 0.8f;   // 🔥 altura mínima de monedas
    public float powerupMaxHeight = 2f;     // 🔥 altura máxima de monedas
    public float obstacleMinHeight = 2f;    // 🔥 altura mínima de obstáculos
    public float obstacleMaxHeight = 3f;    // 🔥 altura máxima de obstáculos

    public bool useStaticGeneration = true;
    public int totalCoins = 20;             // total de monedas
    public int totalObstacles = 10;         // total de obstáculos
    public int itemsPerPlatform = 4;        // 🔥 número de objetos por plataforma (aprox.)

    private gamecontrol gameControl;
    private List<GameObject> groundObjects = new List<GameObject>();

    void Start()
    {
        gameControl = FindObjectOfType<gamecontrol>();

        if (useStaticGeneration)
        {
            FindGroundObjects();
            GenerateRandomObjects();
        }
    }

    void FindGroundObjects()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Ground") || obj.name.Contains("ground") ||
                obj.name.Contains("Platform") || obj.name.Contains("platform"))
            {
                groundObjects.Add(obj);
                Debug.Log("Encontrado: " + obj.name);
            }
        }

        Debug.Log("Total de objetos válidos encontrados: " + groundObjects.Count);

        if (groundObjects.Count == 0)
        {
            Debug.LogError("❌ NO SE ENCONTRARON OBJETOS GROUND! El juego no tendrá monedas ni obstáculos.");
        }
    }

    void GenerateRandomObjects()
    {
        if (groundObjects.Count == 0) return;

        Shuffle(groundObjects);

        int coinsPlaced = 0;
        int obstaclesPlaced = 0;

        foreach (GameObject ground in groundObjects)
        {
            // Número aleatorio de ítems por plataforma
            int randomCount = Random.Range(2, itemsPerPlatform + 1);

            for (int i = 0; i < randomCount; i++)
            {
                bool spawnCoin = Random.value > 0.4f; // 60% monedas, 40% obstáculos
                if (spawnCoin && coinsPlaced < totalCoins)
                {
                    GenerateObjectOnGround(ground, powerup, powerupMinHeight, powerupMaxHeight);
                    coinsPlaced++;
                }
                else if (!spawnCoin && obstaclesPlaced < totalObstacles)
                {
                    GenerateObjectOnGround(ground, obstacle, obstacleMinHeight, obstacleMaxHeight);
                    obstaclesPlaced++;
                }
            }

            if (coinsPlaced >= totalCoins && obstaclesPlaced >= totalObstacles)
                break;
        }

        Debug.Log($"✅ GENERACIÓN COMPLETA: {coinsPlaced} monedas y {obstaclesPlaced} obstáculos generados.");
    }

    void GenerateObjectOnGround(GameObject ground, GameObject prefab, float minY, float maxY)
    {
        Vector3 position = GetRandomPositionOnGround(ground);
        position.y = Random.Range(minY, maxY); // 🔥 altura aleatoria

        Instantiate(prefab, position, Quaternion.identity);
        Debug.Log($"{prefab.name} generado en {ground.name} en posición {position}");
    }

    Vector3 GetRandomPositionOnGround(GameObject ground)
    {
        Vector3 center = ground.transform.position;
        Vector3 scale = ground.transform.localScale;

        // 🔥 Posición aleatoria dentro del tamaño aproximado del ground
        float randomX = center.x + Random.Range(-scale.x, scale.x);
        float randomZ = center.z + Random.Range(-scale.z, scale.z);

        return new Vector3(randomX, center.y, randomZ);
    }

    void Shuffle(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void Update()
    {
        if (gameControl != null && gameControl.isGameOver) return;
    }
}
