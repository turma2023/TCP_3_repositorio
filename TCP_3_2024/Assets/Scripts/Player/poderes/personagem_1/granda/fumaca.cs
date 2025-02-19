using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class fumaca : NetworkBehaviour
{
    public float lifetime = 10f; // Tempo de vida do objeto

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            StartCoroutine(DestroyAfterTime());
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(lifetime);

        if (Runner.IsServer && Object != null)
        {
            Runner.Despawn(Object);
        }
    }
}
