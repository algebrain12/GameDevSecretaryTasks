using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public float movespeed = 10f;
    public GameObject player;

    public GameObject Flag;
    public Transform cameraTransform;
    public float cameraSmoothSpeed = 5f; 
    private Camera mainCamera;
    public LayerMask playerObjects;
    private Rigidbody2D playerRB;

    void Start()
    {
        playerRB = player.GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
        
        if (cameraTransform == null && mainCamera != null)
        {
            cameraTransform = mainCamera.transform;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            SelectObject();
        }
    }

    void FixedUpdate()
    {  
        Vector2 input = InputManager.instance.GetMovement();
        if (input.sqrMagnitude > 0.01f)
        {
            float accelerationForce = movespeed * 5f; 
            playerRB.AddForce(input * accelerationForce);
        }
        else
        {
            float frictionCoefficient = 0.15f;
            playerRB.velocity = Vector2.Lerp(playerRB.velocity, Vector2.zero, frictionCoefficient);
        }
        if (playerRB.velocity.magnitude > movespeed)
        {
            playerRB.velocity = playerRB.velocity.normalized * movespeed;
        }
    }

    void LateUpdate()
    {
        Vector3 targetCameraPosition = new Vector3(
            player.transform.position.x,
            player.transform.position.y,
            cameraTransform.position.z 
        );
        Vector3 smoothedPosition = Vector3.Lerp(cameraTransform.position, targetCameraPosition, cameraSmoothSpeed * Time.deltaTime);
        
        cameraTransform.position = smoothedPosition;
    }

    private void SelectObject()
    {
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos, Vector2.zero);
        if (hit.collider != null)
        {
            player = hit.collider.gameObject;
            playerRB = player.GetComponent<Rigidbody2D>();
            if(player == Flag)
            {
                Debug.Log("You win");
                InputManager.instance.Player.Player.Disable();
                SceneManager.LoadScene(2);
            }
        }
    }
}