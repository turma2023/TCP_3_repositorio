using System.Collections;
using Fusion;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Bullet2 : NetworkBehaviour
{
    private int ObjectsPenetrated;

    public Rigidbody Rigidbody { get; private set; }


    public delegate void CollisionEvent(Bullet2 Bullet, Collision Collision, int ObjectsPenetrated);

    public event CollisionEvent OnCollision;
   [Networked] private TickTimer life { get; set; }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
    }

    public void Spawn(Vector3 SpawnForce)
    {
        Rigidbody = GetComponent<Rigidbody>();
        Rigidbody.useGravity = false;
        
        ObjectsPenetrated = 0;
        transform.forward = SpawnForce.normalized;
        Rigidbody.AddForce(SpawnForce * 1000);
        life = TickTimer.CreateFromSeconds(Runner, 1.0f);
        // RPC_SpawnBall(SpawnForce * Time.fixedDeltaTime / Rigidbody.mass);
        StartCoroutine(DelayedDisable(2));
        
    }

    public void FixedUpdate()
    {
        
        if(life.Expired(Runner)){
            gameObject.SetActive(false);
            Runner.Despawn(Object);
        }
        // else{
        //     transform.position += 5 * transform.forward * Runner.DeltaTime;
        // }
    }

    private IEnumerator DelayedDisable(float Time)
    {
        yield return null;
        yield return new WaitForSeconds(Time);
        OnCollisionEnter(null);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(this, collision, ObjectsPenetrated);
        ObjectsPenetrated++;
        gameObject.SetActive(false);
        Runner.Despawn(Object);


    }

    private void OnDisable()
    {
        StopAllCoroutines();
        Rigidbody.velocity = Vector3.zero;
        Rigidbody.angularVelocity = Vector3.zero;
        OnCollision = null;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    private void RPC_SpawnBall(Vector3 transform) 
    { 
        // if(delay.ExpiredOrNotRunning(Runner)){
            // delay = TickTimer.CreateFromSeconds(Runner, 0.5f);
            Runner.Spawn(
                gameObject,
                this.transform.position + transform, 
                Quaternion.LookRotation(transform),
                Object.InputAuthority, 
                (runner, o) =>
                {
                    // Initialize the Ball before synchronizing it
                    o.GetComponent<Bullet2>();

                }
            );
        // }
    }

    

    // public void SpawnBullet(Vector3 position, Vector3 force) {
        // Runner.Spawn(gameObject, position, Quaternion.identity, null, (runner, obj) => {
        //      var bullet = obj.GetComponent<Bullet2>(); 
        //      bullet.Spawn(force)
        //      ; 
        // });
    // }

}




// using System.Collections;
// using Fusion;
// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// public class Bullet2 : NetworkBehaviour
// {
//     private int ObjectsPenetrated;

//     public Rigidbody Rigidbody { get; private set; }
//     [field: SerializeField] public Vector3 SpawnLocation { get; private set; }

//     [field: SerializeField] public Vector3 SpawnVelocity { get; private set; }

//     public delegate void CollisionEvent(Bullet2 Bullet, Collision Collision, int ObjectsPenetrated);

//     public event CollisionEvent OnCollision;

//     private void Awake()
//     {
//         Rigidbody = GetComponent<Rigidbody>();
//         Rigidbody.useGravity = false;
//         gameObject.layer = LayerMask.NameToLayer("Tiro");
//     }

//     public void Spawn(Vector3 SpawnForce)
//     {
//         ObjectsPenetrated = 0;
//         SpawnLocation = transform.position;
//         transform.forward = SpawnForce.normalized;
//         Rigidbody.AddForce(SpawnForce);
//         SpawnVelocity = SpawnForce * Time.fixedDeltaTime / Rigidbody.mass;
//         // RPC_SpawnBall(SpawnForce * Time.fixedDeltaTime / Rigidbody.mass);
//         StartCoroutine(DelayedDisable(2));
//     }

//     private IEnumerator DelayedDisable(float Time)
//     {
//         yield return null;
//         yield return new WaitForSeconds(Time);
//         OnTriggerEnter(null);
//     }

//     private void OnTriggerEnter(Collision collision)
//     {
//         if(collision.gameObject.layer != this.gameObject.layer){
//             OnCollision?.Invoke(this, collision, ObjectsPenetrated);
//             ObjectsPenetrated++;
//             RPC_HandleCollision(collision);
//         }
//     }

//     [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
//     private void RPC_HandleCollision(Collision collision)
//     {
//         // Lógica para lidar com a colisão em todos os clientes
//     }

//     private void OnDisable()
//     {
//         StopAllCoroutines();
//         Rigidbody.velocity = Vector3.zero;
//         Rigidbody.angularVelocity = Vector3.zero;
//         OnCollision = null;
//     }
// }
