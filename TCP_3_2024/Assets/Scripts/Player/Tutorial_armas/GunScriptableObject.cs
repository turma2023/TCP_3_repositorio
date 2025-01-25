// using System;
// using System.Collections;
// using Fusion;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.EventSystems;
// using UnityEngine.Pool;
// using Random = UnityEngine.Random;

// [CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
// public class GunScriptableObject :  ScriptableObject 
// {

//     public ImpactType ImpactType;
//     public GunType Type;
//     public string Name;
//     public GameObject ModelPrefab;
//     public Vector3 SpawnPoint;
//     public Vector3 SpawnRotation;

//     public ShootConfigScriptableObject ShootConfig;
//     public TrailConfigScriptableObject TrailConfig;

//     private NetworkBehaviour ActiveNetworkBehaviour;
//     private GameObject Model;
//     private float LastHootTime;
//     private ParticleSystem ShootSystem;
//     private ObjectPool<TrailRenderer> TrailPool;

//     public void Spawn(Transform Parent, NetworkBehaviour ActiveNetworkBehaviour){

//         this.ActiveNetworkBehaviour = ActiveNetworkBehaviour;
//         LastHootTime = 0;
//         TrailPool = new ObjectPool<TrailRenderer>(RPC_CreateTrail);

//         Model = Instantiate(ModelPrefab);
//         Model.transform.SetParent(Parent, false);
//         Model.transform.localPosition = SpawnPoint;
//         Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

//         ShootSystem = Model.GetComponentInChildren<ParticleSystem>();
//     }

//     public void Shoot(){
//         if (Time.time > ShootConfig.FireRate + LastHootTime){
//             ShootSystem.Play();
//             Vector3 shootDirection = ShootSystem.transform.forward + 
//             new Vector3(
//                 Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
//                 Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y),
//                 Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z)

//             );
//             shootDirection.Normalize();

//             if (Physics.Raycast(ShootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
//             {
//                 ActiveNetworkBehaviour.StartCoroutine(
//                     PlayTrail(
//                         ShootSystem.transform.position,
//                         hit.point,
//                         hit
//                     )
//                 );
//             }else{
//                  ActiveNetworkBehaviour.StartCoroutine(
//                     PlayTrail(
//                         ShootSystem.transform.position,
//                         ShootSystem.transform.position + (shootDirection * TrailConfig.MissDistance),
//                         new RaycastHit()
//                     )
//                 );
//             }

//         }
//     }

//     private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit, int Iteration = 0)
//         {
//             TrailRenderer instance = TrailPool.Get();
//             instance.gameObject.SetActive(true);
//             instance.transform.position = StartPoint;
//             yield return null; // avoid position carry-over from last frame if reused

//             instance.emitting = true;

//             float distance = Vector3.Distance(StartPoint, EndPoint);
//             float remainingDistance = distance;
//             while (remainingDistance > 0)
//             {
//                 instance.transform.position = Vector3.Lerp(
//                     StartPoint,
//                     EndPoint,
//                     Mathf.Clamp01(1 - (remainingDistance / distance))
//                 );
//                 remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

//                 yield return null;
//             }

//             instance.transform.position = EndPoint;

//             if (Hit.collider != null)
//             {
//                 // HandleBulletImpact(distance, EndPoint, Hit.normal, Hit.collider, Iteration);
//                 SurfaceManager.Instance.HandleImpact(
//                     Hit.transform.gameObject,
//                     EndPoint,
//                     Hit.normal,
//                     ImpactType,
//                     0
//                 );
//             }

//             yield return new WaitForSeconds(TrailConfig.Duration);
//             yield return null;
//             instance.emitting = false;
//             instance.gameObject.SetActive(false);
//             TrailPool.Release(instance);

//         }

//     [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
//     private TrailRenderer RPC_CreateTrail(){
//         // this.ActiveNetworkBehaviour.Runner.Spawn(new GameObject("Bullet Trail"));
//         GameObject instance = new GameObject("Bullet Trail");
//         // NetworkObject instance = 
//         instance.AddComponent<Bullet2>();
//         instance.AddComponent<BoxCollider>().isTrigger = true;
//         instance.AddComponent<NetworkObject>();
//         instance.AddComponent<NetworkTransform>();
        
//         TrailRenderer trail = instance.AddComponent<TrailRenderer>();
//         trail.colorGradient = TrailConfig.Color;
//         trail.material = TrailConfig.Material;
//         trail.widthCurve = TrailConfig.WidthCurve;
//         trail.time = TrailConfig.Duration;
//         trail.minVertexDistance = TrailConfig.MinVertexDistance;

//         trail.emitting = false;
//         trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
//         // this.ActiveNetworkBehaviour.Runner.Spawn(instance);

//         return trail;
        
        

//     }

// }






using System;
using System.Collections;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "Gun", menuName = "Guns/Gun", order = 0)]
public class GunScriptableObject : ScriptableObject 
{
    public ImpactType ImpactType;
    public GunType Type;
    public string Name;
    public GameObject ModelPrefab;
    public Vector3 SpawnPoint;
    public Vector3 SpawnRotation;

    public ShootConfigScriptableObject ShootConfig;
    public TrailConfigScriptableObject TrailConfig;

    private NetworkBehaviour ActiveNetworkBehaviour;
    private NetworkObject Model;
    private float LastHootTime;
    private ParticleSystem ShootSystem;
    private ObjectPool<TrailRenderer> TrailPool;

    public GameObject bulletPrefab;


    public void Spawn(Transform Parent, NetworkBehaviour ActiveNetworkBehaviour){
        this.ActiveNetworkBehaviour = ActiveNetworkBehaviour;
        LastHootTime = 0;

        Model = this.ActiveNetworkBehaviour.Runner.Spawn(ModelPrefab);
        // Model = Instantiate(ModelPrefab);
        Model.transform.SetParent(Parent, false);
        Model.transform.localPosition = SpawnPoint;
        Model.transform.localRotation = Quaternion.Euler(SpawnRotation);

        

        ShootSystem = Model.GetComponentInChildren<ParticleSystem>();

        TrailPool = new ObjectPool<TrailRenderer>(RPC_CreateTrail);

    }

    public void Shoot(){
            //! com que o tiro saida do player que atirou
            if (Time.time > ShootConfig.FireRate + LastHootTime){
                ShootSystem.Play();
                Vector3 shootDirection = ShootSystem.transform.forward + 
                new Vector3(
                    Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
                    Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y),
                    Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z)
                );
                shootDirection.Normalize();

                // RPC_SpawnBall(shootDirection);

                // if (ActiveNetworkBehaviour.HasInputAuthority){
                //     ActiveNetworkBehaviour.Runner.SendRpc(nameof(RPC_SpawnBall), ActiveNetworkBehaviour.Object.InputAuthority, shootDirection);
                // }


                if (Physics.Raycast(ShootSystem.transform.position, shootDirection, out RaycastHit hit, float.MaxValue, ShootConfig.HitMask))
                {
                    ActiveNetworkBehaviour.StartCoroutine(
                        PlayTrail(
                            ShootSystem.transform.position,
                            hit.point,
                            hit
                        )
                    );
                }else{
                    ActiveNetworkBehaviour.StartCoroutine(
                        PlayTrail(
                            ShootSystem.transform.position,
                            ShootSystem.transform.position + (shootDirection * TrailConfig.MissDistance),
                            new RaycastHit()
                        )
                    );
                }


            }
            
    }

    private IEnumerator PlayTrail(Vector3 StartPoint, Vector3 EndPoint, RaycastHit Hit, int Iteration = 0)
    {
        TrailRenderer instance = TrailPool.Get();
        instance.gameObject.SetActive(true);
        instance.transform.position = StartPoint;
        yield return null; // avoid position carry-over from last frame if reused

        instance.emitting = true;

        float distance = Vector3.Distance(StartPoint, EndPoint);
        float remainingDistance = distance;
        while (remainingDistance > 0)
        {
            instance.transform.position = Vector3.Lerp(
                StartPoint,
                EndPoint,
                Mathf.Clamp01(1 - (remainingDistance / distance))
            );
            remainingDistance -= TrailConfig.SimulationSpeed * Time.deltaTime;

            yield return null;
        }

        instance.transform.position = EndPoint;

        if (Hit.collider != null)
        {
            // HandleBulletImpact(distance, EndPoint, Hit.normal, Hit.collider, Iteration);
            // SurfaceManager.Instance.HandleImpact(
            //     Hit.transform.gameObject,
            //     EndPoint,
            //     Hit.normal,
            //     ImpactType,
            //     0
            // );
        }

        yield return new WaitForSeconds(TrailConfig.Duration);
        yield return null;
        instance.emitting = false;
        instance.gameObject.SetActive(false);
        TrailPool.Release(instance);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private TrailRenderer RPC_CreateTrail(){
        // GameObject instance = new GameObject("Bullet Trail");
        // instance.AddComponent<Bullet2>();
        // instance.AddComponent<BoxCollider>().isTrigger = true;
        // instance.AddComponent<NetworkObject>();
        // instance.AddComponent<NetworkTransform>();
        // instance.layer = LayerMask.NameToLayer("Tiro");
        
        // TrailRenderer trail = instance.AddComponent<TrailRenderer>();

        Vector3 shootDirection = ShootSystem.transform.forward + 
                new Vector3(
                    Random.Range(-ShootConfig.Spread.x, ShootConfig.Spread.x),
                    Random.Range(-ShootConfig.Spread.y, ShootConfig.Spread.y),
                    Random.Range(-ShootConfig.Spread.z, ShootConfig.Spread.z)
                );
                shootDirection.Normalize();

        // if (ActiveNetworkBehaviour.HasStateAuthority){
            RPC_SpawnBall(shootDirection);
        // }

        TrailRenderer trail = bulletPrefab.GetComponent<TrailRenderer>();

        trail.colorGradient = TrailConfig.Color;
        trail.material = TrailConfig.Material;
        trail.widthCurve = TrailConfig.WidthCurve;
        trail.time = TrailConfig.Duration;
        trail.minVertexDistance = TrailConfig.MinVertexDistance;

        trail.emitting = false;
        trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        
        return trail;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SpawnBall(Vector3 shootDirection) 
    { 
        // if (ModelGun.GetComponentInChildren<ParticleSystem>() is ParticleSystem p)
        // {
            Debug.Log(this.ShootSystem.gameObject);
            NetworkObject NO = ActiveNetworkBehaviour.Runner.Spawn(
                bulletPrefab, 
                this.ShootSystem.transform.position,
                null,
                null,
                (runner, o) =>
                {
                    o.GetComponent<Bullet2>().Spawn(shootDirection);
                }
            );
        // }
        
    }
}


