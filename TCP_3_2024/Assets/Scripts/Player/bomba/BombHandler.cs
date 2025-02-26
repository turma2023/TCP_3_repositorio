using System;
using UnityEngine;
using Fusion;

public class BombHandler : NetworkBehaviour
{
    [SerializeField] private Transform referencePoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float timeToPlant;
    [SerializeField] private float timeToDefuse;
    [Networked] public TeamSide Team { get; set; }
    public bool CanPlant { get; private set; }
    public bool CanDefuse { get; private set; }

    public event Action OnBombPlant;
    public event Action OnBombExplode;
    public event Action OnBombDefuse;

    private bool hasTheBomb;
    private float elapsedTime;
    private Bomb bomb;

    void Update()
    {
        TryCarryBomb();
    }

    public void TryDefuseBomb()
    {
        if (CanDefuse && !hasTheBomb)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeToDefuse)
            {
                DefuseBomb();
                CanDefuse = false;
            }

            UpdateDefusing(true);
            Debug.Log("Defusing...");
        }
    }
    public void TryPlantBomb()
    {
        if (CanPlant && hasTheBomb)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeToPlant)
            {
                PlantBomb();
            }

            Debug.Log("Planting...");
        }
    }

    public void ResetTimer()
    {
        elapsedTime = 0;
    }

    public void UpdateDefusing(bool value)
    {
        bomb.RPC_UpdateDefusing(value);
    }

    public void AddBombToInventory(Bomb bomb)
    {
        this.bomb = bomb;
        hasTheBomb = true;
    }

    private void TryCarryBomb()
    {
        if (hasTheBomb && bomb != null)
        {
            bomb.RPC_SetBombPosition(transform.position);
        }
    }

    public void DefuseBomb()
    {
        Debug.Log("Bomb Defused");
        OnBombDefuse?.Invoke();
    }
    public void PlantBomb()
    {

        Ray ray = new Ray(referencePoint.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, groundLayer))
        {
            bomb.RPC_SetBombPosition(hit.point + (transform.forward * 2));
            bomb.RPC_DisablePlant();
            bomb.RPC_EnableRendering();
            Debug.Log("Bomb Planted");
            hasTheBomb = false;
            OnBombPlant?.Invoke();
        }
    }

    private void TrySetAttacker(Collider other)
    {
        if (Team != TeamSide.Attacker) return;

        if (other.CompareTag("area_de_pnatar_bomba") && hasTheBomb)
        {
            CanPlant = true;
        }
    }

    public void SetDefender(bool value, Bomb bomb)
    {
        CanDefuse = value;
        this.bomb = bomb;
    }
    public void Enable()
    {
        enabled = true;
    }
    public void Disable()
    {
        enabled = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        TrySetAttacker(other);
    }

    public void OnTriggerExit(Collider other)
    {
        if (Team != TeamSide.Attacker) return;

        if (other.CompareTag("area_de_pnatar_bomba"))
        {
            CanPlant = false;
            UnityEngine.Debug.Log("saiu  na area de pantar bomba ");
        }
    }

    public override void Spawned()
    {

    }
}

public enum TeamSide
{
    Attacker,
    Defender,
}



