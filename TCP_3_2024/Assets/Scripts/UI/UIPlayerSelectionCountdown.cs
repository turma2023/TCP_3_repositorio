using Fusion;
using TMPro;
using UnityEngine;

public class UIPlayerSelectionCountdown : MonoBehaviour
{
    [SerializeField] private float selectionDuration;
    private TextMeshProUGUI timerText;
    private ServerTimer serverTimer;
    private UISelectionManager UISelectionManager;
    private bool initialized;

    private void Start()
    {
        timerText = GetComponentInChildren<TextMeshProUGUI>();

        UISelectionManager = FindObjectOfType<UISelectionManager>();
        if (!serverTimer) serverTimer = FindObjectOfType<ServerTimer>();

        //serverTimer.StartTimer(selectionDuration);
        //serverTimer.OnTimerExpired += OnTimerEnd;
    }

    private void Initialize()
    {
        if (initialized) return;
        if (serverTimer == null) return;
        serverTimer.StartTimer(selectionDuration);
        serverTimer.OnTimerExpired += OnTimerEnd;
        initialized = true;
    }

    private void FindServerTimer()
    {
        if (serverTimer == null) serverTimer = FindAnyObjectByType<ServerTimer>();
    }
    private void Update()
    {
        FindServerTimer();
        Initialize();
        if (serverTimer == null || !serverTimer.IsActive())
        {
            Debug.Log("Server Timer is null");
            return;
        }

        float? remainingTime = serverTimer.RemainingTime();

        int minutes = Mathf.FloorToInt((float)remainingTime / 60f);
        int seconds = Mathf.FloorToInt((float)remainingTime % 60f);

        timerText.text = $"{minutes}:{seconds:D2}";
    }

    private void OnTimerEnd()
    {
        if (!UISelectionManager.HasSelected)
        {
            UISelectionManager.SetDefaultCharacter();
        }

        Servidor2 servidor = FindObjectOfType<Servidor2>();
        servidor.LoadNextScene(FindObjectOfType<NetworkRunner>(), "TerrainTest");
    }

    private void OnDisable()
    {
        serverTimer.OnTimerExpired -= OnTimerEnd;
    }
}
