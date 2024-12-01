using System.Linq;
using Cinemachine;
using Fusion;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    
    public int CurrentHealth { get; set; } 
    public int MaxHealth = 100;

    [SerializeField] private Transform pivotGun;
    public new Camera camera;
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
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage) 
    { 
        CurrentHealth -= damage; 
        if (CurrentHealth <= 0) 
        { 
            Die(); 
        }
    } 
    private void Die() { 

        // Transferir a autoridade para outro jogador antes de despawnar 
        if (Object.HasStateAuthority) { 
            PlayerRef newHost = FindNewHost(); 
            if (newHost != null) { 
                NetworkObject newHostObject = Runner.GetPlayerObject(newHost); 
                if (newHostObject != null) { 
                    Runner.SetPlayerObject(newHost, newHostObject); 
                }
            } 
        } // Despawn o objeto de rede Runner.Despawn(Object); Debug.Log("Player died and despawned"


        // Implementar lógica de morte do jogador 
        Debug.Log("Player died");
        // Destroy(gameObject);
        Runner.Despawn(Object);
    }

    private PlayerRef FindNewHost() 
    { // Implementar lógica para encontrar um novo host // Por exemplo, selecionar um jogador aleatório ou o próximo na lista de jogadores 
        foreach (var player in Runner.ActivePlayers) { 
            if (player != Object.InputAuthority) { 
                return player; 
            } 
        } 
        return default;
    }


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
            pivotGun.rotation  = networkPivotGun;
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

    public override void Spawned()
    {
        base.Spawned();
        transform.rotation = Quaternion.identity;


        if (Object.HasInputAuthority) 
        { 
            Debug.Log("entrou aqui aaaaaaaaaaaaaaaaaaaaaa");
            camera.gameObject.SetActive(true); 
            // camera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraPosition.transform; 
            camera.GetComponent<FirstPersonCamera>().Target = playerCameraPosition.transform;
            GetComponent<StateMachine>().enabled = true; 
            pivotGun.gameObject.SetActive(false);

        
        }
        else 
        {
            camera.gameObject.SetActive(false); 
        }
        
    }

}