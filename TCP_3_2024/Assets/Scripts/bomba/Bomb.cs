using UnityEngine;
using Fusion;
public class Bomb : NetworkBehaviour
{
    [Networked] public bool IsActive { get; private set; }
    [Networked] public bool WasPlanted { get; private set; }
    [Networked] public bool IsDefusing { get; private set; }
    [Networked] public Vector3 NetworkedPosition { get; private set; }

    [SerializeField] private Collider normalCollider;
    [SerializeField] private Collider contactAreaCollider;

    private MeshRenderer meshRenderer;

    public override void Spawned()
    {
        if (Object.HasStateAuthority)
        {
            IsActive = true;
            NetworkedPosition = transform.position;
        }

        transform.position = NetworkedPosition;
    }
    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        contactAreaCollider.isTrigger = true;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        other.TryGetComponent(out BombHandler bombHandler);
        if (bombHandler == null)
        {
            Debug.LogWarning($"Bomb Hanlder is null on {gameObject.name}");
            return;
        }

        if (bombHandler.Team == TeamSide.Attacker)
        {
            if (bombHandler != null && IsActive && !WasPlanted)
            {
                bombHandler.AddBombToInventory(this);
                RPC_DisableRendering();
            }
        }

        else if (bombHandler.Team == TeamSide.Defender)
        {
            if (bombHandler != null && IsActive && WasPlanted && !IsDefusing)
            {
                bombHandler.SetDefender(true, this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
        other.TryGetComponent(out BombHandler bombHandler);

        if (bombHandler == null)
        {
            Debug.LogWarning($"Bomb Hanlder is null on {gameObject.name}");
            return;
        }
        if (bombHandler.Team == TeamSide.Defender)
        {
            if (bombHandler != null && IsActive && WasPlanted && !IsDefusing)
            {
                bombHandler.SetDefender(false, null);
            }
        }
    }

    private void Update()
    {
        if (Object.HasStateAuthority && IsActive)
        {
            if (WasPlanted)
            {
                normalCollider.enabled = IsActive;
                meshRenderer.enabled = IsActive;
                contactAreaCollider.enabled = IsActive;
                NetworkedPosition = transform.position;
                return;
            }

            NetworkedPosition = transform.position;
        }

        else
        {
            transform.position = NetworkedPosition;
        }

        normalCollider.enabled = IsActive;
        meshRenderer.enabled = IsActive;
        contactAreaCollider.enabled = IsActive;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DisableRendering()
    {
        if (Object.HasStateAuthority)
        {
            IsActive = false;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_EnableRendering()
    {
        if (Object.HasStateAuthority)
        {
            IsActive = true;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetBombPosition(Vector3 position)
    {
        NetworkedPosition = position;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_DisablePlant()
    {
        WasPlanted = true;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_EnablePlant()
    {
        WasPlanted = false;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_UpdateDefusing(bool value)
    {
        IsDefusing = value;
    }

}
