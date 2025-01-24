using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlador_da_bomba : MonoBehaviour
{
    public float tempoParaExplodir;
    public float tempoRestante;

    public float tempo_Para_desarmar;
    public float tempo_desarmado;
    public Collider colider;
    public bool jogodaro_no_ponto = false;
    public bool bomba_ativa =true;
 
    void Start()
    {
        tempoRestante = tempoParaExplodir;
        colider = GetComponent<Collider>();

    }

    void Update()
    {


        if(bomba_ativa && jogodaro_no_ponto && (Input.GetKey(KeyCode.Y)))
        {
            tempo_desarmado += Time.deltaTime;
            Debug.Log($"Desarmando... {tempo_desarmado:F2}s");

            if(tempo_desarmado >= tempo_Para_desarmar) 
            {
                desarmar();
            }
        }
        else if (jogodaro_no_ponto) 
        {
            tempo_desarmado = 0f;
        }


        if (tempoRestante > 0 && bomba_ativa)
        {
            tempoRestante -= Time.deltaTime; 
        }

        if (tempoRestante <= 0)
        {
            Explodir();
        }
    }

    public void OnTriggerEnter(Collider other)
    {


        if (other.CompareTag("atacante")) 
        {
            jogodaro_no_ponto=true;


            Debug.Log("jogador esta perto da bomba");
        }
    }

    public void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("atacante")) 
        {
            jogodaro_no_ponto=false;
            tempo_desarmado =0f;
            Debug.Log("Jogador saiu do trigger!");

        }
    }



    public void Explodir()
    {
        Debug.Log("A bomba explodiu!");
       
    }

    public void desarmar()
    {
        Debug.Log("Bomba desarmada!");
        Debug.LogError("Bomba desarmada!");
        colider.enabled = false;
        bomba_ativa =false;
    }
}
