using UnityEngine;
using Fusion;
using System.Collections;
public class PlayerMovement : NetworkBehaviour
{

    private new Rigidbody rigidbody;
    [SerializeField] private float speed = 10f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float forceJump = 5f;

    private new Camera camera;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        camera = GetComponent<PlayerController>().camera;
    }


    public void Movement(Vector3 input)
    {

        Vector3 moveDirection = (CameraDirection() * input.y + camera.transform.right * input.x).normalized * speed;
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








////mechido por vitor a baixo /////////

public bool isStunned = false;
    public bool isspideeffect = false;
    public void Stun(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(StunEffect(duration));
        }
    }
    public void speedbuff(float duration)
    {
        if (!isStunned)
        {
            StartCoroutine(spideeffect(duration));
        }
    }


    private IEnumerator StunEffect(float duration)
    {
        isStunned = true;

        // Aqui você pode desativar o movimento do jogador
        speed = 0;
        Debug.LogWarning("Jogador foi atingido e está atordoado!");

        yield return new WaitForSeconds(duration);

        // Reativa o movimento após o tempo de stun
        isStunned = false;
        speed = 10;
        Debug.LogWarning("Jogador se recuperou do stun!");
    }

    private IEnumerator spideeffect(float duration)
    {
        isspideeffect = true;

        // Aqui você pode desativar o movimento do jogador
        speed = 20;
        Debug.LogWarning("Jogador foi atingido e está atordoado!");

        yield return new WaitForSeconds(duration);

        // Reativa o movimento após o tempo de stun
        isspideeffect = false;
        speed = 10;
        Debug.LogWarning("Jogador se recuperou do stun!");
    }




}