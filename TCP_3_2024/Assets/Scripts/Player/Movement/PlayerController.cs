using System;
using Fusion;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] public Transform pivotGun;
    [SerializeField] private Transform playerCameraPosition;
    public int MaxHealth = 100;
    public new Camera camera;
    [Networked] private Quaternion networkRotation { get; set; }
    [Networked] private Quaternion networkPivotGun { get; set; }
    [Networked] public Vector3 networkPosition { get; set; }
    [Networked] public string Team { get; set; }
    [Networked] public bool IsDead { get; set; }
    public int CurrentHealth { get; set; }
    public PlayerMovement PlayerMovement { get; private set; }
    public PlayerInputController PlayerInputController { get; private set; }
    public BombHandler BombHandler { get; private set; }
    public TeamSide TeamSide { get; private set; }

    public event Action<TeamSide> OnDeath;

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
        // GetComponent<StateMachine>().NetworkAnimator.SetTrigger("Armature_Morrendo");
        IsDead = true;
    }

    private void Die()
    {

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
            RPC_SendRotationToHost(transform.rotation, pivotGun.rotation, transform.position, Team);
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
    private void RPC_SendRotationToHost(Quaternion playerRotation, Quaternion gunRotation, Vector3 playerPosition, string Team)
    {
        networkRotation = playerRotation;
        networkPivotGun = gunRotation;
        networkPosition = playerPosition;
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
    }

}