using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class colizor_daloja : MonoBehaviour
{
    public controlador_da_loja_de_armas  loja_De_Armas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("jogador")) 
        {
            loja_De_Armas.abrir_loja();    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("jogador"))
        {
            loja_De_Armas.fechar_loja();
        }
    }
}
