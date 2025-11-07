using UnityEngine;

public class FallingRock : MonoBehaviour
{
    public float detectionRange = 5f;
    public float fallSpeed = 10f;
    public float destroyDelay = 5f;
    public float heightAboveGround = 10f; // Nueva variable
    
    private bool isFalling = false;
    private Transform player;
    private Rigidbody rb;
    
    void Start()
    {
        // Posicionar la roca sobre la plataforma Ground más cercana
        PositionAboveGround();
        
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
        
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    
    void PositionAboveGround()
    {
        // Buscar todas las plataformas Ground
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        GameObject closestGround = null;
        float closestDistance = Mathf.Infinity;
        
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.Contains("Ground"))
            {
                float distance = Vector3.Distance(transform.position, obj.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestGround = obj;
                }
            }
        }
        
        if (closestGround != null)
        {
            Vector3 newPos = closestGround.transform.position;
            Collider collider = closestGround.GetComponent<Collider>();
            
            if (collider != null)
            {
                newPos.y = collider.bounds.max.y + heightAboveGround;
            }
            else
            {
                newPos.y += heightAboveGround;
            }
            
            transform.position = newPos;
        }
    }
    
    void Update()
    {
        if (isFalling || player == null) return;
        
        Vector3 playerPos = new Vector3(player.position.x, transform.position.y, player.position.z);
        float distance = Vector3.Distance(new Vector3(transform.position.x, transform.position.y, transform.position.z), playerPos);
        
        if (distance <= detectionRange)
        {
            StartFalling();
        }
    }
    
    void StartFalling()
    {
        isFalling = true;
        rb.isKinematic = false;
        rb.useGravity = true;
        Destroy(gameObject, destroyDelay);
    }
    
   void OnCollisionEnter(Collision collision)
{
    if (collision.gameObject.CompareTag("Player"))
    {
        Debug.Log("¡La roca golpeó al jugador!");
        
        // Aplicar daño
        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(playerHealth.rockDamage);
        }
        
        // Destruir la roca inmediatamente al golpear
        Destroy(gameObject);
    }
}
    
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}