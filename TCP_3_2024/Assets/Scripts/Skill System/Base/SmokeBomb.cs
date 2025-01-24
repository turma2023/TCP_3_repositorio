using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SmokeBomb : MonoBehaviour
{
    [SerializeField] private LayerMask collisionLayers;
    [SerializeField] private GameObject anticipationParticles;
    [SerializeField] private GameObject expandedParticles;
    private LineRenderer trajectoryLine;
    private Rigidbody rigidbody;


    public void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
        trajectoryLine = GetComponent<LineRenderer>();
        trajectoryLine.enabled = false;
    }

    public void Throw(float throwForce)
    {
        rigidbody.useGravity = true;
        rigidbody.AddForce((transform.forward + transform.up) * throwForce, ForceMode.Impulse);
        trajectoryLine.enabled = false;
    }
    public void ShowTrajectory(Vector3 startPosition, Vector3 initialVelocity)
    {
        trajectoryLine.enabled = true;
        int pointsCount = 100;
        float timeStep = 0.025f;

        Vector3[] points = new Vector3[pointsCount];
        for (int i = 0; i < pointsCount; i++)
        {
            float time = i * timeStep;
            points[i] = startPosition + (initialVelocity * time) + (0.5f * Physics.gravity * time * time);
        }

        trajectoryLine.positionCount = pointsCount;
        trajectoryLine.SetPositions(points);
    }

    public void HideTrajectory()
    {
        trajectoryLine.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == collisionLayers)
        {
            rigidbody.useGravity = false;
            GetComponent<ParticleSystem>().Play();
        }
    }
}
