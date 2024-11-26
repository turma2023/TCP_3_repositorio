using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlador_da_loja_de_armas : MonoBehaviour
{
    public GameObject loja_de_armas;
    public GameObject hud_dojogador;
    public bool esta_na_area_da_loja = false;
    public bool esta_na_loja = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Alterna o estado da loja com a tecla F
        if (Input.GetKeyDown(KeyCode.F) && esta_na_area_da_loja)
        {
            esta_na_loja = !esta_na_loja;
            loja_de_armas.SetActive(esta_na_loja);
            hud_dojogador.SetActive(!esta_na_loja); // Atualiza o HUD do jogador
            Debug.Log(esta_na_loja ? "Loja aberta" : "Loja fechada");
        }

        // Fecha a loja automaticamente ao sair da área
        if (!esta_na_area_da_loja && esta_na_loja)
        {
            loja_de_armas.SetActive(false);
            hud_dojogador.SetActive(true); // Reativa o HUD do jogador
            esta_na_loja = false;
        }
    }

    public void abrir_loja()
    {
        esta_na_area_da_loja = true;
    }

    public void fechar_loja()
    {
        esta_na_area_da_loja = false;
    }
}
