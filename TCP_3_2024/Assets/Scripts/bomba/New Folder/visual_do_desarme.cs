using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class visual_do_desarme : MonoBehaviour
{
    public controlador_da_bomba bomba;
    public float tempo_para_desarmar;
    public float desarme;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        tempo_para_desarmar = bomba.tempo_Para_desarmar;
        desarme = bomba.tempo_desarmado;
    }
}
