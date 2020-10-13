using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private Vector2 movementInput;



    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
        Debug.Log(movementInput);
    }

    public void OnShootInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            Debug.Log("Shoot button pushed down now");

        }

        if (context.performed)
        {
            Debug.Log("Shoot is being held down");

        }

        if (context.canceled)
        {
            Debug.Log("Shoot button is released");

        }
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        Debug.Log("Interact");
    }
}
