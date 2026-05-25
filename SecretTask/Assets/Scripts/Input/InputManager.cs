using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager instance {get; private set;}
    public InputActions Player;
    private void Awake()
    {
        instance = this;
        Player = new InputActions();
        Player.Player.Enable();
    }

    void ODestroy()
    {
        Player.Player.Disable();
    }

    public Vector2 GetMovement()
    {
        return Player.Player.Movement.ReadValue<Vector2>();
    }
}
