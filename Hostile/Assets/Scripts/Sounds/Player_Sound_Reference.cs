using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Sound_Reference : MonoBehaviour
{
    public AudioClip[] footSteps;
    public AudioClip[] GetHit;
    public AudioClip[] Die;

    public AudioSource source;
    private System.Random rd = new System.Random();
    private AudioClip previousClip;

    public bool isRunning;

    public void PlayFootSteps(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.2)
        {
            if(isRunning)
                source.pitch = Random.Range(0.9f, 1.1f);
            else
                source.pitch = Random.Range(1.1f, 1.3f);
            source.volume = Random.Range(0.10f, 0.20f);
            AudioClip clip = GetAudioClip();
            source.PlayOneShot(clip);
        }
    }

    private AudioClip GetAudioClip()
    {
        int i = 3;
        AudioClip selectedClip = footSteps[Random.Range(0, footSteps.Length - 1)];

        while(selectedClip == previousClip && i > 0)
        {
            selectedClip = footSteps[Random.Range(0, footSteps.Length - 1)];
            i--;
        }
        return selectedClip;
    }
}
