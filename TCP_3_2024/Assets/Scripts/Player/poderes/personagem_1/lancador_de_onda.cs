using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lancador_de_onda : MonoBehaviour
{
    public GameObject shockwavePrefab; // Prefab da onda de choque

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Tecla para lançar a onda
        {
            LaunchShockwave();
        }
    }

    void LaunchShockwave()
    {
        if (shockwavePrefab != null)
        {
            Instantiate(shockwavePrefab, transform.position, transform.rotation);
        }
    }
}
