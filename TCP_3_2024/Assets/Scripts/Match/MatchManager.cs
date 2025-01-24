using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : MonoBehaviour
{
    public static MatchManager Instance { get; private set; }
    [field: SerializeField] public float RoundTime { get; private set; }
    private float originalRoundTime;
    [field: SerializeField] public float BuyPhaseTime { get; private set; }
    private float originalBuyPhaseTime;
    [field: SerializeField] public int MaxNumberOfRounds { get; private set; }
    public MatchPhases CurrentMatchPhase { get; private set; }

    public bool IsInitialized { get; private set; }
    public int TeamARoundsWon { get; private set; }
    public int TeamBRoundsWon { get; private set; }
    public int TotalRoundsCount => TeamARoundsWon + TeamBRoundsWon;

    public event Action OnBuyPhaseStart;
    public event Action OnBuyPhaseEnd;

    public event Action OnRoundStart;
    public event Action OnRoundEnd;

    public event Action OnMatchStart;
    public event Action OnMatchEnd;

    public event Action<MatchPhases> OnPhaseChanged;


    private void Awake()
    {
        if (!Instance) Instance = this;
        DontDestroyOnLoad(gameObject);

        originalBuyPhaseTime = BuyPhaseTime;
        originalRoundTime = RoundTime;
    }
    void Start()
    {
        OnRoundEnd += OnRoundEnded;
        OnPhaseChanged += TryEndMatch;
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
        ResetImportantValues();
        IsInitialized = true;
        OnMatchStart?.Invoke();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Cena1");
    }

    public void ResetImportantValues()
    {
        RoundTime = originalRoundTime;
        BuyPhaseTime = originalBuyPhaseTime;
    }

    public void UpdateRoundsWon()
    {
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

    private void UpdateTimers()
    {

        if (BuyPhaseTime > 0)
        {
            BuyPhaseTime -= Time.deltaTime;
            return;
        }

        if (RoundTime > 0) RoundTime -= Time.deltaTime;

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

    private void TryEndMatch(MatchPhases currentMatchPhase)
    {
        if (currentMatchPhase == MatchPhases.EndPhase && TeamARoundsWon + TeamBRoundsWon >= MaxNumberOfRounds)
        {
            OnMatchEnd?.Invoke();
        }
    }
    private void OnRoundEnded()
    {
        UpdateRoundsWon();
        Invoke("ReloadScene", 2f);
    }

    private void OnDisable()
    {
        OnMatchEnd -= OnRoundEnded;
        OnPhaseChanged -= TryEndMatch;
    }
}



public enum MatchPhases
{
    BuyPhase,
    RoundPhase,
    EndPhase,
}
