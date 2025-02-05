using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    private PlayerSelectionButton[] buttonList;
    [SerializeField] private Button cancelSelectionButton;

    public event Action OnSelectionCanceled;
    private void Awake()
    {
        DisableCancelSelection();
        buttonList = GetComponentsInChildren<PlayerSelectionButton>();
        foreach (PlayerSelectionButton button in buttonList)
        {
            button.OnPlayerSelected += OnPlayerSelected;
        }
        
    }

    public void CancelSelection()
    {
        OnSelectionCanceled?.Invoke();
        DisableCancelSelection();
    }

    private void OnPlayerSelected(PlayerSelectionButton playerSelectionButton)
    {
        EnableCancelSelection();
        foreach (PlayerSelectionButton button in buttonList)
        {
            if (button == playerSelectionButton) continue;
            button.DisableSelection();
        }
    }
    
    private void EnableCancelSelection()
    {
        cancelSelectionButton?.gameObject.SetActive(true);
    }

    private void DisableCancelSelection()
    {
        cancelSelectionButton?.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        foreach (PlayerSelectionButton button in buttonList)
        {
            button.OnPlayerSelected -= OnPlayerSelected;
        }
    }
}
