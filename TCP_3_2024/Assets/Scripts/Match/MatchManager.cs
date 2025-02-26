using System;
using Fusion;
using UnityEngine;

public class MatchManager : NetworkBehaviour
{
    [field: SerializeField] public float RoundTime { get; private set; }
    private float originalRoundTime;
    [field: SerializeField] public float BuyPhaseTime { get; private set; }
    private float originalBuyPhaseTime;
    [field: SerializeField] public int MaxNumberOfRounds { get; private set; }
    [Networked] public MatchPhases CurrentMatchPhase { get; private set; }
    public bool IsInitialized { get; private set; }
    [Networked] public int TeamARoundsWon { get; private set; }
    [Networked] public int TeamBRoundsWon { get; private set; }
    public int TotalRoundsCount => TeamARoundsWon + TeamBRoundsWon;

    public event Action OnBuyPhaseStart;
    public event Action OnBuyPhaseEnd;

    public event Action OnRoundStart;
    public event Action OnRoundEnd;

    public event Action OnMatchStart;
    public event Action OnMatchEnd;

    public event Action OnBombPlant;
    public event Action OnBombDefuse;

    public event Action<MatchPhases> OnPhaseChanged;

    private ServerTimer serverTimer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        originalBuyPhaseTime = BuyPhaseTime;
        originalRoundTime = RoundTime;

    }
    void Start()
    {
    }

    void Update()
    {
        Initialize();
        if (!IsInitialized) return;
        TryInvokeBuyPhaseEvents();
        TryInvokeRoundEvents();
        TryInvokeBombEvents();
        UpdateTimers();
    }

    public void Initialize()
    {
        if (IsInitialized) return;

        ResetImportantValues();

        serverTimer = FindObjectOfType<ServerTimer>();

        if (serverTimer == null) return;

        RPC_SetCurrentPhase(MatchPhases.BuyPhase);
        serverTimer.OnTimerExpired += OnServerTimerEnd;
        OnRoundEnd += OnRoundEnded;

        IsInitialized = true;
        OnMatchStart?.Invoke();
    }

    public void ResetImportantValues()
    {
        RoundTime = originalRoundTime;
        BuyPhaseTime = originalBuyPhaseTime;
    }

    public void UpdateRoundsWon(TeamSide team)
    {
        if (!Runner.IsServer) return;

        switch (team)
        {
            case TeamSide.Attacker:
                {
                    TeamARoundsWon++;
                    break;
                }

            case TeamSide.Defender:
                {
                    TeamBRoundsWon++;
                    break;
                }
        }

    }

    public void SetBuyPhaseTime(float time)
    {
        originalBuyPhaseTime = time;
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RPC_SetCurrentPhase(MatchPhases phase)
    {
        CurrentMatchPhase = phase;
        RPC_InvokeOnPhaseChanged(phase);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InvokeOnPhaseChanged(MatchPhases phase)
    {
        OnPhaseChanged?.Invoke(phase);

        if (phase == MatchPhases.RoundPhase)
        {
            BuyPhaseTime = 0f;
            RoundTime = originalRoundTime;
            serverTimer.RPC_StartTimer(originalRoundTime);
            OnBuyPhaseEnd?.Invoke();
            OnRoundStart?.Invoke();
        }

        else if (phase == MatchPhases.BuyPhase)
        {
            RoundTime = 0f;
            BuyPhaseTime = originalBuyPhaseTime;
            serverTimer.RPC_StartTimer(originalBuyPhaseTime);
            OnBuyPhaseStart?.Invoke();
        }

        else if (phase == MatchPhases.BombPlanted)
        {
            serverTimer.RPC_StartTimer(60f);
            OnBombPlant?.Invoke();
        }

        else if (phase == MatchPhases.BombDefused)
        {
            OnBombDefuse?.Invoke();
            RPC_SetCurrentPhase(MatchPhases.EndPhase);
            return;
        }

        else if (phase == MatchPhases.BombExploded)
        {
            RPC_SetCurrentPhase(MatchPhases.EndPhase);
            return;
        }

        else if (phase == MatchPhases.EndPhase)
        {
            RoundTime = 0f;
            OnRoundEnd?.Invoke();

            if (Object.HasStateAuthority)
            {
                TryEndMatch();
            }

            if (CurrentMatchPhase == MatchPhases.MatchEnd) return;

            serverTimer.RPC_StartTimer(2f);
            RoundTime = 2f;
            Invoke("ResetRound", 2f);
        }

        else if (phase == MatchPhases.MatchEnd)
        {
            OnMatchEnd?.Invoke();
        }
    }

    private void ResetRound()
    {
        ResetImportantValues();
        RPC_SetCurrentPhase(MatchPhases.BuyPhase);
    }
    private void UpdateTimers()
    {
        float remainingTime = serverTimer.RemainingTime() == null ? 0f : (float)serverTimer.RemainingTime();

        if (BuyPhaseTime > 0f)
        {
            BuyPhaseTime = remainingTime;
            return;
        }

        if (RoundTime > 0f) RoundTime = remainingTime;

    }

    private void TryInvokeBuyPhaseEvents()
    {
        if (CurrentMatchPhase != MatchPhases.BuyPhase) return;

        if (BuyPhaseTime <= 0f && !serverTimer.IsActive())
        {
            BuyPhaseTime -= Time.deltaTime;
            if (Object.HasStateAuthority) RPC_SetCurrentPhase(MatchPhases.RoundPhase);
        }
    }

    private void TryInvokeRoundEvents()
    {
        if (CurrentMatchPhase != MatchPhases.RoundPhase) return;

        if (RoundTime <= 0f && !serverTimer.IsActive())
        {
            if (Object.HasStateAuthority)
            {
                UpdateRoundsWon(TeamSide.Defender);
                RPC_SetCurrentPhase(MatchPhases.EndPhase);
                OnRoundEnd?.Invoke();
            }

            RoundTime -= Time.deltaTime;
        }
    }

    private void TryInvokeBombEvents()
    {
        if (CurrentMatchPhase != MatchPhases.BombPlanted) return;

        if (RoundTime <= 0f && !serverTimer.IsActive())
        {
            if (Object.HasStateAuthority)
            {
                UpdateRoundsWon(TeamSide.Attacker);
                RPC_SetCurrentPhase(MatchPhases.BombExploded);
            }

            RoundTime -= Time.deltaTime;
        }
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InvokeBombPlantEvents()
    {
        RPC_SetCurrentPhase(MatchPhases.BombPlanted);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InvokeBombDefuseEvents()
    {
        if (Object.HasStateAuthority)
        {
            OnRoundEnd?.Invoke();
            UpdateRoundsWon(TeamSide.Defender);
        }

        RPC_SetCurrentPhase(MatchPhases.EndPhase);
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_InvokeTeamDeathEvents(TeamSide winingTeam)
    {
        if (Object.HasStateAuthority)
        {
            OnRoundEnd?.Invoke();
            UpdateRoundsWon(winingTeam);
        }

        RPC_SetCurrentPhase(MatchPhases.EndPhase);
    }

    private void TryEndMatch()
    {
        if (CurrentMatchPhase == MatchPhases.EndPhase)
        {
            if(TeamARoundsWon >= 5 ||  TeamARoundsWon >= 5) RPC_SetCurrentPhase(MatchPhases.MatchEnd);
        }
    }
    private void OnRoundEnded()
    {

    }

    private void OnServerTimerEnd()
    {

    }

    private void OnDisable()
    {
        serverTimer.OnTimerExpired -= OnServerTimerEnd;
        OnRoundEnd -= OnRoundEnded;
    }
}



public enum MatchPhases
{
    None,
    BuyPhase,
    RoundPhase,
    EndPhase,
    MatchEnd,
    BombPlanted,
    BombDefused,
    BombExploded,
}
