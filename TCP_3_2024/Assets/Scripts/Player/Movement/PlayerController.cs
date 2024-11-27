using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    
    [SerializeField] private Transform pivotGun;
    
    public PlayerMovement PlayerMovement {get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }
    [Networked] private Quaternion networkRotation {get; set;}
    
    private void Awake()
    {
        PlayerInputController = new PlayerInputController(GetComponent<PlayerInput>());
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        PlayerMovement.RotateGun(ref pivotGun);
        PlayerMovement.TurnToCameraDirection();
    }

    private void UpdateNetwork()
    {
        Debug.Log(Object.HasInputAuthority);
        if (Object.HasInputAuthority)
        {
            PlayerMovement.TurnToCameraDirection();
            networkRotation = transform.rotation;
        }else{
            transform.rotation = networkRotation;
        }
        PlayerMovement.RotateGun(ref pivotGun);
        // PlayerMovement.TurnToCameraDirection();
    }

    public override void Spawned()
    {
        base.Spawned();
        transform.rotation = Quaternion.identity;
        
    }

}