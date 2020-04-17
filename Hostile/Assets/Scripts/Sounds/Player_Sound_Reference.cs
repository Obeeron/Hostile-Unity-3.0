using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void PlayGetHit()
    {
        source.pitch = Random.Range(0.9f, 1.1f);
        source.volume = Random.Range(0.60f, 0.70f);
        AudioClip clip = GetAudioClip(2);
        source.PlayOneShot(clip);
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
        Debug.Log(indexArray);
        AudioClip selectedClip = clips[Random.Range(0, footSteps.Length - 1)];

        while(selectedClip == previousClip && i > 0)
        {
            selectedClip = clips[Random.Range(0, footSteps.Length - 1)];
            i--;
        }
        return selectedClip;
    }
}
