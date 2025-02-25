using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
public class Recarga : NetworkBehaviour
{
    [SerializeField] private int numCartridge = 2;
    [SerializeField] private int shotsPerCartridge = 30;
    private int currentShots;
    private int currentCartridge;
    [SerializeField] private int rechargeTime;
    [Networked] private TickTimer reloading {get; set;}

    void Start()
    {
        currentShots  = shotsPerCartridge;
        currentCartridge = numCartridge;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.LogWarning("municao: "+currentShots);
        Debug.LogWarning("Cartuchos: "+currentCartridge);

        if (Input.GetKeyDown(KeyCode.T))
        {
            FillCartridge();
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



}
