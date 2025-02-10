using Fusion;
using UnityEngine;

public class PlayerSelectionManager : NetworkBehaviour
{
    private Spawner spawner;
    private UISelectionManager UISelectionManager;

    private void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        UISelectionManager = FindObjectOfType<UISelectionManager>();
        UISelectionManager.OnPlayerSelected += SelectCharacter;
    }
    private void SelectCharacter(int characterIndex)
    {
        Rpc_SendSelectedCharacter(characterIndex, Runner.LocalPlayer);
    }

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void Rpc_SendSelectedCharacter(int selectedCharacter, PlayerRef localPlayer, RpcInfo info = default)
    {   
        Debug.Log("Sent Character of ID: " + localPlayer.PlayerId);
        spawner.SetSelectedCharacter(localPlayer, selectedCharacter);
    }

    private void OnDisable()
    {
        UISelectionManager.OnPlayerSelected -= SelectCharacter;
    }
}
