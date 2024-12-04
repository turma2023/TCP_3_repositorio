using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "armas", menuName = "criador_de_armas")]
public class ScriptableObject_de_armas : ScriptableObject
{
    [Header("arma")]
    public int idDaArma;
    public string nome_da_arma;
    public Texture imagem_de_arma_para_a_loja;
    public GameObject modelo_da_arma;
    [Header("dano da arma")]
    public int dano_da_arma;
    [Header("muniçao")]
    public int muniçao_do_caregador_da_arma;
    public int muniçao_guardada;
    [Header("disparos")]
    public float distancia_deo_RaycastHit;
    public float cadencia_de_disparos;
    [Header("valor_para_comprar_a_arma")]
    public int valor_de_compra;


}


