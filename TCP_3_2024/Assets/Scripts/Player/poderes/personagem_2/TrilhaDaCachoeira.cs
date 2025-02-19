using UnityEngine;
using Fusion; // Se estiver usando Photon Fusion

public class TrilhaDaCachoeira : NetworkBehaviour
{
    public float força_do_dash = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Object.HasInputAuthority)
        {
            StartDash();
        }
    }

    void StartDash()
    {
        rb.AddForce(transform.forward * força_do_dash, ForceMode.Impulse);
    }
}
