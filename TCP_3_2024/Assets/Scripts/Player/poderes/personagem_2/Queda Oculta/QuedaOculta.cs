using System.Collections;
using UnityEngine;

public class QuedaOculta : MonoBehaviour
{
    public GameObject fireWallPrefab;  // Prefab da parede de fogo
    public int maxWallSegments = 10;   // Número máximo de segmentos
    public float segmentSpacing = 2f;  // Distância entre cada parede
    public float wallDuration = 5f;    // Tempo que a parede dura
    public float curveIntensity = 2f;  // Intensidade da curva
    public float spawnRate = 0.1f;     // Tempo entre cada segmento gerado
    

    private bool isCasting = false;
    private Vector3 lastWallPosition;
    private Quaternion lastWallRotation;

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q)) // Pressionar para começar
        //{
        //    StartCoroutine(SpawnFireWall());
        //}
    }

    IEnumerator SpawnFireWall()
    {
        isCasting = true;
        lastWallPosition = GetGroundPosition(transform.position); // Garante que a primeira parede esteja no chão
        lastWallRotation = transform.rotation * Quaternion.Euler(0, 0, 0); // Gira a parede 90° para ficar "de lado"

        for (int i = 0; i < maxWallSegments; i++)
        {
            if (!isCasting) break;

            // Ajusta a posição do novo segmento no chão
            Vector3 spawnPosition = lastWallPosition + lastWallRotation * Vector3.forward * segmentSpacing;
            spawnPosition = GetGroundPosition(spawnPosition);

            // Instancia a parede
            GameObject newWall = Instantiate(fireWallPrefab, spawnPosition, lastWallRotation);

            // Define a nova Tag da parede instanciada
            newWall.tag = gameObject.tag; // Certifique-se de que "NovaTag" existe no projeto

            Destroy(newWall, wallDuration);

            lastWallPosition = spawnPosition;

            // Ajusta a curva baseado no input do mouse
            float mouseInput = Input.GetAxis("Mouse X");
            lastWallRotation *= Quaternion.Euler(0, mouseInput * curveIntensity, 0);

            yield return new WaitForSeconds(spawnRate);
        }

        isCasting = false;
    }

    // Garante que a parede sempre seja spawnada no chão
    Vector3 GetGroundPosition(Vector3 position)
    {
        RaycastHit hit;
        if (Physics.Raycast(position + Vector3.up * 5f, Vector3.down, out hit, 10f))
        {
            return hit.point; // Retorna a posição do chão
        }
        return position; // Caso não encontre o chão, mantém a posição original
    }
}
