using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    //Use this variable to get player's control scheme
    private PlayerInput playerInput;
    private Camera cam;
    
    //If theres a reference to the InputHandler, can now read in this variable
    public Vector2 RawMovementInput { get; private set; }

    public Vector2 RawDodgeDirectionInput { get; private set; }
    //Vector2Int is the same as a vector2 but does not use float variables
    public Vector2Int DodgeDirectionInput { get; private set; }

    public Vector2 RawAimDirectionInput { get; private set; }
    public Vector2Int AimDirectionInput { get; private set; }

    public int NormalisedInputX { get; private set; }
    public int NormalisedInputY { get; private set; }
    public bool DodgeInput { get; private set; }
    public bool DodgeInputStop { get; private set; }
    public bool ShootInput { get; private set; }

    [SerializeField]
    private float inputHoldTime = 0.2f;

    private float dodgeInputStartTime;

    public bool shot = false;


    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        cam = Camera.main;
    }

    private void Update()
    {
        CheckDodgeInputHoldTime();
    }

    //If an input button to move has been pressed, 
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        RawMovementInput = context.ReadValue<Vector2>();

        //NormalisedInputX = (int)(RawMovementInput * Vector2.right).normalized.x;
        //NormalisedInputY = (int)(RawMovementInput * Vector2.up).normalized.y;

        Debug.Log(RawMovementInput);
    }



    //If an input button to shoot has been pressed,
    public void OnShootInput(InputAction.CallbackContext context)
    {
        //If the shoot button has just been pressed down:
        if(context.started)
        {
            //Debug.Log("Shoot button pushed down now");
            shot = true;

        }

        if (context.performed)
        {
            //Debug.Log("Shoot is being held down");

        }

        if (context.canceled)
        {
            //Debug.Log("Shoot button is released");

        }
    }


    public void OnShootDirectionInput(InputAction.CallbackContext context)
    {
        
        //Get a vector2 of the right stick, assuming the player is using a controller
        RawAimDirectionInput = context.ReadValue<Vector2>();

        
        //If the control scheme is set to Keyboard instead, 
        if(playerInput.currentControlScheme == "Keyboard/Mouse")
        {
            //This code gets the position of the mouse
            //To make it a vector pointing from the player to the mouse, subtract the player's position.
            RawAimDirectionInput = cam.ScreenToWorldPoint((Vector3)RawAimDirectionInput) - transform.position;

            //AimDirectionInput = RawAimDirectionInput.normalized;
        }
    }


    public void OnDodgeInput(InputAction.CallbackContext context)
    {
        Debug.Log("Dodge");

        //If the dodge button has been pressed
        if(context.started)
        {
            //Set dodgeInput to true
            DodgeInput = true;
            DodgeInputStop = false;
            dodgeInputStartTime = Time.time;
        }
        else if(context.canceled)
        {
            DodgeInputStop = true;
        }
    }


    public void OnDodgeDirectionInput(InputAction.CallbackContext context)
    {
        //Retrieves a vector2 from the player's input of WASD or Left Stick on Gamepad
        RawDodgeDirectionInput = context.ReadValue<Vector2>();

        DodgeDirectionInput = Vector2Int.RoundToInt(RawDodgeDirectionInput.normalized);
    }

    //Short way to write a function which only sets DodgeInput to false
    //public void UseDodge() => DodgeInput = false;

    public void CheckDodgeInputHoldTime()
    {
        if(Time.time >= dodgeInputStartTime + inputHoldTime)
        {
            DodgeInput = false;
        }
    }


    public void OnInteractInput(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
    }
}
