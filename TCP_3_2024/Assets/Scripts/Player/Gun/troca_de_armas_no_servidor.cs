using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class troca_de_armas_no_servidor : MonoBehaviour
{

    public GameObject Ponto_De_aparecimento;
    GameObject modelo_de_arma;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void atualizar_arma(GameObject arma_atual) 
    {
        modelo_de_arma = arma_atual;
        Instantiate(modelo_de_arma, Ponto_De_aparecimento.transform.position, Quaternion.identity);


        // Define a rotação do modelo para ignorar a rotação do pai
        modelo_de_arma.transform.localRotation = Quaternion.identity;
    }
}
