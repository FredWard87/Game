using UnityEngine;
using TMPro; // para usar TextMeshPro
using UnityEngine.UI;

public class mecanicas : MonoBehaviour
{
    [Header("Referencia al texto de instrucciones")]
    public TextMeshProUGUI instruccionesText;

    [Header("Duración del mensaje (segundos)")]
    public float duracionMensaje = 10f;

    // Llamar este método desde el botón
    public void MostrarInstrucciones()
    {
        if (instruccionesText == null)
        {
            Debug.LogWarning("No se asignó el texto de instrucciones en el inspector.");
            return;
        }

        // Texto que aparecerá en pantalla
        string mensaje = 
@"Te mueves con: W A S D
Saltas con: Barra espaciadora
Corres con: Shift
Mueves la cámara con: Mouse
Tienes 12 segundos iniciales
Por cada plátano: +5 segundos
Completas el nivel al obtener 35 plátanos";

        instruccionesText.text = mensaje;
        instruccionesText.gameObject.SetActive(true);

        // Ocultar después de unos segundos
        Invoke(nameof(OcultarInstrucciones), duracionMensaje);
    }

    private void OcultarInstrucciones()
    {
        instruccionesText.gameObject.SetActive(false);
    }
}
