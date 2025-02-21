using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{

    // !cor de cada lado 
    public Material blueMaterial;
    public Material redMaterial;
    private Renderer playerRenderer;
    public GameObject PlayerModel;
    [Networked] public string Team { get; set; }

    //[SerializeField] private GameObject TeamUI;

    //! fim cor de cada lado      

    // ! vida
    public int CurrentHealth { get; set; }
    public int MaxHealth = 100;
    // ! fim vida


    [SerializeField] public Transform pivotGun;
    public new Camera camera;
    [SerializeField] private Transform playerCameraPosition;

    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }

    [Networked] private Quaternion networkRotation { get; set; }
    [Networked] private Quaternion networkPivotGun { get; set; }
    [Networked] public Vector3 networkPosition { get; set; }
    public BombHandler BombHandler { get; private set; }

    private int numTeam;

    // Skills
    [SerializeField] private SmokeBombSkill smokeBombSkill;

    private void Awake()
    {

        PlayerInputController = new PlayerInputController(GetComponent<PlayerInput>());
        PlayerMovement = GetComponent<PlayerMovement>();
        CurrentHealth = MaxHealth;

        playerRenderer = PlayerModel.GetComponent<Renderer>();
        BombHandler = GetComponent<BombHandler>();
    }

    void Start()
    {
        //transform.position = networkPosition;
    }

    private void SetStartPosition()
    {
        networkPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    private void Die()
    {

        // Transferir a autoridade para outro jogador antes de despawnar 
        if (Object.HasStateAuthority)
        {
            PlayerRef newHost = FindNewHost();
            if (newHost != null)
            {
                NetworkObject newHostObject = Runner.GetPlayerObject(newHost);
                if (newHostObject != null)
                {
                    Runner.SetPlayerObject(newHost, newHostObject);
                }
            }
        }

        Debug.Log("Player died");
        Runner.Despawn(Object);
    }

    private PlayerRef FindNewHost()
    {
        foreach (var player in Runner.ActivePlayers)
        {
            if (player != Object.InputAuthority)
            {
                return player;
            }
        }
        return default;
    }


    public void SetTeam(string team)
    {
        Team = team;
    }
    public string GetTeam()
    {
        return Team;
    }


    private void Update()
    {


        if (Object.HasInputAuthority)
        {
            PlayerMovement.TurnToCameraDirection();
            PlayerMovement.RotateGun(ref pivotGun);
            networkRotation = transform.rotation;
            networkPivotGun = pivotGun.rotation;
            networkPosition = transform.position;
            Team = this.Team;
            RPC_SendRotationToHost(transform.rotation, pivotGun.rotation, transform.position, Team);
        }
        else
        {
            transform.rotation = networkRotation;
            pivotGun.rotation = networkPivotGun;
            transform.position = networkPosition;

            // this.Team = Team;
            // this.ApplyTeamColor();
        }

    }



    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendRotationToHost(Quaternion playerRotation, Quaternion gunRotation, Vector3 playerPosition, string Team)
    {
        networkRotation = playerRotation;
        networkPivotGun = gunRotation;
        networkPosition = playerPosition;
        // this.Team = Team;
        // ApplyTeamColor();
    }

    public override void Spawned()
    {
        //base.Spawned();
        transform.rotation = Quaternion.identity;
        if (Object.HasStateAuthority) SetStartPosition();

        if (Object.HasInputAuthority)
        {
            camera.gameObject.GetComponent<Camera>().enabled = !camera.gameObject.GetComponent<Camera>().enabled;
            camera.GetComponent<FirstPersonCamera>().enabled = !camera.GetComponent<FirstPersonCamera>().enabled;

            // camera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraPosition.transform; 
            camera.GetComponent<FirstPersonCamera>().Target = playerCameraPosition.transform;
            GetComponent<StateMachine>().enabled = true;
            // pivotGun.gameObject.SetActive(false);
            //networkPosition = transform.position;
            //TeamUI.GetComponent<TeamSelection>().Show(gameObject);

        }
        else
        {

        }

        transform.position = networkPosition;

    }

}