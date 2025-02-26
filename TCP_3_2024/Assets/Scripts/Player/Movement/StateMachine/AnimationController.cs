using System.Collections;
using System.Collections.Generic;
using Fusion;
using UnityEngine;

public class AnimationController : NetworkBehaviour
{
    private NetworkMecanimAnimator NetworkAnimator;
    void Start()
    {
        NetworkAnimator = GetComponentInChildren<NetworkMecanimAnimator>();
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RpcSetAnimationTrigger(string triggerName, bool value)
    {
        NetworkAnimator.SetTrigger(triggerName, value);
    }

    public void PlayIdle(bool value){
        RpcSetAnimationTrigger("Armature_Idle", value);
    }

    public void PlayWalk(bool value){
        RpcSetAnimationTrigger("Armature_Correndo", value);
    }

    public void PlayJump(bool value){
        RpcSetAnimationTrigger("Armature_Pulando", value);
    }

    public void PlayDead(bool value){
        RpcSetAnimationTrigger("Armature_Dead", value);
    }


}
