using UnityEngine;
using UnityEngine.UI;

public class mecanicas : MonoBehaviour
{
    [Header("Referencia a la imagen de instrucciones")]
    public Image instruccionesImage;

    [Header("Duración del mensaje (segundos)")]
    public float duracionMensaje = 10f;

    // NUEVO: Desactivar al inicio
    private void Start()
    {
        if (instruccionesImage != null)
        {
            instruccionesImage.gameObject.SetActive(false);
        }
    }

    // Llamar este método desde el botón
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