using System;
using Fusion;
using UnityEngine;

public class MatchManager : NetworkBehaviour
{
    [field: SerializeField] public float? RoundTime { get; private set; }
    private float? originalRoundTime;
    [field: SerializeField] public float? BuyPhaseTime { get; private set; }
    private float? originalBuyPhaseTime;
    [field: SerializeField] public int MaxNumberOfRounds { get; private set; }
    public MatchPhases CurrentMatchPhase { get; private set; }

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

    public event Action<MatchPhases> OnPhaseChanged;

    private ServerTimer serverTimer;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        originalBuyPhaseTime = BuyPhaseTime;
        originalRoundTime = RoundTime;

        serverTimer = FindObjectOfType<ServerTimer>();
        serverTimer.OnTimerExpired += OnServerTimerEnd;
        OnRoundEnd += OnRoundEnded;

        Initialize();
    }
    void Start()
    {
        
    }

    void Update()
    {
        if (!IsInitialized) return;
        TryInvokeBuyPhaseEvents();
        TryInvokeRoundEvents();
        UpdateTimers();
    }

    public void Initialize()
    {
        if (IsInitialized) return;
        ResetImportantValues();
        IsInitialized = true;
        OnMatchStart?.Invoke();
    }

    public void ResetImportantValues()
    {
        RoundTime = originalRoundTime;
        BuyPhaseTime = originalBuyPhaseTime;
        CurrentMatchPhase = MatchPhases.BuyPhase;
    }

    public void UpdateRoundsWon()
    {
        if (!Runner.IsServer) return;

        int random = UnityEngine.Random.Range(0, 2);
        switch (random)
        {
            case 0:
                {
                    TeamARoundsWon++;
                    break;
                }

            case 1:
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

    public void SetCurrentPhase(MatchPhases phase)
    {
        CurrentMatchPhase = phase;
        OnPhaseChanged?.Invoke(phase);
    }

    private void UpdateTimers()
    {

        if (BuyPhaseTime > 0f)
        {
            BuyPhaseTime = serverTimer.RemainingTime();
            return;
        }

        if (RoundTime > 0f) RoundTime = serverTimer.RemainingTime();

    }

    private void TryInvokeBuyPhaseEvents()
    {
        if (CurrentMatchPhase != MatchPhases.BuyPhase) return;

        if (BuyPhaseTime <= 0f)
        {
            OnBuyPhaseEnd?.Invoke();
            CurrentMatchPhase = MatchPhases.RoundPhase;
            OnPhaseChanged?.Invoke(CurrentMatchPhase);
            BuyPhaseTime -= Time.deltaTime;
        }

        if (BuyPhaseTime == originalBuyPhaseTime)
        {
            CurrentMatchPhase = MatchPhases.BuyPhase;
            OnBuyPhaseStart?.Invoke();
            OnPhaseChanged?.Invoke(CurrentMatchPhase);
        }
    }

    private void TryInvokeRoundEvents()
    {
        if (CurrentMatchPhase != MatchPhases.RoundPhase) return;

        if (RoundTime == originalRoundTime)
        {
            CurrentMatchPhase = MatchPhases.RoundPhase;
            OnRoundStart?.Invoke();
            OnPhaseChanged?.Invoke(CurrentMatchPhase);
        }

        if (RoundTime <= 0f)
        {
            CurrentMatchPhase = MatchPhases.EndPhase;
            OnRoundEnd?.Invoke();
            OnPhaseChanged?.Invoke(CurrentMatchPhase);
            RoundTime -= Time.deltaTime;
        }
    }

    private void TryEndMatch()
    {
        if (CurrentMatchPhase == MatchPhases.EndPhase && TotalRoundsCount >= MaxNumberOfRounds)
        {
            CurrentMatchPhase = MatchPhases.MatchEnd;
            OnMatchEnd?.Invoke();

        }
    }
    private void OnRoundEnded()
    {
        UpdateRoundsWon();
        TryEndMatch();

        if (CurrentMatchPhase == MatchPhases.MatchEnd) return;
        Invoke("ResetImportantValues", 2f);
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
    BuyPhase,
    RoundPhase,
    EndPhase,
    MatchEnd,
}
