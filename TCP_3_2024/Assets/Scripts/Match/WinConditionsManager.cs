using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class WinConditionsManager : NetworkBehaviour
{
    private MatchManager matchManager;
    private BombHandler bombHandler;
    private PlayerController[] players;
    private List<PlayerController> attackers;
    private List<PlayerController> defenders;
    private void Start()
    {
        attackers = new();
        defenders = new();

        players = FindObjectsByType<PlayerController>(FindObjectsSortMode.None);

        foreach (var player in players)
        {
            if (player.BombHandler.Team == TeamSide.Attacker)
            {
                attackers.Add(player);
            }

            else if (player.BombHandler.Team == TeamSide.Defender)
            {
                defenders.Add(player);
            }

            if (player.HasInputAuthority)
            {
                bombHandler = player.GetComponent<BombHandler>();
            }

            player.OnDeath += VerifyTeamDeath;
        }

        if (bombHandler == null)
        {
            return;
        }

        Debug.Log("Attackers Size = " + attackers.Count);
        Debug.Log("Defenders Size = " + defenders.Count);
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

    private void VerifyTeamDeath(TeamSide team)
    {
        if (team == TeamSide.Attacker)
        {
            foreach (var player in attackers)
            {
                if (!player.IsDead) return;
            }

            Debug.LogWarning("Attackers Dead!!!");
            NotifyTeamDeath(TeamSide.Attacker);
        }

        else if(team == TeamSide.Defender)
        {
            foreach (var player in defenders)
            {
                if (!player.IsDead) return;
            }
            Debug.LogWarning("Defenders Dead!!!");

            NotifyTeamDeath(TeamSide.Defender);
        }
        

    }
    private void NotifyTeamDeath(TeamSide team)
    {
        if (team == TeamSide.Attacker && matchManager!= null) matchManager.RPC_InvokeTeamDeathEvents(TeamSide.Defender);

        if (team == TeamSide.Defender && matchManager!= null) matchManager.RPC_InvokeTeamDeathEvents(TeamSide.Attacker);
    }

    private void OnDisable()
    {
        bombHandler.OnBombPlant -= NotifyBombPlanted;
        bombHandler.OnBombDefuse -= NotifyBombDefused;

        foreach (var player in players)
        {
            player.OnDeath -= VerifyTeamDeath;
        }
    }


}
