using UnityEngine;

public class Onda : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;
    public float speed = 1f;

    private Mesh mesh;
    private Vector3[] originalVertices;
    private Vector3[] displacedVertices;

    void Start()
    {
        // Certifique-se de que o MeshFilter está presente
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gameObject.AddComponent<MeshFilter>();
        }
        mesh = meshFilter.mesh;

        originalVertices = mesh.vertices;
        displacedVertices = new Vector3[originalVertices.Length];
    }

    void Update()
    {
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];
            vertex.y = originalVertices[i].y + Mathf.Sin(Time.time * speed + originalVertices[i].x * frequency) * amplitude;
            displacedVertices[i] = vertex;
        }

        mesh.vertices = displacedVertices;
        mesh.RecalculateNormals();
    }
}
