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
        foreach (PlayerSelectionButton button in buttonList)
        {
            button.gameObject.SetActive(true);
            button.EnableSelection();
        }
    }

    public void SetDefaultCharacter()
    {
        buttonList[UnityEngine.Random.Range(0, 2)].OnClick();
        Debug.Log("Auto Select Character");
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
            button.gameObject.SetActive(false);
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
