using System.Collections;
using Fusion;
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

    private Recarga recarga;
    void Start()
    {
        playerInputController = playerController.PlayerInputController;

        particles = GetComponentInChildren<ParticleSystem>();
        originalPosition = transform.localPosition;

        spreadAmountMax = spreadAmountMax / 100;
        spreadAmountCurrotin = spreadAmountMin;
        if (Object.HasInputAuthority)
        {
            imageRectTransform.gameObject.SetActive(true);
        }

        recarga = GetComponentInChildren<Recarga>();
    }

    void ResizeImage(float width, float height)
    {
        imageRectTransform.sizeDelta = new Vector2(width, height);
    }


    private void FixedUpdate()
    {
        if (playerController.IsDead) return;

        if (Object.HasInputAuthority)
        {
            float scale = Mathf.Lerp(minSize, maxSize, spreadAmountCurrotin / spreadAmountMax);
            ResizeImage(scale, scale);

            recarga.Reloading();

            if (delay.ExpiredOrNotRunning(Runner) && recarga.PodeAtirar())
            {
                delay = TickTimer.CreateFromSeconds(Runner, 0.1f);

                if (playerInputController.FireAction.IsPressed())
                {

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
                    else
                    {
                        RPC_ShootEffect();
                    }

                }
                else
                {
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
        float spreadX = Random.Range(-spreadAmountCurrotin, spreadAmountCurrotin);
        float spreadY = Random.Range(-spreadAmountCurrotin, spreadAmountCurrotin);
        float spreadZ = Random.Range(-spreadAmountCurrotin, spreadAmountCurrotin);
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
    private void RPC_ShootEffect()
    {
        particles.Play();
        StartCoroutine(ApplyRecoil());
        recarga.Atirou();
        if (spreadAmountCurrotin < spreadAmountMax)
        {
            spreadAmountCurrotin = spreadAmountCurrotin + 0.005f;

        }
    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_TakeDamage(PlayerController playerController, int damage)
    {

        if (this.playerController.TeamSide != playerController.TeamSide)
        {
            playerController.TakeDamage(damage);
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
