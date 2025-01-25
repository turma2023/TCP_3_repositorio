using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class colocador_De_bomba : MonoBehaviour
{
    public bool tem_bomba;
    public Transform ponto_de_apareciemnto;
    public LayerMask layerChao;
    public GameObject bomba;
    public bool esta_em_uma_area_de_plantar_bomba;
    public float tempo_para_plantar;
    public float tempo_corido;

  


    // Start is called before the first frame update
    void Start()
    {
        
    
    }

    // Update is called once per frame
    void Update()
    {
       


        if (esta_em_uma_area_de_plantar_bomba && tem_bomba && Input.GetKey(KeyCode.Y))
        {
            
            tempo_corido += Time.deltaTime;
            if (tempo_corido >= tempo_para_plantar)
            {
                colocar_bomba();
            }
        }

        else
        {
            tempo_corido = 0;
        }
    }

    public void colocar_bomba()
    {
        
        Ray ray = new Ray(ponto_de_apareciemnto.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerChao))
        {

            Instantiate(bomba, hit.point, Quaternion.identity);
           
            tem_bomba = false;
        }
        else
        {
           
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("area_de_pnatar_bomba"))
        {
            esta_em_uma_area_de_plantar_bomba = true;
           
        }
    }
    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("area_de_pnatar_bomba"))
        {
            esta_em_uma_area_de_plantar_bomba = false;
            UnityEngine.Debug.Log("saiu  na area de pantar bomba ");
        }
    }
}



