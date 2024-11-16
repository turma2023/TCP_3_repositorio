using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Photon.Pun.Demo.Cockpit;
using Photon.Pun.Demo.SlotRacer;
using Unity.VisualScripting;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private PlayerInputSystem playerInputSystem;
    public Vector2 movementInput;

    public float verticalInput;
    public float horizontalInput;
    
    private void OnEnable()
    {
        if (playerInputSystem == null)
        {
            playerInputSystem = new PlayerInputSystem();
            playerInputSystem.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
                
        }
        playerInputSystem.Enable();
    }

    private void OnDisable()
    {
        playerInputSystem.Disable();
    }

    public void HandleAllInputs()
    {
        HandleMovementInput();
    }

    
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
    }
}
