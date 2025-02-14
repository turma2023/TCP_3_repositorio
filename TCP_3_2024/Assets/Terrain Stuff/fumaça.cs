using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fumaça : MonoBehaviour
{
    public float lifetime = 10f; // Tempo de vida do objeto

    void Start()
    {
        Destroy(gameObject, lifetime); // Destroi o objeto após o tempo definido
    }
}