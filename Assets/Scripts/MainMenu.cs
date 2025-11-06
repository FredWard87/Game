using UnityEngine;
using UnityEngine.SceneManagement;  // Para cambiar de escena
using UnityEngine.UI;               // Para los botones

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;

    [Header("Buttons")]
    public Button startButton;
    public Button optionsButton;
    public Button backButton;
    public Button quitButton;

    void Start()
    {
        // Asegura que el menú principal esté visible y las opciones ocultas
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);

        // Asigna eventos a los botones
        startButton.onClick.AddListener(StartGame);
        optionsButton.onClick.AddListener(OpenOptions);
        backButton.onClick.AddListener(CloseOptions);
        quitButton.onClick.AddListener(QuitGame);
    }

    public void StartGame()
    {
        Debug.Log("Starting the game...");
        // Carga la siguiente escena (asegúrate de agregarla en Build Settings)
        SceneManager.LoadScene("level");  
    }

    public void OpenOptions()
    {
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
    }

    public void CloseOptions()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        Application.Quit();

        // Esto es útil para probar en el editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
