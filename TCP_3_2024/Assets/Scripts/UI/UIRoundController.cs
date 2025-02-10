using UnityEngine;
using TMPro;
public class UIRoundController : MonoBehaviour
{
    [Header("Timer Panel References")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Phase Panel References")]
    [SerializeField] private TextMeshProUGUI phaseText;

    [Header("Round Panel References")]
    [SerializeField] private TextMeshProUGUI ASideText;
    [SerializeField] private TextMeshProUGUI BSideText;

    //private MatchManager matchManager;
    private bool initialized;

    void Start()
    {
        DisableUI();
        //AssignMatchManager();
        //Initialize();
    }

    private void AssignMatchManager()
    {
        //if (matchManager == null)
        //{
        //    matchManager = FindObjectOfType<MatchManager>();
        //}
    }

    private void Initialize()
    {
        if (initialized) return;

        //matchManager.OnPhaseChanged += UpdatePhasePanel;
        //matchManager.OnRoundEnd += UpdateRoundPanel;
        //matchManager.OnMatchStart += EnableUI;
        EnableUI();
        UpdateRoundPanel();
        initialized = true;
    }

    void Update()
    {
        AssignMatchManager();
        Initialize();
        UpdateTimerPanel();
    }

    public void DisableUI()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }

    public void EnableUI()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
    private void UpdateTimerPanel()
    {
        //if (matchManager.BuyPhaseTime > 0)
        //{
        //    timerText.text = Mathf.CeilToInt((float)matchManager.BuyPhaseTime).ToString("F0");
        //    return;
        //}

        //switch (matchManager.RoundTime)
        //{

        //    case > 3:
        //        {
        //            timerText.text = Mathf.CeilToInt((float)matchManager.RoundTime).ToString("F0");
        //            break;
        //        }

        //    case float n when n > 0 && n < 3:
        //        {
        //            float roundTime = (float)matchManager.RoundTime;
        //            timerText.text = roundTime.ToString("F1");
        //            break;
        //        }

        //    case float n when n <= 0:
        //        {
        //            timerText.text = Mathf.CeilToInt((float)matchManager.RoundTime).ToString("F0");
        //            break;
        //        }
        //}
    }

    private void UpdatePhasePanel(MatchPhases currentPhase)
    {
        switch (currentPhase)
        {
            case MatchPhases.BuyPhase:
                {
                    phaseText.text = "Buy Phase";
                    break;
                }

            case MatchPhases.RoundPhase:
                {
                    phaseText.text = "Round Phase";
                    break;
                }

            case MatchPhases.EndPhase:
                {
                    phaseText.text = "End Phase";
                    break;
                }

            case MatchPhases.MatchEnd:
                {
                    phaseText.text = "Match Ended";
                    break;
                }
        }
    }

    private void UpdateRoundPanel()
    {
        //ASideText.text = matchManager.TeamARoundsWon.ToString();
        //BSideText.text = matchManager.TeamBRoundsWon.ToString();
    }

    private void OnDisable()
    {
        //matchManager.OnPhaseChanged -= UpdatePhasePanel;
        //matchManager.OnRoundEnd -= UpdateRoundPanel;
        //matchManager.OnMatchStart -= EnableUI;
    }
}
