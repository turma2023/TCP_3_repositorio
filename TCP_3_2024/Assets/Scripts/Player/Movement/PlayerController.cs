using System.Linq;
using Cinemachine;
using Fusion;
using Unity.Mathematics;
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

    [SerializeField] private GameObject TeamUI;

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
    [Networked] private Vector3 networkPosition { get; set; }
    public BombHandler BombHandler { get; private set; }

    public BombHandler BombHandler { get; private set; }

    private int numTeam;

    // Skills
    [SerializeField] private SmokeBombSkill smokeBombSkill;

    private void Awake()
    {
        numTeam = UnityEngine.Random.Range(1, 3);

        PlayerInputController = new PlayerInputController(GetComponent<PlayerInput>());
        PlayerMovement = GetComponent<PlayerMovement>();
        CurrentHealth = MaxHealth;

        playerRenderer = PlayerModel.GetComponent<Renderer>();
        smokeBombSkill.Initialize(transform);
        BombHandler = GetComponent<BombHandler>();
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
        // Implementar lógica para encontrar um novo host // Por exemplo, selecionar um jogador aleatório ou o próximo na lista de jogadores 
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
        // ApplyTeamColor();
    }
    public string GetTeam()
    {
        return Team;
    }

    public void Start(){
        if (numTeam == 1)
        {
            gameObject.tag = "Atacante";
        }
        if (numTeam == 2)
        {
            gameObject.tag = "Defensor";
        }

    }

    // [Rpc(RpcSources.All, RpcTargets.All)]
    // [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void RPC_ApplyTeamColor()
    {
        // if (Team == "Blue")
        // {
        //     playerRenderer.material = blueMaterial;
        // }
        // else if (Team == "Red")
        // {
        //     playerRenderer.material = redMaterial;
        // }

        if (gameObject.tag == "Atacante"){
            playerRenderer.material = redMaterial;
        }
        if (gameObject.tag == "Defensor"){
            playerRenderer.material = blueMaterial;
        }
    }


    private void Update()
    {
        // if (Object.has)
        RPC_ApplyTeamColor();

        if (smokeBombSkill.Preparing)
        {
            smokeBombSkill.ShowTrajectory(transform);

            if (Input.GetKeyDown(KeyCode.Mouse0)) smokeBombSkill.Execute();
            if (Input.GetKeyDown(KeyCode.T)) smokeBombSkill.Cancel();
        }

        else
        {
            smokeBombSkill.HideTrajectory();
            if (Input.GetKeyDown(KeyCode.T)) smokeBombSkill.Anticipate();
        }

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
        base.Spawned();
        transform.rotation = Quaternion.identity;


        if (Object.HasInputAuthority)
        {
            camera.gameObject.GetComponent<Camera>().enabled = !camera.gameObject.GetComponent<Camera>().enabled;
            camera.GetComponent<FirstPersonCamera>().enabled = !camera.GetComponent<FirstPersonCamera>().enabled;

            // camera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraPosition.transform; 
            camera.GetComponent<FirstPersonCamera>().Target = playerCameraPosition.transform;
            GetComponent<StateMachine>().enabled = true;
            // pivotGun.gameObject.SetActive(false);


            TeamUI.GetComponent<TeamSelection>().Show(gameObject);

        }
        else
        {
            // camera.gameObject.SetActive(false);

        }
    }

}