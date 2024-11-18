using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunFire : MonoBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerController playerController;
    private PlayerInputController playerInputController;
    
    void Start()
    {
        
        playerInputController = playerController.PlayerInputController;

    }

    private void FixedUpdate()
    {
        if (playerInputController.FireAction.IsPressed())
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
            { 
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.red); 
                Debug.Log("Did Hit"); 
            }
            else
            { 
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.green); 
                Debug.Log("Did not Hit"); 
            }
        }
        
    }
    
}
