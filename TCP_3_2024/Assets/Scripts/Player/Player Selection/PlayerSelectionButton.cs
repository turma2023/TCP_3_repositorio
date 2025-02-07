using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionButton : UIHover
{
    [field: SerializeField] public NetworkObject playerPrefab { get; private set; }

    private Button button;
    private PlayerSelectionManager playerSelectionManager;
    public event Action<PlayerSelectionButton, NetworkObject> OnPlayerSelected;
    void Start()
    {
        button = GetComponent<Button>();
        playerSelectionManager = GetComponentInParent<PlayerSelectionManager>();
        playerSelectionManager.OnSelectionCanceled += OnSelectionCanceled;
    }

    public void DisableSelection()
    {
        hasClicked = true;
        highlight.gameObject.SetActive(true);
        highlight.color = Color.gray;
        button.enabled = false;
        highlight.fillCenter = true;
    }

    public void EnableSelection()
    {
        hasClicked = false;
        button.enabled = true;
        highlight.fillCenter = false;
    }
    public void OnClick()
    {
        highlight.gameObject.SetActive(true);
        highlight.fillCenter = true;
        highlight.color = new Color(255 / 255f, 0 / 255f, 0 / 255f, 0.85f);
        hasClicked = true;
        OnPlayerSelected?.Invoke(this, playerPrefab);
        button.enabled = false;
    }

    private void OnSelectionCanceled()
    {
        EnableSelection();
        highlight.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        playerSelectionManager.OnSelectionCanceled -= OnSelectionCanceled;
    }
}
