using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UISearchMatchController : MonoBehaviour
{
    [SerializeField] private GameObject searchMatchPanel;
    private TextMeshProUGUI timerText;
    private bool initialized;
    private float elapsedTime;
    private int secconds;
    private int minutes;

    private void Start()
    {
        searchMatchPanel.SetActive(false);
    }
    void Update()
    {
        if (!initialized) return;
        UpdateTimer();

    }

    private void UpdateTimer()
    {
        elapsedTime += Time.deltaTime;
        minutes = Mathf.FloorToInt(elapsedTime / 60f);
        secconds = Mathf.FloorToInt(elapsedTime % 60f);

        timerText.text = $"{minutes} : {secconds:D2}";
    }
    public void Initialize()
    {
        timerText = searchMatchPanel.GetComponentInChildren<TextMeshProUGUI>();
        searchMatchPanel.SetActive(true);
        initialized = true;
    }
}
