using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class generateCoins : MonoBehaviour
{
    public GameObject powerup;
    public GameObject obstacle;
    public float powerupHeight = 1f;
    public float obstacleHeight = 2.5f;
    
    // Configuración SUPER SIMPLE
    public bool useStaticGeneration = true;
    public int totalCoins = 8;           // Exactamente 8 monedas
    public int totalObstacles = 4;       // Exactamente 4 obstáculos
    
    private gamecontrol gameControl;
    private List<GameObject> groundObjects = new List<GameObject>();
    
    void Start()
    {
        gameControl = FindObjectOfType<gamecontrol>();
        
        if (useStaticGeneration)
        {
            FindGroundObjects();
            GenerateExactObjects();
        }
    }
    
    void FindGroundObjects()
    {
        // Buscar objetos Ground de varias formas
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
            Debug.LogError("NO SE ENCONTRARON OBJETOS GROUND! El juego no tendrá monedas ni obstáculos.");
        }
    }
    
    void GenerateExactObjects()
    {
        if (groundObjects.Count == 0) return;
        
        // Mezclar para distribución aleatoria
        Shuffle(groundObjects);
        
        // Generar monedas EXACTAS
        for (int i = 0; i < Mathf.Min(totalCoins, groundObjects.Count); i++)
        {
            GenerateObjectOnGround(groundObjects[i], powerup, powerupHeight);
        }
        
        // Generar obstáculos EXACTOS (usando siguientes objetos)
        int startIndex = Mathf.Min(totalCoins, groundObjects.Count);
        int endIndex = Mathf.Min(startIndex + totalObstacles, groundObjects.Count);
        
        for (int i = startIndex; i < endIndex; i++)
        {
            GenerateObjectOnGround(groundObjects[i], obstacle, obstacleHeight);
        }
        
        Debug.Log("✅ GENERACIÓN EXITOSA: " + 
                 Mathf.Min(totalCoins, groundObjects.Count) + " monedas, " + 
                 Mathf.Max(0, endIndex - startIndex) + " obstáculos");
    }
    
    void GenerateObjectOnGround(GameObject ground, GameObject prefab, float height)
    {
        Vector3 position = GetSafePositionOnGround(ground);
        position.y = height;
        
        GameObject obj = (GameObject)Instantiate(prefab, position, Quaternion.identity);
        Debug.Log(prefab.name + " generado en: " + ground.name);
    }
    
    Vector3 GetSafePositionOnGround(GameObject ground)
    {
        Vector3 groundPos = ground.transform.position;
        
        // Posición siempre en el centro para evitar problemas
        return new Vector3(groundPos.x, groundPos.y, groundPos.z);
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