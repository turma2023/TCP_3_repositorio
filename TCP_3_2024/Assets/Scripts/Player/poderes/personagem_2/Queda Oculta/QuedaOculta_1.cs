using System.Collections;
using UnityEngine;
using Fusion;

public class QuedaOculta_1 : Skill
{
    public GameObject wallPrefab; // Prefab da parede que sera spawnada
    public float spawnDistance = 2f; // Distancia � frente do jogador onde a parede ser� spawnada
    public float wallLifetime = 5f; // Tempo em segundos ate a parede desaparecer

    void Update()
    {
        // Verifica se o jogador pressionou a tecla Q e se tem autoridade de entrada
        if (Input.GetKeyDown(KeyCode.E) && Object.HasInputAuthority && !HasUsed)
        {
            RPC_SpawnWall();
            DisableUse();
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    void RPC_SpawnWall()
    {
        if (Runner.IsServer && wallPrefab != null)
        {
          
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

         
            spawnPosition = GetGroundPosition(spawnPosition);

            Quaternion wallRotation = Quaternion.Euler(0, transform.eulerAngles.y + 90, 0);

          
            NetworkObject wallNetworkObject = Runner.Spawn(wallPrefab, spawnPosition, wallRotation, Object.InputAuthority);

            
            wallNetworkObject.gameObject.tag = gameObject.tag;

          
            StartCoroutine(DestroyWall(wallNetworkObject, wallLifetime));
        }
    }

    private Vector3 GetGroundPosition(Vector3 position)
    {
     
        if (Physics.Raycast(position + Vector3.up * 5f, Vector3.down, out RaycastHit hit, 10f))
        {
            return hit.point;
        }
        return position;
    }

    private IEnumerator DestroyWall(NetworkObject wall, float lifetime)
    {

        yield return new WaitForSeconds(lifetime);

 
        if (wall != null)
        {
            Runner.Despawn(wall);
        }
    }
}
