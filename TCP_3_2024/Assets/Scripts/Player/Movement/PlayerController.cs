using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour, IPlayerLeft
{

    // !cor de cada lado 
    public Material blueMaterial;
    public Material redMaterial;
    private Renderer playerRenderer;
    public GameObject PlayerModel;
    [Networked] public string Team { get; set; }

    [SerializeField] private GameObject TeamUI;

    public PlayerController Local {  get; private set; }

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
    public BombHandler BombHandler { get; private set; }

    [Networked] private Quaternion networkRotation { get; set; }
    [Networked] private Quaternion networkPivotGun { get; set; }
    [Networked] private Vector3 networkPosition { get; set; }

    // Skills
    [SerializeField] private SmokeBombSkill smokeBombSkill;

    private void Awake()
    {
        PlayerInputController = new PlayerInputController(GetComponent<PlayerInput>());
        PlayerMovement = GetComponent<PlayerMovement>();
        CurrentHealth = MaxHealth;
        BombHandler = GetComponent<BombHandler>();

        playerRenderer = PlayerModel.GetComponent<Renderer>();
        smokeBombSkill.Initialize(transform);
    }

    private void Start()
    {
        MatchManager.Instance.OnRoundEnd += OnRoundEnd;
    }

    private void OnDisable()
    {
        MatchManager.Instance.OnRoundEnd -= OnRoundEnd;
    }

    private void OnRoundEnd()
    {
        if (MatchManager.Instance.CurrentMatchPhase == MatchPhases.MatchEnd) return;
        Invoke("ResetPlayer", 2f);
    }

    private void ResetPlayer()
    {
        CurrentHealth = MaxHealth;
        transform.position = new Vector3(UnityEngine.Random.Range(-10, 10), 1, UnityEngine.Random.Range(-10, 10));
        transform.rotation = Quaternion.identity;
        pivotGun.rotation = Quaternion.identity;
    }
    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public override void Spawned()
    {
        transform.name = $"Player_{Object.Id}";

        if(Object.HasInputAuthority)
        {
            Local = this;
            Debug.Log("Spawned Local Player");
            camera.gameObject.SetActive(true);
            // camera.GetComponent<CinemachineVirtualCamera>().Follow = playerCameraPosition.transform; 
            camera.GetComponent<FirstPersonCamera>().Target = playerCameraPosition.transform;
            GetComponent<StateMachine>().enabled = true;
            pivotGun.gameObject.SetActive(false);


            TeamUI.GetComponent<TeamSelection>().Show(gameObject);
            return;
        }

        camera.gameObject.SetActive(false);
        Debug.Log("Spawned Remote Player");

    }

    public void PlayerLeft(PlayerRef player)
    {
        if (player == Object.InputAuthority)
        {
            Runner.Despawn(Object);
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

        //Debug.Log("Player died");
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
        ApplyTeamColor();
    }
    public string GetTeam()
    {
        return Team;
    }
    void ApplyTeamColor()
    {
        if (Team == "Blue")
        {
            playerRenderer.material = blueMaterial;
        }
        else if (Team == "Red")
        {
            playerRenderer.material = redMaterial;
        }
    }


    private void Update()
    {
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

            Team = Team;
            ApplyTeamColor();
        }

    }



    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendRotationToHost(Quaternion playerRotation, Quaternion gunRotation, Vector3 playerPosition, string Team)
    {
        networkRotation = playerRotation;
        networkPivotGun = gunRotation;
        networkPosition = playerPosition;
        this.Team = Team;
        ApplyTeamColor();
    }

}