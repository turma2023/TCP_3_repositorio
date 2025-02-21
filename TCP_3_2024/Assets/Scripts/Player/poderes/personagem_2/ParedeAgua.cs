using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParedeAgua : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update() {
        Vector3 scale = transform.localScale;
        
        if (scale.y < 6)
        {
            scale.y += 0.1f;
        }

        if (scale.x > -16)
        {
            scale.x -= 0.5f;
        }

        transform.localScale = scale;

        // Ajusta a posição do objeto para manter a base no mesmo lugar
        Vector3 position = transform.position;
        position.y = scale.y / 2;
        transform.position = position;
    }
      

}
