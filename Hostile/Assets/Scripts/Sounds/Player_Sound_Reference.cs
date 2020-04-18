﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Sound_Reference : MonoBehaviour
{
    public AudioClip[] footSteps;
    public AudioClip[] footStepsRocks;
    public AudioClip[] GetHit;
    public AudioClip[] Die;

    public AudioSource source;
    private System.Random rd = new System.Random();
    private AudioClip previousClip;

    public int indexGround = 0;
    public bool isRunning;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
    }


    public void PlayFootSteps(AnimationEvent animationEvent)
    {
        //Debug.Log(animationEvent.animatorClipInfo.clip.name + " // " + animationEvent.animatorClipInfo.weight);
        if(animationEvent.animatorClipInfo.weight >= 0.5)
        {
            if (!isRunning)
                source.pitch = Random.Range(0.9f, 1.1f);
            else
                source.pitch = Random.Range(1.1f, 1.3f);
            AudioClip clip = GetAudioClip(indexGround);
            source.PlayOneShot(clip);
        }
    }

    public void PlayGetHit(int pv)
    {
        PV.RPC("PlayGetHit_Local", RpcTarget.AllViaServer, pv);
    }

    [PunRPC]
    public void PlayGetHit_Local(int pv)
    {
        if(PV.ViewID == pv)
        {
            source.pitch = Random.Range(0.9f, 1.1f);
            source.volume = Random.Range(0.60f, 0.70f);
            source.PlayOneShot(GetAudioClip(2));
        }
    }

    private AudioClip GetAudioClip(int indexArray)
    {
        int i = 3;
        AudioClip[] clips;
        switch (indexArray)
        {
            case 0:
                clips = footSteps;
                source.volume = Random.Range(0.009f, 0.012f);
                break;
            case 1:
                clips = footStepsRocks;
                source.volume = Random.Range(0.025f, 0.029f);
                break;
            case 2:
                clips = GetHit;
                break;
            case 3:
                clips = Die;
                break;
            default:
                clips = footSteps;
                break;
             
        }
        AudioClip selectedClip = clips[Random.Range(0, clips.Length)];

        while(selectedClip == previousClip && i > 0)
        {
            selectedClip = clips[Random.Range(0, footSteps.Length - 1)];
            i--;
        }
        return selectedClip;
    }
}
