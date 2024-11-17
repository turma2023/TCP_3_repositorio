using System;
using Unity.VisualScripting;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    
    private new Rigidbody rigidbody;

    [SerializeField] private float speed = 10f;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    public void Movement(Vector2 input)
    {
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
        Vector3 cameraDirection = Camera.main.transform.forward;
        cameraDirection.y = 0; 
        cameraDirection.Normalize();

        return cameraDirection;
    }
    
    public void RotateGun(ref Transform pivotGun)
    {
        pivotGun.rotation = Camera.main.transform.rotation;
    }

}