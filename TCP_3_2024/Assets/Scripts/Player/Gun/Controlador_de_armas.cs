using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controlador_de_armas : MonoBehaviour
{
    [Header("armas")]
    public ScriptableObject_de_armas armas1 = null;
    public ScriptableObject_de_armas armas2 = null;
    public ScriptableObject_de_armas armas3 = null;

    public GameObject Ponto_De_aparecimento;
    public GameObject modelo_atual;

    [Header("Raycast")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerController playerController;
    private PlayerInputController playerInputController;

    private ScriptableObject_de_armas armaAtual;

    // Start is called before the first frame update
    void Start()
    {
        playerInputController = playerController.PlayerInputController;
        TrocarArma(armas1); // Define a arma inicial como armas1
    }

    // Update is called once per frame
    void Update()
    {
        // Troca de armas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            TrocarArma(armas1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            TrocarArma(armas2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            TrocarArma(armas3);
        }

        // Raycast para disparo
        if (playerInputController.FireAction.IsPressed())
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2));
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100, layerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red);
                Debug.Log("Did Hit");
                Debug.Log(hit.point);
            }
            else
            {
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.green);
                Debug.Log("Did not Hit");
            }
        }
    }

    public void CromprarArma(ScriptableObject_de_armas arma)
    {
        if (arma == null)
        {
            Debug.LogError("Arma não pode ser comprada porque é nula.");
            return;
        }

        // Adiciona a arma ao slot
        if (armas1 == null)
        {
            armas1 = arma;
            Debug.Log("Arma adicionada ao slot 1.");
        }
        else if (armas2 == null)
        {
            armas2 = arma;
            Debug.Log("Arma adicionada ao slot 2.");
        }
        else if (armas3 == null)
        {
            armas3 = arma;
            Debug.Log("Arma adicionada ao slot 3.");
        }
        else
        {
            Debug.Log("Não há mais espaço para armas.");
        }
    }

    private void TrocarArma(ScriptableObject_de_armas novaArma)
    {
        if (armaAtual == novaArma) return; // Evita recriação da mesma arma

        armaAtual = novaArma;

        if (modelo_atual != null)
        {
            Destroy(modelo_atual); // Remove o modelo atual
        }

        if (novaArma != null)
        {
            modelo_atual = Instantiate(novaArma.modelo_da_arma, Ponto_De_aparecimento.transform.position, Quaternion.identity, Ponto_De_aparecimento.transform);
        }
    }
}