using UnityEngine;
using UnityEngine.SceneManagement; // **1. Importante: Necesario para cambiar de escena.**

public class LevelLoader : MonoBehaviour
{
    // Nombre de la escena que quieres cargar cuando se llama a esta función.
    // Asegúrate de que este nombre coincida EXACTAMENTE con el nombre de tu archivo de escena.
    [SerializeField] // Permite editar esta variable en el Inspector de Unity.
    private string sceneToLoad = "level"; // **Asegúrate de cambiar "Level" por el nombre real de tu escena.**

    /// <summary>
    /// Función pública que se llama cuando se hace clic en el botón del nivel.
    /// </summary>
    public void LoadLevelScene()
    {
        // Carga la escena cuyo nombre está guardado en la variable 'sceneToLoad'.
        SceneManager.LoadScene(sceneToLoad);
        
        // Opcional: Puedes añadir un mensaje en la consola para confirmar que funciona
        Debug.Log("Cargando escena: " + sceneToLoad);
    }
}