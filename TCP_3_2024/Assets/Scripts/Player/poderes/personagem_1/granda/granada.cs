using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class granada : MonoBehaviour
{
    public float delay = 3f; // Tempo até a explosão
    public GameObject explosionEffect; // Prefab do efeito de explosão

    void Start()
    {
        Invoke("Explode", delay); // Explode após o tempo definido
    }

    void Explode()
    {
        // Instancia o efeito de explosão (prefab)
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject); // Destroi a granada após a explosão
    }
}