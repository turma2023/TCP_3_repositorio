using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Security.Cryptography;

public class UIRoundController : MonoBehaviour
{
    [Header("Timer Panel References")]
    [SerializeField] private TextMeshProUGUI timerText;

    [Header("Phase Panel References")]
    [SerializeField] private TextMeshProUGUI phaseText;

    [Header("Round Panel References")]
    [SerializeField] private TextMeshProUGUI ASideText;
    [SerializeField] private TextMeshProUGUI BSideText;

    private MatchManager matchManager;

    void Start()
    {
        matchManager = MatchManager.Instance;
        DisableUI();
        matchManager.OnPhaseChanged += UpdatePhasePanel;
        matchManager.OnPhaseChanged += UpdateRoundPanel;
        matchManager.OnMatchStart += EnableUI;
    }

    void Update()
    {
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
        if (matchManager.BuyPhaseTime > 0)
        {
            timerText.text = matchManager.BuyPhaseTime.ToString("F0");
            return;
        }

        switch (matchManager.RoundTime)
        {

            case > 3:
                {
                    timerText.text = matchManager.RoundTime.ToString("F0");
                    break;
                }

            case float n when n >= 0 && n < 3:
                {
                    timerText.text = matchManager.RoundTime.ToString("F1");
                    break;
                }
        }
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
        }
    }

    private void UpdateRoundPanel(MatchPhases currentPhase)
    {
        if (currentPhase != MatchPhases.EndPhase) return;

        ASideText.text = matchManager.TeamARoundsWon.ToString();
        BSideText.text = matchManager.TeamBRoundsWon.ToString();
    }

    private void OnDisable()
    {
        matchManager.OnPhaseChanged -= UpdatePhasePanel;
        matchManager.OnPhaseChanged -= UpdateRoundPanel;
        matchManager.OnMatchStart -= EnableUI;
    }
}
