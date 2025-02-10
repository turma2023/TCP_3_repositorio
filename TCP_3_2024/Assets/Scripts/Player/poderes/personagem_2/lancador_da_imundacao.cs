using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lancador_da_imundacao : MonoBehaviour
{
    public GameObject imundacao; // Prefab da onda de choque
    public int numero_de_usaos;


    void Update()
    {
        if(numero_de_usaos > 0)
        if (Input.GetKeyDown(KeyCode.X)) // Tecla para lançar a onda
        {
            LaunchShockwave();
                numero_de_usaos--;
        }
    }

    void LaunchShockwave()
    {
        if (imundacao != null)
        {
            imundacao.tag = gameObject.tag;
            Instantiate(imundacao, transform.position, transform.rotation);
        }
    }
}
