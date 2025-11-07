using UnityEngine;
using UnityEngine.EventSystems;

// Estas interfaces detectan el mouse
public class ButtonHoverSound : MonoBehaviour, IPointerEnterHandler
{
    // El clip de sonido de hover
    public AudioClip hoverClip;

    // El componente AudioSource que reproducirá el clip
    private AudioSource audioSource;

    void Start()
    {
        // Busca o añade un AudioSource en este objeto (o en otro)
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // Se llama cuando el puntero entra en el área del botón
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(hoverClip);
        }
    }
}