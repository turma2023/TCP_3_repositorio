using System;
using UnityEngine;

public class BombHandler : MonoBehaviour
{
    [SerializeField] private Transform referencePoint;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject bombPrefab;
    [SerializeField] private float timeToPlant;
    [SerializeField] private float timeToDefuse;
    public bool CanPlant { get; private set; }
    public bool CanDefuse { get; private set; }

    public event Action OnBombPlant;
    public event Action OnBombDefuse;

    private bool hasTheBomb;
    private float elapsedTime;
    private TeamSide team;



    // Start is called before the first frame update
    void Start()
    {

    }

    public void TryDefuseBomb()
    {
        if (CanPlant && hasTheBomb)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeToDefuse)
            {
                DefuseBomb();
            }

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

    public void AddBombToInventory()
    {
        hasTheBomb = true;
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

            Instantiate(bombPrefab, hit.point, Quaternion.identity);
            Debug.Log("Bomb Planted");

            hasTheBomb = false;
            OnBombPlant?.Invoke();
        }
    }

    private void TrySetAttacker(Collider other)
    {
        if (team != TeamSide.Attacker) return;

        if (other.CompareTag("area_de_pnatar_bomba") && hasTheBomb)
        {
            CanPlant = true;
        }
    }

    private void TrySetDefender(Collider other)
    {
        CanDefuse = true;
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
        TrySetDefender(other);
    }
    public void OnTriggerExit(Collider other)
    {
        if (team != TeamSide.Attacker) return;

        if (other.CompareTag("area_de_pnatar_bomba"))
        {
            CanPlant = false;
            UnityEngine.Debug.Log("saiu  na area de pantar bomba ");
        }
    }
}

public enum TeamSide
{
    Attacker,
    Defender,
}



