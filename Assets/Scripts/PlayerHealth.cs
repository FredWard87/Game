using UnityEngine;
using UnityEngine.UI; // Necesario para Slider e Image

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    
    [Header("Damage Settings")]
    public int rockDamage = 25; // Daño de la roca
    public int obstacleDamage = 10; // Daño del obstáculo
    
    [Header("UI References")]
    public Slider healthBar; // Referencia al componente Slider
    public Image healthFill; // Referencia al componente Image del relleno de la barra
    
    [Header("Health Bar Colors")]
    public Color healthyColor = Color.green;
    public Color warningColor = Color.yellow;
    public Color criticalColor = Color.red;
    
    // Referencia al script principal de control del juego
    private gamecontrol gameControl; 
    
    void Start()
    {
        // Inicializar la vida al máximo
        currentHealth = maxHealth; 
        
        // Buscar el controlador de juego en la escena (debe haber solo uno)
        gameControl = FindObjectOfType<gamecontrol>();
        
        // Configurar la barra de vida al inicio
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }
        
        UpdateHealthBar(); // Llamada inicial para establecer el color
    }
    
    /// <summary>
    /// Aplica daño al jugador y verifica si ha muerto.
    /// </summary>
    /// <param name="damage">Cantidad de daño recibido.</param>
    public void TakeDamage(int damage)
    {
        // Reducir la vida y asegurar que no baje de cero
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log($"Jugador recibió {damage} de daño. Vida actual: {currentHealth}/{maxHealth}");
        
        UpdateHealthBar();
        
        // Si la vida llega o baja de cero, llama a Die
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Cura al jugador, asegurando que no exceda la vida máxima.
    /// </summary>
    /// <param name="amount">Cantidad de vida a curar.</param>
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        
        Debug.Log($"Jugador curado {amount}. Vida actual: {currentHealth}/{maxHealth}");
        
        UpdateHealthBar();
    }
    
    /// <summary>
    /// Actualiza el valor y el color de la barra de vida en la UI.
    /// </summary>
    void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.value = currentHealth;
        }
        
        if (healthFill != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            
            // Lógica de cambio de color
            if (healthPercent > 0.6f) // > 60%
            {
                healthFill.color = healthyColor;
            }
            else if (healthPercent > 0.3f) // > 30%
            {
                healthFill.color = warningColor;
            }
            else // <= 30%
            {
                healthFill.color = criticalColor;
            }
        }
    }
    
    /// <summary>
    /// Se llama cuando la vida del jugador llega a cero.
    /// </summary>
    void Die()
    {
        Debug.Log("¡Jugador ha muerto!");
        
        if (gameControl != null)
        {
            // 🎯 LLAMADA CLAVE: Esto activa la pantalla de Game Over
            gameControl.GameOver();
        }
        
        // Desactivar el jugador para que no siga interactuando o moviéndose.
        // Se recomienda usar 'gameObject.SetActive(false);' si el gamecontrol no lo hace,
        // o deshabilitar solo los componentes de movimiento para efectos de muerte.
        
        // Desactivar controles del jugador:
        if (GetComponent<playerMovements>() != null)
        {
            GetComponent<playerMovements>().enabled = false;
        }
        if (GetComponent<CharacterController>() != null)
        {
            GetComponent<CharacterController>().enabled = false;
        }

        // Alternativa simple: desactivar todo el objeto jugador
        // gameObject.SetActive(false); 
    }
}