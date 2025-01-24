using System.Collections;
using System.Collections.Generic;
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

    public Slider slide;
    public GameObject slide_de_pante;

    // Start is called before the first frame update
    void Start()
    {
        slide_de_pante.SetActive(false);
        slide.maxValue = tempo_para_plantar;
    }

    // Update is called once per frame
    void Update()
    {
        slide.value = tempo_corido;


        if (esta_em_uma_area_de_plantar_bomba && tem_bomba && Input.GetKey(KeyCode.Y))
        {
            slide_de_pante.SetActive(true);
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
        slide_de_pante.SetActive(false);
        Ray ray = new Ray(ponto_de_apareciemnto.position, Vector3.down);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, layerChao))
        {

            Instantiate(bomba, hit.point, Quaternion.identity);
            Debug.Log("Bomba colocada!");
            tem_bomba = false;
        }
        else
        {
            Debug.Log("Você não pode colocar a bomba aqui!");
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("area_de_pnatar_bomba"))
        {
            esta_em_uma_area_de_plantar_bomba = true;
            Debug.LogWarning("entrou  na area de pantar bomba ");
        }
    }
    public void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("area_de_pnatar_bomba"))
        {
            esta_em_uma_area_de_plantar_bomba = false;
            Debug.LogWarning("saiu  na area de pantar bomba ");
        }
    }
}
    


