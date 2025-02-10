using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.VisualScripting;
using UnityEngine;

public class GunFire : NetworkBehaviour
{

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private PlayerController playerController;
    private PlayerInputController playerInputController;

    [SerializeField] private int damage = 5;

    [SerializeField] private NetworkObject _prefabBall;

    [Networked] private TickTimer delay { get; set; }
    private ParticleSystem particles;
    
    [SerializeField] private float spreadAmountMax = 5f;
    [SerializeField] private float spreadAmountMin = 0f;

    private float spreadAmountCurrotin = 0f;

    [SerializeField] private float recoilForce = 0.1f;
    [SerializeField] private float recoilDuration = 0.1f;

    private Vector3 originalPosition;


    [SerializeField] private RectTransform imageRectTransform;

    [SerializeField] private float resizeDuration = 0.5f;
    private int minSize = 50;
    private int maxSize = 200;

    [SerializeField] private BallPool bulletPool;

    void Start()
    {
        playerInputController = playerController.PlayerInputController;

        if (_prefabBall == null) { 
            Debug.LogError("Prefab da bolinha não está atribuído.");
        }
        particles = GetComponentInChildren<ParticleSystem>();
        originalPosition = transform.localPosition;

        spreadAmountMax = spreadAmountMax / 100;
        spreadAmountCurrotin = spreadAmountMin;
        if (Object.HasInputAuthority){
            imageRectTransform.gameObject.SetActive(true);
        }

    }

    void ResizeImage(float width, float height)
    {
        imageRectTransform.sizeDelta = new Vector2(width, height);
    }
    

    private void FixedUpdate()
    // public override void FixedUpdateNetwork()
    {
        if (Object.HasInputAuthority){

            float scale = Mathf.Lerp(minSize, maxSize, spreadAmountCurrotin / spreadAmountMax);
            ResizeImage(scale, scale);

            if(delay.ExpiredOrNotRunning(Runner)){
                delay = TickTimer.CreateFromSeconds(Runner, 0.1f);

                

                if (playerInputController.FireAction.IsPressed()){
                    RaycastHit hit;
                    Vector3 shootDirection = GetSpreadDirection(playerController.camera.transform.forward);


                    if (Physics.Raycast(playerController.camera.transform.position, shootDirection, out hit, 100, layerMask))
                    {
                        Debug.DrawRay(playerController.camera.transform.position, shootDirection * hit.distance, Color.red);

                        PlayerController hitPayerControllerLife = hit.transform.GetComponent<PlayerController>(); 
                        RPC_ShootEffect();
                        RPC_SpawnBall(hit.point);
                        if (hitPayerControllerLife != null)
                        { 
                            RPC_TakeDamage(hitPayerControllerLife, damage); 
                        } 
                    }
                    
                }
                else{
                    if (spreadAmountCurrotin > spreadAmountMin)
                    {
                        spreadAmountCurrotin = spreadAmountCurrotin - 0.01f;
                        
                    }
                }
            }
            
        
        }
        
    }

    private Vector3 GetSpreadDirection(Vector3 originalDirection)
    {
        float spreadX = UnityEngine.Random.Range(-spreadAmountCurrotin, spreadAmountCurrotin);
        float spreadY = UnityEngine.Random.Range(-spreadAmountCurrotin, spreadAmountCurrotin);
        float spreadZ = UnityEngine.Random.Range(-spreadAmountCurrotin, spreadAmountCurrotin);
        Vector3 spreadDirection = originalDirection + new Vector3(spreadX, spreadY, spreadZ);
        return spreadDirection.normalized;
    }

    private IEnumerator ApplyRecoil()
    {
        Vector3 recoilPosition = originalPosition - new Vector3(0, 0, recoilForce);
        float elapsedTime = 0f;

        while (elapsedTime < recoilDuration)
        {
            transform.localPosition = Vector3.Lerp(originalPosition, recoilPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;

        while (elapsedTime < recoilDuration)
        {
            transform.localPosition = Vector3.Lerp(recoilPosition, originalPosition, elapsedTime / recoilDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPosition;
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    private void RPC_ShootEffect(){
        particles.Play();
        StartCoroutine(ApplyRecoil());
            
        if (spreadAmountCurrotin < spreadAmountMax)
        {
            spreadAmountCurrotin = spreadAmountCurrotin + 0.005f;

        }
    }
    

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_TakeDamage(PlayerController playerController, int damage) 
    { 
        // ! preciso verificar qual o Time do player que foi atingido pelo tiro, codigo abaixo não funciona

        Debug.Log(playerController.GetTeam());

        if (this.playerController.tag != playerController.tag)
        {
            Debug.Log("Dano no time: " + playerController.tag);
            playerController.TakeDamage(damage); 
        }else{
            Debug.Log("sem dano no time: " + playerController.tag);
        }
        
            
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SpawnBall(Vector3 transform) 
    { 
        Runner.Spawn(
            _prefabBall,
            transform, 
            Quaternion.LookRotation(this.transform.forward * -1),
            Object.InputAuthority, 
            (runner, o) =>
            {
                o.GetComponent<Ball>().Init();
            }
        );  
    }


}
