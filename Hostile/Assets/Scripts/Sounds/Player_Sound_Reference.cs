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
    private int i = 1;
    private int j = 0;

    public void PlayFootSteps(AnimationEvent animationEvent)
    {
        if(animationEvent.animatorClipInfo.weight > 0.2)
        {

            j = rd.Next(0, footSteps.Length);
            Debug.Log("footsteps");
            if (i < footSteps.Length)
            {
                source.PlayOneShot(footSteps[i]);
                i += 1;
            }
            else
            {
                source.PlayOneShot(footSteps[0]);
                i = 1;
            }
        }

        
    }
}
