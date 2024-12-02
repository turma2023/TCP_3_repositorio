using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class GunFire : NetworkBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerController playerController;
    private PlayerInputController playerInputController;

    [SerializeField] private int damage = 5;

    void Start()
    {

        playerInputController = playerController.PlayerInputController;

    }

    private void FixedUpdate()
    {
        if (playerInputController.FireAction.IsPressed())
        {

            // Ray ray = playerController.camera.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            // RaycastHit hit;
            // if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            // { 
            //     Debug.DrawRay(transform.position, ray.direction * hit.distance, Color.red); 
            //     // Debug.DrawRay(transform.position, hit.point * hit.distance, Color.red); 
            //     Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red); 

            //     Debug.Log("Did Hit");
            // }
            // else
            // { 
            //     Debug.DrawRay(transform.position, ray.direction * 100, Color.green); 
            //     // Debug.DrawRay(transform.position, hit.point * 100, Color.green); 
            //     Debug.DrawRay(ray.origin, ray.direction * 100, Color.green); 

            //     Debug.Log("Did not Hit"); 
            // }
            // // Debug.Log(hit.point);
            // hit.transform.GetComponent<PlayerController>().life = hit.transform.GetComponent<PlayerController>().life - 1;
            // Debug.Log(hit.transform.GetComponent<PlayerController>().life);

            if (Object.HasInputAuthority){
                RaycastHit hit; 
                if (Physics.Raycast(playerController.camera.transform.position, playerController.camera.transform.forward, out hit, 100, layerMask))
                {
                    PlayerController playerControllerLife = hit.transform.GetComponent<PlayerController>(); 
                    Debug.Log(hit.transform.GetComponent<PlayerController>().Team);
                    if (playerControllerLife != null)
                    { // Enviar dano para o jogador acertado 
                        RPC_TakeDamage(playerControllerLife, damage); 
                    } 
                    Debug.DrawRay(playerController.camera.transform.position, playerController.camera.transform.forward * hit.distance, Color.red);
                }
                else{
                    Debug.DrawRay(playerController.camera.transform.position, playerController.camera.transform.forward * hit.distance, Color.green);
                
                }


            }

        }

    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_TakeDamage(PlayerController playerController, int damage) 
    { 
        playerController.TakeDamage(damage); 
    }

}
