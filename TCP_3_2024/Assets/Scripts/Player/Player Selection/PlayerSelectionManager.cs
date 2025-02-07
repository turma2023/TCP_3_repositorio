using System;
using Fusion;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelectionManager : MonoBehaviour
{
    public bool HasSelected {  get; private set; }
    private PlayerSelectionButton[] buttonList;
    [SerializeField] private Button cancelSelectionButton;

    public event Action OnPlayerSelected;
    public event Action OnSelectionCanceled;

    private Spawner spawner;
    private NetworkRunner runner;
    private void Awake()
    {
        DisableCancelSelection();
        buttonList = GetComponentsInChildren<PlayerSelectionButton>();
        foreach (PlayerSelectionButton button in buttonList)
        {
            button.OnPlayerSelected += SelectCharacter;
        }
    }

    private void Start()
    {
        runner = FindObjectOfType<NetworkRunner>();
        spawner = runner.GetComponent<Spawner>();
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

    private void SelectCharacter(PlayerSelectionButton playerSelectionButton, NetworkObject characterPrefab)
    {
        EnableCancelSelection();
        foreach (PlayerSelectionButton button in buttonList)
        {
            if (button == playerSelectionButton)
            {
                spawner.SetSelectedCharacter(runner, characterPrefab);
                HasSelected = true;
                continue;
            }
            button.DisableSelection();
        }

        OnPlayerSelected?.Invoke();
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
