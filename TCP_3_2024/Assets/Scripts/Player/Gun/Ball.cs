using System;
using System.Collections;
using System.Collections.Generic;
using Fusion;
using Unity.Mathematics;
using UnityEngine;


public class Ball : NetworkBehaviour
{
  [Networked] private TickTimer life { get; set; }

  public void Init()
  {
    life = TickTimer.CreateFromSeconds(Runner, 0.5f);
  }

  // public void Start()
  // {
  //   life = TickTimer.CreateFromSeconds(Runner, 1.0f);
  // }

  public override void FixedUpdateNetwork()
  {
    
    if(life.Expired(Runner)){
        gameObject.SetActive(false);
        Runner.Despawn(Object);
        // Debug.Log("Acabou vida tiro");
    }
    else{
        // transform.position += 5 * transform.forward * Runner.DeltaTime;
    }
  }
}