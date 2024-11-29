using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Controlador_De_Loja_De_Armas : MonoBehaviour
{
    public GameObject loja_de_armas;
    public bool esta_na_loja = false;
    public bool esta_na_Area_Da_loja = false;

    void Start()
    {
        loja_de_armas.SetActive(false); // A loja começa fechada
    }

    void Update()
    {
        if (esta_na_Area_Da_loja && Input.GetKeyDown(KeyCode.B))
        {
            if (esta_na_loja)
            {
                fechar_loja(); // Se já estiver na loja, fecha
            }
            else
            {
                abri_loja(); // Se não estiver na loja, abre
            }
        }
    }

    public void abri_loja()
    {
        loja_de_armas.SetActive(true); // Ativa o objeto da loja
        esta_na_loja = true; // Marca que a loja está aberta
        Debug.Log("Entrou na loja");
    }

    public void fechar_loja()
    {
        loja_de_armas.SetActive(false); // Desativa o objeto da loja
        esta_na_loja = false; // Marca que a loja está fechada
        Debug.Log("Saiu da loja");
    }

    private void OnTriggerEnter(Collider other)
    {
        esta_na_Area_Da_loja = true; // Marca que o jogador entrou na área da loja
        Debug.LogError("Entrou na loja");
    }

    private void OnTriggerExit(Collider other)
    {
        esta_na_loja = false; // Marca que o jogador saiu da loja
        esta_na_Area_Da_loja = false; // Marca que o jogador saiu da área da loja
        loja_de_armas.SetActive(false); // Fecha a loja quando sai da área
        Debug.LogError("Saiu da loja");
    }
}
