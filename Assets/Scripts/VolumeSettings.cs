using UnityEngine;
using UnityEngine.Audio; // 1. NECESARIO para usar AudioMixer
using UnityEngine.UI; // NECESARIO para usar Slider

public class VolumeSettings : MonoBehaviour
{
    // 2. Referencia al Mixer creado (arrástralo desde Project al Inspector)
    public AudioMixer mainMixer; 
    
    // 3. Referencia al Slider de Música
    public Slider musicSlider; 

    // Se llama al inicio para cargar la configuración guardada
    void Start()
    {
        // 4. Carga el valor guardado y lo aplica al slider y al mixer
        if (PlayerPrefs.HasKey("MusicVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MusicVolume");
            musicSlider.value = savedVolume;
            SetMusicVolume(savedVolume); 
        }
    }

    /// Función llamada por el Slider para cambiar el volumen de la Música
    public void SetMusicVolume(float volume)
    {
        // El slider va de 0 a 1, el Mixer usa una escala logarítmica (dB)
        if (volume <= 0)
        {
            // Silencia el audio completamente al llegar a 0
            mainMixer.SetFloat("MusicVolume", -80f); 
        }
        else
        {
            // Convierte el valor lineal del slider (0-1) a escala logarítmica (dB)
            float decibels = Mathf.Log10(volume) * 20f;
            mainMixer.SetFloat("MusicVolume", decibels);
        }

        // Guarda el valor del slider para la próxima sesión
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}