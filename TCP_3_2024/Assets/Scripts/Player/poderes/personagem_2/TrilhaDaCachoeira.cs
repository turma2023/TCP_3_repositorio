using UnityEngine;
using Fusion; // Se estiver usando Photon Fusion

public class TrilhaDaCachoeira : Skill
{
    public float forca_do_dash = 10f;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Object.HasInputAuthority && HasUsed)
        {
            StartDash();
            DisableUse();
        }
    }

    void StartDash()
    {
        rb.AddForce(transform.forward * forca_do_dash, ForceMode.Impulse);
    }
}
