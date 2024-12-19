using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class GunFire : NetworkBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerController playerController;
    private PlayerInputController playerInputController;

    [SerializeField] private int damage = 5;

    [SerializeField] private NetworkObject _prefabBall;

    [Networked] private TickTimer delay { get; set; }



    void Start()
    {

        playerInputController = playerController.PlayerInputController;

        if (_prefabBall == null) { 
            Debug.LogError("Prefab da bolinha não está atribuído.");
        }

    }

    private void FixedUpdate()
    {

        if (Object.HasInputAuthority){
            if (playerInputController.FireAction.IsPressed()){

                

                RPC_SpawnBall(transform.forward);
                

                RaycastHit hit;
                if (Physics.Raycast(playerController.camera.transform.position, playerController.camera.transform.forward, out hit, 100, layerMask))
                {
                    Debug.DrawRay(playerController.camera.transform.position, playerController.camera.transform.forward * hit.distance, Color.red);

                    PlayerController hitPayerControllerLife = hit.transform.GetComponent<PlayerController>(); 
                    
                    // Debug.Log("Meu time: "+playerController.Team + "Outro time: " + playerControllerLife.Team);
                    if (hitPayerControllerLife != null)
                    { // Enviar dano para o jogador acertado 
                        RPC_TakeDamage(hitPayerControllerLife,damage); 
                    } 
                
                }
            }
        }
        
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_TakeDamage(PlayerController playerController, int damage) 
    { 
        // ! preciso verificar qual o Time do player que foi atingido pelo tiro, codigo abaixo não funciona

        Debug.Log(playerController.GetTeam());

        if (this.playerController.Team != playerController.Team)
        {
            Debug.Log("Dano no time: " + playerController.Team);
            // playerController.TakeDamage(damage); 
        }else{
            Debug.Log("sem dano no time: " + playerController.Team);
        }
        
            
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SpawnBall(Vector3 transform) 
    { 
        if(delay.ExpiredOrNotRunning(Runner)){
            delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            Runner.Spawn(
                _prefabBall,
                this.transform.position + transform, 
                Quaternion.LookRotation(transform),
                Object.InputAuthority, 
                (runner, o) =>
                {
                    // Initialize the Ball before synchronizing it
                    o.GetComponent<Ball>().Init();

                }
            );
        }
    }


}
