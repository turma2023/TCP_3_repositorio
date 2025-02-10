using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target; // O objeto em torno do qual você quer orbitar
    public float speed = 10f; // Velocidade da órbita

    void Update()
    {
        // Gira o objeto em torno do alvo
        transform.RotateAround(target.position, Vector3.up, speed * Time.deltaTime);
    }
}
