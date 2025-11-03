using UnityEngine;
using System.Collections;

public class gamecontrol : MonoBehaviour 
{
    float timeRemaining = 10;
    float timeExtension = 3f;
    float timeDeduction = 5f;
    float totalTimeElapsed = 0;
    float score = 0f;
    public bool isGameOver = false;
    
    // Referencias para desactivar componentes
    private GameObject player;
    private generateCoins coinGenerator;
    private playerMovements playerMovement;
    private CharacterController playerController;
    
    void Start()
    {
        Time.timeScale = 1;
        FindPlayerReferences();
        
        // Configurar cursor inicial
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    
    void FindPlayerReferences()
    {
        // Buscar todas las referencias necesarias
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<playerMovements>();
            playerController = player.GetComponent<CharacterController>();
        }
        coinGenerator = FindObjectOfType<generateCoins>();
    }
    
    void Update () 
    { 
        if(isGameOver)
            return;
            
        totalTimeElapsed += Time.deltaTime;
        score = totalTimeElapsed * 100;
        timeRemaining -= Time.deltaTime;
        
        if(timeRemaining <= 0)
        {
            GameOver();
        }
        
        // Permitir ESC para liberar cursor (solo durante el juego)
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    public void PowerupCollected()
    {
        timeRemaining += timeExtension;
    }
    
    public void AlcoholCollected()
    {
        timeRemaining -= timeDeduction;
    }
    
    public void PlayerFell()
    {
        if (!isGameOver)
        {
            GameOver();
        }
    }
    
    void GameOver()
    {
        if (isGameOver) return;
        
        isGameOver = true;
        Debug.Log("GAME OVER ACTIVADO - Deteniendo juego...");
        
        // 1. DESACTIVAR MOVIMIENTO DEL JUGADOR COMPLETAMENTE
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
            Debug.Log("Movimiento del jugador desactivado");
        }
        
        if (playerController != null)
        {
            playerController.enabled = false;
            Debug.Log("CharacterController desactivado");
        }
        
        // 2. DETENER GENERACIÓN DE OBJETOS
        if (coinGenerator != null)
        {
            coinGenerator.enabled = false;
            Debug.Log("Generador de objetos desactivado");
            
            // Destruir objetos existentes SIN USAR TAGS
            DestroyAllPowerupsAndObstacles();
        }
        
        // 3. LIBERAR CURSOR INMEDIATAMENTE
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Cursor liberado");
        
        // 4. DETENER TIEMPO DEL JUEGO
        Time.timeScale = 0f;
        Debug.Log("Tiempo del juego detenido completamente");
    }
    
    void DestroyAllPowerupsAndObstacles()
    {
        // Destruir por nombre en lugar de por tag para evitar errores
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Powerup") || obj.name.Contains("Obstacle"))
            {
                Destroy(obj);
            }
        }
        Debug.Log("Objetos de juego destruidos");
    }
    
    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(); 
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.green;
        
        if(!isGameOver)    
        {
            GUI.Label(new Rect(10, 10, Screen.width/5, Screen.height/6), "TIME LEFT: " + ((int)timeRemaining).ToString(), guiStyle);
            GUI.Label(new Rect(Screen.width - (Screen.width/6), 10, Screen.width/6, Screen.height/6), "SCORE: " + ((int)score).ToString(), guiStyle);
        }
        else
        {
            // Fondo oscuro completo
            GUI.color = new Color(0, 0, 0, 0.95f);
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
            GUI.color = Color.white;
            
            // Estilo para Game Over
            GUIStyle gameOverStyle = new GUIStyle(GUI.skin.box);
            gameOverStyle.alignment = TextAnchor.MiddleCenter;
            gameOverStyle.fontSize = 32;
            gameOverStyle.fontStyle = FontStyle.Bold;
            gameOverStyle.normal.textColor = Color.red;
            
            // Estilo para botones
            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.fontSize = 16;
            buttonStyle.fontStyle = FontStyle.Bold;
            buttonStyle.normal.textColor = Color.white;
            
            // Panel principal de Game Over
            Rect gameOverRect = new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/3);
            GUI.Box(gameOverRect, "🎮 GAME OVER 🎮\n\nFINAL SCORE: " + (int)score, gameOverStyle);
            
            // Botón RESTART - MÁS GRANDE Y VISIBLE
            Rect restartRect = new Rect(Screen.width/2 - 150, Screen.height/2 + 30, 120, 50);
            if (GUI.Button(restartRect, "🔄RESTART", buttonStyle))
            {
                RestartGame();
            }
            
            // Botón QUIT - MÁS GRANDE Y VISIBLE
            Rect quitRect = new Rect(Screen.width/2 + 30, Screen.height/2 + 30, 120, 50);
            if (GUI.Button(quitRect, "❌ QUIT", buttonStyle))
            {
                Application.Quit();
                #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
                #endif
            }
            
            // Mensaje adicional
            GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
            messageStyle.alignment = TextAnchor.MiddleCenter;
            messageStyle.fontSize = 14;
            messageStyle.normal.textColor = Color.yellow;
            
            GUI.Label(new Rect(Screen.width/4, Screen.height/2 + 90, Screen.width/2, 30), 
                     "Presiona RESTART para jugar again", messageStyle);
        }
    }
    
    void RestartGame()
    {
        Debug.Log("Reiniciando juego...");
        
        // RESTAURAR TODO ANTES DE REINICIAR
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        // Recargar escena
        Application.LoadLevel(Application.loadedLevel);
    }
}