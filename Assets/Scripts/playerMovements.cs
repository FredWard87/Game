using UnityEngine;
using System.Collections;

public class playerMovements : MonoBehaviour
{
    public gamecontrol control;
    CharacterController controller;
    public float speed = 8.0f;
    public float jumpSpeed = 10.0f;
    public float gravity = 25.0f;
    public float boostSpeed = 15.0f;
    public float normalSpeed = 8.0f;
    public float rotationSpeed = 15f;
    public Transform cameraTransform;
    
    // Variables para el control de cámara con mouse
    public float mouseSensitivity = 2200f;
    public float cameraDistance = 4f;
    public float cameraHeight = 1.8f;
    public float cameraSmoothness = 8f;
    
    private Vector3 moveDirection = Vector3.zero;
    public float deathHeight = -10f;
    
    // Variables de rotación de cámara
    private float mouseX;
    private float mouseY;
    private float xRotation = 0f;
    private Vector3 cameraOffset;
    
    // Variables para movimiento más fluido
    private Vector3 currentVelocity;
    public float acceleration = 12f;
    public float deceleration = 10f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        
        // Bloquear y ocultar cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        if (control == null)
        {
            control = FindObjectOfType<gamecontrol>();
        }
        
        // Configurar offset inicial de cámara
        cameraOffset = new Vector3(0, cameraHeight, -cameraDistance);
    }

    void Update()
    {
        // ✅ DETENER TODO SI EL JUEGO TERMINÓ
        if (control != null && control.isGameOver)
        {
            // Desactivar este script también para asegurar
            if (control.isGameOver)
            {
                this.enabled = false;
                return;
            }
        }
        
        HandleCameraMovement();
        HandlePlayerMovement();
        
        // Detectar si cayó
        if (transform.position.y < deathHeight)
        {
            GameOver();
            return;
        }
    }

    void HandleCameraMovement()
    {
        // Obtener input del mouse
        mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Rotación vertical (arriba/abajo)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -40f, 80f);

        // Rotación horizontal (izquierda/derecha)
        transform.Rotate(Vector3.up * mouseX);

        // Calcular posición deseada de la cámara
        Quaternion cameraRotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);
        Vector3 desiredCameraPos = transform.position + cameraRotation * cameraOffset;

        // Suavizar movimiento de cámara
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, desiredCameraPos, cameraSmoothness * Time.deltaTime);
        cameraTransform.LookAt(transform.position + Vector3.up * 1.2f);
    }

    void HandlePlayerMovement()
    {
        // CONTROL DE BOOST DE VELOCIDAD
        float targetSpeed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? boostSpeed : normalSpeed;
        speed = Mathf.Lerp(speed, targetSpeed, 8f * Time.deltaTime);

        // MOVIMIENTO DEL PERSONAJE
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        // Calcular dirección relativa a la rotación del personaje
        Vector3 desiredMove = (transform.right * horizontal + transform.forward * vertical).normalized;

        // MOVIMIENTO MÁS FLUIDO con aceleración/desaceleración
        if (desiredMove.magnitude > 0.1f)
        {
            currentVelocity = Vector3.Lerp(currentVelocity, desiredMove * speed, acceleration * Time.deltaTime);
        }
        else
        {
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, deceleration * Time.deltaTime);
        }

        if (controller.isGrounded)
        {
            // Animación de correr
            if (GetComponent<Animation>() != null)
            {
                if (currentVelocity.magnitude > 0.1f)
                {
                    GetComponent<Animation>().Play("run");
                    if (GetComponent<Animation>()["run"] != null)
                    {
                        GetComponent<Animation>()["run"].speed = 1.2f;
                    }
                }
                else
                {
                    GetComponent<Animation>().Stop("run");
                }
            }

            // Aplicar movimiento horizontal más fluido
            moveDirection.x = currentVelocity.x;
            moveDirection.z = currentVelocity.z;

            // SALTO
            if (Input.GetButtonDown("Jump"))
            {
                if (GetComponent<Animation>() != null)
                {
                    GetComponent<Animation>().Stop("run");
                    GetComponent<Animation>().Play("jump_pose");
                }
                
                moveDirection.y = jumpSpeed;
            }
        }
        else
        {
            // Movimiento en aire
            Vector3 airMove = desiredMove * speed * 0.6f;
            moveDirection.x = Mathf.Lerp(moveDirection.x, airMove.x, 4f * Time.deltaTime);
            moveDirection.z = Mathf.Lerp(moveDirection.z, airMove.z, 4f * Time.deltaTime);
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }

    void GameOver()
    {
        if (control != null)
        {
            control.PlayerFell();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Powerup(Clone)")
        {	
            if (control != null) control.PowerupCollected();
        }
        else if (other.gameObject.name == "Obstacle(Clone)")
        {
            if (control != null) control.AlcoholCollected();
        } 
        Destroy(other.gameObject); 
    }
}