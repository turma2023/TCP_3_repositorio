using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.LowLevel;

public class ShortcutManager : MonoBehaviour
{
    [Header("Shortcut Inputs")]
    [SerializeField] private KeyCode exitMainMenuShortcut;
    [SerializeField] private KeyCode exitGameplayShortcut;

    [Header("Shortcut Events")]
    [SerializeField] private UnityEvent exitMainMenu;
    [SerializeField] private UnityEvent cancelExitMainMenu;

    [SerializeField] private UnityEvent exitGameplay;
    [SerializeField] private UnityEvent cancelExitGameplay;

    [Header("Main Panels")]
    [SerializeField] private GameObject exitMainMenuPannel;
    [SerializeField] private GameObject exitGameplayPannel;

    [Header("Other Panels")]
    [SerializeField] private GameObject mainMenuButtonsPannel;

    private void Update()
    {

        CheckExitMainMenuShortcut();
        CheckExitGameplayShortcut();
    }

    private void CheckExitMainMenuShortcut()
    {
        if (exitMainMenuShortcut == KeyCode.None) return;

        if (Input.GetKeyDown(exitMainMenuShortcut))
        {
            if (mainMenuButtonsPannel.activeInHierarchy)
            {
                exitMainMenu?.Invoke();
            }

            else
            {
                if (exitMainMenuPannel.activeInHierarchy)
                {
                    cancelExitMainMenu?.Invoke();
                }
            }
        }
    }

    private void CheckExitGameplayShortcut()
    {
        if (exitGameplayShortcut == KeyCode.None) return;

        if (Input.GetKeyDown(exitGameplayShortcut))
        {
            if (exitGameplayPannel.activeInHierarchy)
            {
                cancelExitGameplay?.Invoke();
            }

            else
            {
                exitGameplay?.Invoke();
            }
        }
    }
}
