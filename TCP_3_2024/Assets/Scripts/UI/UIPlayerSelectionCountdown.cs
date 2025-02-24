using TMPro;
using UnityEngine;
using Fusion;

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
        serverTimer.RPC_StartTimer(selectionDuration);
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
        Debug.Log("Timer Has Expired");
        if (!UISelectionManager.HasSelected)
        {
            UISelectionManager.SetDefaultCharacter();
        }

        Servidor servidor = FindObjectOfType<Servidor>();
        servidor.LoadNextScene(FindObjectOfType<NetworkRunner>(), "TerrainTest");
    }

    private void OnDisable()
    {
        serverTimer.OnTimerExpired -= OnTimerEnd;
    }
}
