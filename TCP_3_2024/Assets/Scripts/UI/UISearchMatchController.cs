using TMPro;
using UnityEngine;

public class UISearchMatchController : MonoBehaviour
{
    [SerializeField] private GameObject cancelButton;
    [SerializeField] private TextMeshProUGUI timerText;
    private bool initialized;
    private float elapsedTime;
    private int secconds;
    private int minutes;
    private string initialText;


    private void Start()
    {
        initialText = timerText.text;
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
        cancelButton.SetActive(true);
        initialized = true;
    }

    public void CancelSearch()
    {
        cancelButton.SetActive(false);
        initialized = false;
        timerText.text = initialText;
        elapsedTime = 0f;
    }

    private void OnDisable()
    {
        timerText.text = initialText;
    }
}
