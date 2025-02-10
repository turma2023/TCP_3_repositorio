using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class granada : MonoBehaviour
{
    public float delay = 3f; // Tempo at� a explos�o
    public GameObject explosionEffect; // Prefab do efeito de explos�o

    void Start()
    {
        Invoke("Explode", delay); // Explode ap�s o tempo definido
    }

    void Explode()
    {
        // Instancia o efeito de explos�o (prefab)
        if (explosionEffect != null)
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
        }

        Destroy(gameObject); // Destroi a granada ap�s a explos�o
    }
}