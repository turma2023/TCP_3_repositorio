using Fusion;
using TMPro;
using UnityEngine;

public class UIPlayerSelectionCountdown : MonoBehaviour
{
    [SerializeField] private float selectionDuration;
    private TextMeshProUGUI timerText;
    private ServerTimer serverTimer;
    private PlayerSelectionManager playerSelectionManager;

    private void Start()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();
        serverTimer = FindObjectOfType<ServerTimer>();
        playerSelectionManager = FindObjectOfType<PlayerSelectionManager>();
        serverTimer.StartTimer(selectionDuration);
        serverTimer.OnTimerExpired += OnTimerEnd;
    }
    private void Update()
    {
        if (!serverTimer.IsActive()) return;

        float? remainingTime = serverTimer.RemainingTime();

        int minutes = Mathf.FloorToInt((float)remainingTime / 60f);
        int seconds = Mathf.FloorToInt((float)remainingTime % 60f);

        timerText.text = $"{minutes}:{seconds:D2}";
    }

    private void OnTimerEnd()
    {
        if (!playerSelectionManager.HasSelected)
        {
            playerSelectionManager.SetDefaultCharacter();
        }

        Servidor2 servidor = FindObjectOfType<Servidor2>();
        servidor.LoadNextScene(FindObjectOfType<NetworkRunner>(), "Cena1TestNewServer");
    }

    private void OnDisable()
    {
        serverTimer.OnTimerExpired -= OnTimerEnd;

    }
}
