using Cinemachine;
using Fusion;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    
    [SerializeField] private Transform pivotGun;
    public Camera camera;
    [SerializeField] private Transform playerCameraPosition;
    
    public PlayerMovement PlayerMovement {get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }

    [Networked] private Quaternion networkRotation {get; set;}
    [Networked] private Quaternion networkPivotGun {get; set;}
    [Networked] private Vector3 networkPosition {get; set;}
    
    private void Awake()
    {
        PlayerInputController = new PlayerInputController(GetComponent<PlayerInput>());
        PlayerMovement = GetComponent<PlayerMovement>();
    }

    // public NetworkVariable<Quaternion> networkRotation = new NetworkVariable<Quaternion>(); 
    // public NetworkVariable<Quaternion> networkPivotGun = new NetworkVariable<Quaternion>();

    private void FixedUpdate()
    {
        // PlayerMovement.RotateGun(ref pivotGun);
        // PlayerMovement.TurnToCameraDirection();

        if (Object.HasInputAuthority)
        {
            PlayerMovement.TurnToCameraDirection();
            PlayerMovement.RotateGun(ref pivotGun);
            networkRotation = transform.rotation;
            networkPivotGun = pivotGun.rotation;
            networkPosition = transform.position;
            RPC_SendRotationToHost(transform.rotation, pivotGun.rotation, transform.position);

        }else{
            transform.rotation = networkRotation;
            pivotGun.rotation = networkPivotGun;
            transform.position = networkPosition;
        }
            

    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)] 
    private void RPC_SendRotationToHost(Quaternion playerRotation, Quaternion gunRotation, Vector3 playerPosition) 
    { 
        networkRotation = playerRotation; 
        networkPivotGun = gunRotation;
        networkPosition = playerPosition;
    }

    public void UpdateNetwork()
    {
        // Debug.Log(Object.HasInputAuthority);
        if (Object.HasInputAuthority)
        {
            PlayerMovement.TurnToCameraDirection();
            networkRotation = transform.rotation;
            Debug.Log("entrou aqui TRUE");
        }else{
            transform.rotation = networkRotation;
            Debug.Log("entrou aqui FALSE");
        }
        PlayerMovement.RotateGun(ref pivotGun);

        // PlayerMovement.TurnToCameraDirection();
    }

    public override void Spawned()
    {
        base.Spawned();
        transform.rotation = Quaternion.identity;


        if (Object.HasInputAuthority) 
        { 
            camera.gameObject.SetActive(true); 
            camera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraPosition.transform; 
            camera.GetComponent<FirstPersonCamera>().Target = playerCameraPosition.transform;
            GetComponent<StateMachine>().enabled = true; 

        
        }
        else 
        {
            camera.gameObject.SetActive(false); 
        }
        
    }

}