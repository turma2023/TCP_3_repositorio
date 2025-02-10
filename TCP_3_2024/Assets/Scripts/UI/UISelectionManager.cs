using UnityEngine;
using System;
using UnityEngine.UI;

public class UISelectionManager : MonoBehaviour
{
    public bool HasSelected { get; private set; }
    private PlayerSelectionButton[] buttonList;
    [SerializeField] private Button cancelSelectionButton;

    public event Action<int> OnPlayerSelected;
    public event Action OnSelectionCanceled;

    private void Awake()
    {
        DisableCancelSelection();
        buttonList = GetComponentsInChildren<PlayerSelectionButton>();
        foreach (PlayerSelectionButton button in buttonList)
        {
            button.OnPlayerSelected += SelectCharacter;
        }
    }


    public void CancelSelection()
    {
        HasSelected = false;
        OnSelectionCanceled?.Invoke();
        DisableCancelSelection();
    }

    public void SetDefaultCharacter()
    {
        //spawner.SetDefaultCharacter(runner);
        buttonList[1].OnClick();
    }

    private void SelectCharacter(PlayerSelectionButton playerSelectionButton)
    {
        EnableCancelSelection();
        foreach (PlayerSelectionButton button in buttonList)
        {
            if (button == playerSelectionButton)
            {
                HasSelected = true;
                OnPlayerSelected?.Invoke(button.CharacterIndex);
                continue;
            }
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
            button.OnPlayerSelected -= SelectCharacter;
        }
    }
}
