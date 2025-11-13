using UnityEngine;
using UnityEngine.UI;

public class mecanicas : MonoBehaviour
{
    [Header("Referencia a la imagen de instrucciones")]
    public Image instruccionesImage;

    [Header("Duración del mensaje (segundos)")]
    public float duracionMensaje = 10f;

    // Desactivar la imagen al iniciar el juego
    private void Start()
    {
        if (instruccionesImage != null)
        {
            instruccionesImage.gameObject.SetActive(false);
        }
        // ¡QUITÉ todo lo demás! Solo debe desactivarse aquí
    }

    // Llamar este método desde el botón "Mecanicas"
    public void MostrarInstrucciones()
    {
        if (instruccionesImage == null)
        {
            Debug.LogWarning("No se asignó la imagen de instrucciones en el inspector.");
            return;
        }

        // Mostrar la imagen
        instruccionesImage.gameObject.SetActive(true);

        // Ocultar después de unos segundos
        Invoke(nameof(OcultarInstrucciones), duracionMensaje);
    }

    private void OcultarInstrucciones()
    {
        instruccionesImage.gameObject.SetActive(false);
    }
}