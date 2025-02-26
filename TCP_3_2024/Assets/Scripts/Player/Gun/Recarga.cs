using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
public class Recarga : NetworkBehaviour
{
    [SerializeField] private int numCartridge = 2;
    [SerializeField] private int shotsPerCartridge = 30;
    private int currentShots;
    private int currentCartridge;
    [SerializeField] private int rechargeTime;
    [Networked] private TickTimer reloading {get; set;}


    // [SerializeField] private Slider recarregarSlider;
    private bool podeRecarregar = false;
    private float tempoPressionado = 0f;
    private bool carregando = false;
    private int recarregarValor = 0;

    void Start()
    {
        currentShots  = shotsPerCartridge;
        currentCartridge = numCartridge;
    }

    // Update is called once per frame
    void Update()
    {

        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     FillCartridge();
        // }
        if (podeRecarregar)
        {
            if (Input.GetMouseButton(1))
            {
                tempoPressionado += Time.deltaTime;
                if (tempoPressionado >= 2f && !carregando)
                {
                    StartCoroutine(Recarregar());
                }
            }
            else if (Input.GetMouseButtonUp(1))
            {
                ResetarRecarregamento();
            }
        }

    }

    public void Reloading(){
        if ( currentShots <= 0 && currentCartridge <= numCartridge && currentCartridge > 0)
        {
            reloading = TickTimer.CreateFromSeconds(Runner, rechargeTime); 
            currentCartridge--;
            currentShots = shotsPerCartridge;
            Debug.LogWarning("Recarregou");
        }
    }

    public void Atirou(){
        currentShots--;
    }

    public bool PodeAtirar(){
        if (currentShots <= shotsPerCartridge && currentShots > 0 && reloading.ExpiredOrNotRunning(Runner))
        {
            return true;
        }
        return false;
    }

    public void FillCartridge(){
        currentCartridge = numCartridge;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("AreaRecarga"))
        {
            podeRecarregar = true;
            Debug.Log("Entrou na área de recarregamento.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("AreaRecarga"))
        {
            podeRecarregar = false;
            Debug.Log("Saiu da área de recarregamento.");
            ResetarRecarregamento();
        }
    }

    private IEnumerator Recarregar()
    {
        carregando = true;
        recarregarValor = 0;
        while (recarregarValor <= 100)
        {
            recarregarValor++;
            // recarregarSlider.value = recarregarValor; // Atualiza o slider
            Debug.Log("Número: " + recarregarValor);
            yield return null;
        }
        carregando = false;
        FillCartridge();
    }

    private void ResetarRecarregamento()
    {
        StopAllCoroutines();
        tempoPressionado = 0f;
        carregando = false;
        // recarregarSlider.value = 0; // Reseta o slider
    }

}
