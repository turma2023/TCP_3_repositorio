using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionButton : UIHover
{
    [field: SerializeField] public int CharacterIndex { get; private set; }

    private Button button;
    private UISelectionManager UISelectionManager;
    public event Action<PlayerSelectionButton> OnPlayerSelected;
    void Start()
    {
        button = GetComponent<Button>();
        UISelectionManager = GetComponentInParent<UISelectionManager>();
        UISelectionManager.OnSelectionCanceled += OnSelectionCanceled;
    }

    public void DisableSelection()
    {
        hasClicked = true;
        //highlight.gameObject.SetActive(true);
        //highlight.color = Color.gray;
        //button.enabled = false;
        //highlight.fillCenter = true;
    }

    public void EnableSelection()
    {
        hasClicked = false;
        button.enabled = true;
        //highlight.fillCenter = false;
    }
    public void OnClick()
    {
        highlight.gameObject.SetActive(false);
        //highlight.fillCenter = true;
        //highlight.color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 0.85f);
        hasClicked = true;
        OnPlayerSelected?.Invoke(this);
        button.enabled = false;
    }

    private void OnSelectionCanceled()
    {
        EnableSelection();
        highlight.gameObject.SetActive(false);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        UISelectionManager.OnSelectionCanceled -= OnSelectionCanceled;
    }
}
