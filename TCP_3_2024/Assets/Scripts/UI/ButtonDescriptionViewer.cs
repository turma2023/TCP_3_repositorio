using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonDescriptionViewer : UIHover
{
    [SerializeField] private TextMeshProUGUI descriptionPanelText;
    private TextMeshProUGUI descriptionText;
    void Start()
    {
        descriptionText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        highlight.gameObject.SetActive(true);
        highlight.color = Color.blue;
        descriptionPanelText.transform.parent.gameObject.SetActive(true);
        descriptionPanelText.text = descriptionText.text;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        highlight.gameObject.SetActive(false);
        descriptionPanelText.text = string.Empty;
        descriptionPanelText.transform.parent.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        descriptionPanelText.text = string.Empty;
        descriptionPanelText.transform.parent.gameObject.SetActive(false);
    }
}
