using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Variables")]
    public float moveSpeed = 10f;
    public Vector3 initialPos;
    
    [Header("Jumping Variables")]
    public float jumpForce = 8f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;

    private Vector2 moveDir;
    private bool isReset;
    private bool isJumping;
    
    private bool hasResetThisFrame = false;
    private Rigidbody rb; // Cached reference for performance

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (initialPos == Vector3.zero)
        {
            initialPos = transform.position;
        }
    }

    void Update()
    {
        moveDir = InputManager.instance.getMoveDir();
        isReset = InputManager.instance.IsReset();
        isJumping = InputManager.instance.IsJumping();

        if (isReset)
        {
            if (!hasResetThisFrame)
            {
                ExecuteReset();
                hasResetThisFrame = true;
            }
        }
        else
        {
            hasResetThisFrame = false; 
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isJumping && isGrounded)
        {
            rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            Debug.Log("LeoDero");
        }
    }

    void FixedUpdate()
    {
        Vector3 targetVelocity = new Vector3(moveDir.x * moveSpeed, rb.velocity.y, moveDir.y * moveSpeed);
        rb.velocity = targetVelocity;
    }

    private void ExecuteReset()
    {
        transform.SetPositionAndRotation(initialPos, Quaternion.identity);
        
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (GhostDataClass.instance != null) { GhostDataClass.instance.AddnewReplay(); }
        if (GhostManager.Instance != null) { GhostManager.Instance.Reset(); }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
        }
    }
}