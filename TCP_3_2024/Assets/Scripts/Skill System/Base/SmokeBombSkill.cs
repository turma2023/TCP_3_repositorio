using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Skills/Smobe Bomb")]
public class SmokeBombSkill : ScriptableObject, IBaseSkill
{
    [SerializeField] private GameObject smokeBombPrefab;
    //[SerializeField] private Transform SpawnPoint;
    [SerializeField] public float throwForce;
    [SerializeField] public Vector3 spawnPosition;
    private Transform spawnPoint;
    private GameObject smokeBombInstance;
    private SmokeBomb smokeBomb;
    public bool CanUse { get; private set; }
    public bool Preparing { get; private set; }
    public void Initialize(Transform parent)
    {
        smokeBombInstance = Instantiate(smokeBombPrefab);
        //smokeBombInstance.transform.localPosition = Vector3.zero;
        //smokeBombInstance.transform.rotation = parent.transform.rotation;
        smokeBombInstance.transform.rotation = Quaternion.identity;
        smokeBombInstance.transform.SetParent(parent);
        smokeBombInstance.transform.localPosition = Vector3.zero;
        //Debug.Log(parent.gameObject.name);
        smokeBomb = smokeBombInstance.GetComponent<SmokeBomb>();
        Preparing = false;
        Debug.Log("Initialized");
        //smokeBombInstance.SetActive(false);
    }
    public void Anticipate()
    {
        smokeBombInstance.SetActive(true);
        Preparing = true;
        Debug.Log("Anticipate");
        // Programar animação de antecipação aqui.
    }
    public void Execute()
    {
        Anticipate();
        smokeBomb.Throw(throwForce);
        Debug.Log("Execute");
        // Programar animação de lançamento aqui.
    }
    public void Cancel()
    {
        smokeBombInstance.SetActive(false);
        Preparing = false;
        smokeBomb.HideTrajectory();
        Debug.Log("Cancel");
    }
    public void Finish()
    {
        smokeBombInstance = null;
    }

    public void ShowTrajectory(Transform startPosition)
    {
        smokeBomb.ShowTrajectory(smokeBombInstance.transform.position, (startPosition.forward + startPosition.up).normalized * throwForce);
    }
    public void HideTrajectory()
    {
        smokeBomb.HideTrajectory();
    }
    public void Enable()
    {
        CanUse = true;
    }

    public void Disable()
    {
        CanUse = false;
    }
    
}
