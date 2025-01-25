using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

// [DisallowMultipleComponent]

public class PlayerAction : NetworkBehaviour
{
    
    [SerializeField]
    private PlayerGunSelector GunSelector;

    private PlayerInputController playerInputController;
    [SerializeField] private PlayerController playerController;
    private void Start()
    {
        playerInputController = playerController.PlayerInputController;

    }

    private void Update()
    {
        // if (Object.HasInputAuthority){
            if(playerInputController.FireAction.IsPressed()){
                GunSelector.ActiveGun.Shoot();
            }
        // }

    }

}
