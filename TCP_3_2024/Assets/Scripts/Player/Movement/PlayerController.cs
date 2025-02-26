using System;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] public Transform pivotGun;
    [SerializeField] private Transform playerCameraPosition;
 
    [field: SerializeField] public UISkillData UISkillData {  get; private set; }
    [Networked] private Quaternion networkRotation { get; set; }
    [Networked] private Quaternion networkPivotGun { get; set; }
    [Networked] public Vector3 networkPosition { get; set; }
    [Networked] public bool IsDead { get; set; }
    public int CurrentHealth { get; set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }
    public BombHandler BombHandler { get; private set; }
    public TeamSide TeamSide { get; private set; }

    public event Action<TeamSide> OnDeath;

    public int MaxHealth = 100;
    public new Camera camera;
    private Vector3 respawnPosition;
    private MatchManager matchManager;
    private UIPlayerController UIPlayerController;
    private Skill[] skills;

    private void Awake()
    {
        PlayerInputController = new PlayerInputController(GetComponent<PlayerInput>());
        PlayerMovement = GetComponent<PlayerMovement>();
        CurrentHealth = MaxHealth;

        BombHandler = GetComponent<BombHandler>();
    }

    void Start()
    {
        TeamSide = BombHandler.Team;
        if (!Object.HasInputAuthority) return;

        UIPlayerController = FindObjectOfType<UIPlayerController>();
        UIPlayerController.Initialize(UISkillData);

        skills = GetComponents<Skill>();
        foreach (var skill in skills)
        {
            if (skill is SpawnSmoke || skill is TrilhaDaCachoeira) skill.SetImage(UIPlayerController.GetSkill1Icon());
            else if (skill is ParedeParada || skill is QuedaOculta_1) skill.SetImage(UIPlayerController.GetSkill2Icon());
            else if (skill is LancadorDeOnda || skill is lancador_de_imundacao) skill.SetImage(UIPlayerController.GetUltimateIcon());
        }


        matchManager = FindObjectOfType<MatchManager>();
        if (matchManager != null)
        {
            Debug.LogWarning("Respawn Assigned");
            matchManager.OnBuyPhaseStart += Respawn;
        }
    }

    private void SetStartPosition()
    {
        networkPosition = transform.position;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0 && !IsDead)
        {
            CurrentHealth = 0;

            if(Object.HasStateAuthority) IsDead = true;

            else RPC_SetDeath();

            OnDeath?.Invoke(TeamSide);
        }
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_SetDeath()
    {
        GetComponent<StateMachine>().AnimationController.PlayDead(true);
        IsDead = true;
    }

    private void Update()
    {

        if (Object.HasInputAuthority)
        {
            PlayerMovement.TurnToCameraDirection();
            PlayerMovement.RotateGun(ref pivotGun);
            RPC_SendRotationToHost(transform.rotation, pivotGun.rotation, transform.position);
        }
        else
        {
            if(!IsDead)
            {
                transform.rotation = networkRotation;
                pivotGun.rotation = networkPivotGun;
                transform.position = networkPosition;
            }
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendRotationToHost(Quaternion playerRotation, Quaternion gunRotation, Vector3 playerPosition)
    {
        networkRotation = playerRotation;
        networkPivotGun = gunRotation;
        networkPosition = playerPosition;
    }

    public void Respawn()
    {
        CurrentHealth = MaxHealth;
        foreach (Skill skill in skills)
        {
            skill.EnableUse();
        }

        if (matchManager == null)
        {
            Debug.LogWarning("Match Manager is null on player respawn");
            return;
        }

        if (Object.HasStateAuthority)
        {
            IsDead = false;
            transform.position = respawnPosition;
            networkPosition = transform.position;
        }

        else
        {
            transform.position = respawnPosition;
        }
    }

    public override void Spawned()
    {
        transform.rotation = Quaternion.identity;
        if (Object.HasStateAuthority) SetStartPosition();

        if (Object.HasInputAuthority)
        {
            camera.gameObject.GetComponent<Camera>().enabled = !camera.gameObject.GetComponent<Camera>().enabled;
            camera.GetComponent<FirstPersonCamera>().enabled = !camera.GetComponent<FirstPersonCamera>().enabled;

            camera.GetComponent<FirstPersonCamera>().Target = playerCameraPosition.transform;
        }

        transform.position = networkPosition;
        respawnPosition = transform.position;
    }

    private void OnDisable()
    {
        matchManager.OnBuyPhaseStart -= Respawn;
    }

}