using Fusion;
using UnityEngine;

public class WinConditionsManager : NetworkBehaviour
{
    private MatchManager matchManager;
    private BombHandler bombHandler;
    private void Start()
    {
        var players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);
        foreach (var player in players)
        {
            if (player.HasInputAuthority)
            {
                bombHandler = player.GetComponent<BombHandler>();
                Debug.LogError("Bomb Handler found Sucessfuly");
            }
        }

        if (bombHandler == null)
        {
            Debug.LogError("Bomb Handler not found");
            return;
        }

        matchManager = FindAnyObjectByType<MatchManager>();
        //if (matchManager != null) matchManager.RPC_TestComunication();

        //else Debug.LogError("Match Manager Not Found!");

        SetBombHandlerConfig();
    }

    private void SetBombHandlerConfig()
    {
        if (bombHandler != null)
        {
            bombHandler.OnBombPlant += NotifyBombPlanted;
            bombHandler.OnBombDefuse += NotifyBombDefused;
        }
    }

    private void NotifyBombPlanted()
    {
        if (matchManager != null) matchManager.RPC_InvokeBombPlantEvents();
        else Debug.LogError("Match Manager Null On Bomb Planted!!");
    }

    private void NotifyBombDefused()
    {
        if (matchManager != null) matchManager.RPC_InvokeBombDefuseEvents();
        else Debug.LogError("Match Manager Null On Bomb Defused");
    }


    private void OnDisable()
    {
        bombHandler.OnBombPlant -= NotifyBombPlanted;
        bombHandler.OnBombDefuse -= NotifyBombDefused;
    }


}
