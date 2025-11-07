using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gamecontrol : MonoBehaviour 
{
    float timeRemaining = 10;
    float timeExtension = 3f;
    float timeDeduction = 5f;
    float totalTimeElapsed = 0;
    float score = 0f;
    public bool isGameOver = false;
    public bool hasWon = false;

    public int totalPowerups = 0;
    public int collectedPowerups = 0;

    private GameObject player;
    private generateCoins coinGenerator;
    private playerMovements playerMovement;
    private CharacterController playerController;

    // ⚠️ Lista de frases breves sobre la caza de monos
    private List<string> awarenessMessages = new List<string>();
    private string currentMessage = "";

    void Start()
    {
        Time.timeScale = 1;
        FindPlayerReferences();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        CountTotalPowerups();
        InitializeAwarenessMessages();
    }

    void FindPlayerReferences()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<playerMovements>();
            playerController = player.GetComponent<CharacterController>();
        }
        coinGenerator = FindObjectOfType<generateCoins>();
    }

    void CountTotalPowerups()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Powerup"))
                totalPowerups++;
        }
        Debug.Log($"Total de powerups en la escena: {totalPowerups}");
    }

    void InitializeAwarenessMessages()
    {
        awarenessMessages = new List<string>()
        {
            "Los monos no son\nmascotas, déjalos\nvivir en libertad.",
            "Cada mono cazado\nrompe una familia\nen la selva.",
            "Sin monos, el bosque\npierde su equilibrio\nnatural y vital.",
            "Proteger a un mono\nes cuidar el planeta\ny su futuro.",
            "El tráfico de fauna\npone en peligro\nespecies enteras.",
            "Los monos sienten\nmiedo y dolor.\nNo los lastimes.",
            "Cazar un mono es\nmatar la selva\nlentamente.",
            "La selva sin monos\nno tiene vida\nauténtica.",
            "No compres animales,\nprotege su hogar\nnatural.",
            "Salvar un mono es\nsalvar miles de\nárboles también."
        };
    }

    void Update() 
    { 
        if (isGameOver || hasWon)
            return;
            
        totalTimeElapsed += Time.deltaTime;
        score = totalTimeElapsed * 100;
        timeRemaining -= Time.deltaTime;
        
        if (timeRemaining <= 0)
        {
            GameOver();
        }
        
        if (Input.GetKeyDown(KeyCode.Escape) && !isGameOver)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
    
    public void PowerupCollected()
    {
        timeRemaining += timeExtension;
        collectedPowerups++;

        if (collectedPowerups >= totalPowerups && totalPowerups > 0)
        {
            WinGame();
        }
    }
    
    public void AlcoholCollected()
    {
        timeRemaining -= timeDeduction;
    }
    
    public void PlayerFell()
    {
        if (!isGameOver && !hasWon)
        {
            GameOver();
        }
    }
    
    public void GameOver()
    {
        if (isGameOver || hasWon) return;
        
        isGameOver = true;
        Debug.Log("GAME OVER ACTIVADO - Deteniendo juego...");

        // 🎯 Selecciona mensaje corto aleatorio
        currentMessage = awarenessMessages[Random.Range(0, awarenessMessages.Count)];

        DisableGameplay();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    void WinGame()
    {
        hasWon = true;
        DisableGameplay();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
    }

    void DisableGameplay()
    {
        if (playerMovement != null)
            playerMovement.enabled = false;
        if (playerController != null)
            playerController.enabled = false;
        if (coinGenerator != null)
            coinGenerator.enabled = false;
    }

    void OnGUI()
    {
        GUIStyle guiStyle = new GUIStyle(); 
        guiStyle.fontSize = 20;
        guiStyle.normal.textColor = Color.green;
        
        if (!isGameOver && !hasWon)    
        {
            GUI.Label(new Rect(10, 10, 300, 30), $"TIME LEFT: {(int)timeRemaining}", guiStyle);
            GUI.Label(new Rect(Screen.width - 200, 10, 200, 30), $"SCORE: {(int)score}", guiStyle);
            GUI.Label(new Rect(10, 40, 300, 30), $"POWERUPS: {collectedPowerups}/{totalPowerups}", guiStyle);
        }
        else if (isGameOver)
        {
            DrawGameOverScreen();
        }
        else if (hasWon)
        {
            DrawWinScreen();
        }
    }

    void DrawGameOverScreen()
    {
        // Fondo oscuro
        GUI.color = new Color(0, 0, 0, 0.92f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        // 🎮 Título principal
        GUIStyle titleStyle = new GUIStyle(GUI.skin.label);
        titleStyle.alignment = TextAnchor.MiddleCenter;
        titleStyle.fontSize = 36;
        titleStyle.fontStyle = FontStyle.Bold;
        titleStyle.normal.textColor = Color.red;
        GUI.Label(new Rect(0, Screen.height/3 - 40, Screen.width, 40), "💀 GAME OVER 💀", titleStyle);

        // 💬 Mensaje de conciencia (doble más arriba, fuente más pequeña)
        GUIStyle messageStyle = new GUIStyle(GUI.skin.label);
        messageStyle.alignment = TextAnchor.MiddleCenter;
        messageStyle.fontSize = 15;
        messageStyle.wordWrap = true;
        messageStyle.normal.textColor = new Color(0.85f, 0.85f, 0.85f);

        GUI.Label(
            new Rect(Screen.width / 4 + 40, Screen.height / 6 + 40, Screen.width / 2 - 80, 140),
            currentMessage,
            messageStyle
        );

        // 🎯 Puntaje final
        GUIStyle scoreStyle = new GUIStyle(GUI.skin.label);
        scoreStyle.alignment = TextAnchor.MiddleCenter;
        scoreStyle.fontSize = 22;
        scoreStyle.normal.textColor = Color.yellow;
        GUI.Label(new Rect(0, Screen.height / 2, Screen.width, 30), $"FINAL SCORE: {(int)score}", scoreStyle);

        // 🔘 Botones
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 16;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.normal.textColor = Color.white;

        Rect restartRect = new Rect(Screen.width / 2 - 150, Screen.height / 2 + 60, 120, 45);
        if (GUI.Button(restartRect, "🔄 RESTART", buttonStyle))
        {
            RestartGame();
        }

        Rect quitRect = new Rect(Screen.width / 2 + 30, Screen.height / 2 + 60, 120, 45);
        if (GUI.Button(quitRect, "❌ QUIT", buttonStyle))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    void DrawWinScreen()
    {
        GUI.color = new Color(0, 0, 0, 0.9f);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        GUIStyle boxStyle = new GUIStyle(GUI.skin.box);
        boxStyle.alignment = TextAnchor.MiddleCenter;
        boxStyle.fontSize = 30;
        boxStyle.fontStyle = FontStyle.Bold;
        boxStyle.normal.textColor = Color.yellow;

        GUI.Box(new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/3),
                $"🏆 ¡FELICIDADES!\nHAS GANADO 🎉\n\nSCORE FINAL: {(int)score}", boxStyle);

        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
        buttonStyle.fontSize = 16;
        buttonStyle.fontStyle = FontStyle.Bold;
        buttonStyle.normal.textColor = Color.white;

        Rect restartRect = new Rect(Screen.width/2 - 150, Screen.height/2 + 40, 120, 45);
        if (GUI.Button(restartRect, "🔄 RESTART", buttonStyle))
        {
            RestartGame();
        }

        Rect quitRect = new Rect(Screen.width/2 + 30, Screen.height/2 + 40, 120, 45);
        if (GUI.Button(quitRect, "❌ QUIT", buttonStyle))
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }
    }

    void RestartGame()
    {
        Debug.Log("Reiniciando juego...");
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Application.LoadLevel(Application.loadedLevel);
    }
}
