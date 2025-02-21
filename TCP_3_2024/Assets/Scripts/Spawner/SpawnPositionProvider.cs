using System.Collections.Generic;
using UnityEngine;

public class SpawnPositionProvider : MonoBehaviour
{
    [SerializeField] private Transform[] beachSideSpawnPoitns = new Transform[5];
    [SerializeField] private Transform[] forestSideSpawnPoitns = new Transform[5];

    public static SpawnPositionProvider Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void ProvidePositions(ref List<Transform> beachSidePositionsList, ref List<Transform> forestSidePositionsList)
    {
        foreach (Transform transform in beachSideSpawnPoitns)
        {
            beachSidePositionsList.Add(transform);
        }

        foreach (Transform transform in  forestSideSpawnPoitns)
        {
            forestSidePositionsList.Add(transform);
        }
    }
}
