using UnityEngine;
using UnityEngine.UI;

public class TeamSelection : MonoBehaviour
{
    public Button blueTeamButton;
    public Button redTeamButton;
    private string selectedTeam;

    private GameObject runner;


    void Start()
    {
        blueTeamButton.onClick.AddListener(() => SelectTeam("Blue"));
        redTeamButton.onClick.AddListener(() => SelectTeam("Red"));
    }

    public void Show(GameObject runner)
    {
        // Mostrar a interface de seleção de time
        this.runner = runner;
        gameObject.SetActive(true);
    }

    void SelectTeam(string team)
    {
        selectedTeam = team;
        Debug.Log("Selected Team: " + team);
        gameObject.SetActive(false);

        //this.runner.GetComponent<PlayerController>().SetTeam(team);

    }

}
