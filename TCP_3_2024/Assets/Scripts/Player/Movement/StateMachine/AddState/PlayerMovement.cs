using System;
using Unity.VisualScripting;
using UnityEngine;
using Fusion;


public class PlayerMovement : NetworkBehaviour
{
    
    private new Rigidbody rigidbody;
    [SerializeField] private float speed = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float forceJump = 5f;
    NetworkTransform networkTransform;
    private new Camera camera;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        networkTransform = GetComponent<NetworkTransform>();
        camera = GetComponent<PlayerController>().camera;
    }


    public void Movement(Vector3 input)
    {
        // Debug.Log(networkTransform.transform.position);
        // TurnToCameraDirection();
        Vector3 moveDirection = (CameraDirection() * input.y + Camera.main.transform.right * input.x).normalized * speed;
        moveDirection.y = 0; 
        
        Vector3 movePlayer = transform.position + moveDirection * Time.fixedDeltaTime;
        rigidbody.MovePosition(movePlayer);
        
        
    }

    public void TurnToCameraDirection()
    {
        rigidbody.transform.rotation = Quaternion.LookRotation(CameraDirection());
    }
    private Vector3 CameraDirection()
    {
        Vector3 cameraDirection = camera.transform.forward;
        cameraDirection.y = 0; 
        cameraDirection.Normalize();

        return cameraDirection;
    }
    
    public void RotateGun(ref Transform pivotGun)
    {
        pivotGun.rotation = camera.transform.rotation;
    }
    
    
    public void IsJumping()
    {
        rigidbody.AddForce(Vector3.up * forceJump, ForceMode.Impulse);
    }
    
    public bool IsGrounded()
    {

        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, 1.01f, groundLayer))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow); 
            Debug.Log("Está no chao"); 
            return true;
        }
          
        return false;
        
    }
    
    

}