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
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            { 
                Debug.DrawRay(transform.position, ray.direction * hit.distance, Color.red); 
                // Debug.DrawRay(transform.position, hit.point * hit.distance, Color.red); 
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red); 

                Debug.Log("Did Hit"); 
            }
            else
            { 
                Debug.DrawRay(transform.position, ray.direction * 100, Color.green); 
                // Debug.DrawRay(transform.position, hit.point * 100, Color.green); 
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.green); 

                Debug.Log("Did not Hit"); 
            }
            Debug.Log(hit.point);
        }
        
    }
    
}
