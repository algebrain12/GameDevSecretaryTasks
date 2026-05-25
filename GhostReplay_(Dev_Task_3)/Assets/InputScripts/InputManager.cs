using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{
    public static InputManager instance {get; private set;}
    private IActions inputs;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
        inputs = new IActions();
        inputs.Player.Enable();
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        inputs.Dispose();
    }

    public Vector2 getMoveDir()
    {
        return inputs.Player.Movement.ReadValue<Vector2>();
    }
    public bool IsJumping()
    {
        return inputs.Player.Jump.WasPressedThisFrame();
    }
    public bool IsReset()
    {
        return inputs.Player.Reset.WasPressedThisFrame();
    }

    
}
