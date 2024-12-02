using UnityEngine;
using UnityEngine.UI;

public class TeamSelection : MonoBehaviour
{
    public Button blueTeamButton;
    public Button redTeamButton;
    private string selectedTeam;


    void Start()
    {
        blueTeamButton.onClick.AddListener(() => SelectTeam("Blue"));
        redTeamButton.onClick.AddListener(() => SelectTeam("Red"));
    }

    public void Show()
    {
        // Mostrar a interface de seleção de time
        gameObject.SetActive(true);
    }

    void SelectTeam(string team)
    {
        selectedTeam = team;
        Debug.Log("Selected Team: " + team);
        gameObject.SetActive(false);
        FindObjectOfType<Servidor2>().SetSelectedTeam(selectedTeam);
        // FindObjectOfType<PlayerController>().SetTeam(selectedTeam);
        // FindObjectOfType<HostManager>().selectedTeam = selectedTeam;

    }

}
