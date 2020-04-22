using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Player_Sound_Reference : MonoBehaviour
{
    public AudioClip[] footSteps;
    public AudioClip[] footStepsRocks;
    public AudioClip[] GetHit;
    public AudioClip[] Die;
    public AudioClip[] Woosh;
    public AudioClip[] Landing;
    public AudioClip[] Breathing;

    public AudioSource source;
    public AudioSource source2;
    public AudioSource source3;
    private System.Random rd = new System.Random();
    private AudioClip previousClip;

    public int indexGround = 0;
    public bool isRunning;
    private PhotonView PV;

    private void Start()
    {
        PV = GetComponent<PhotonView>();
        Joueur.StatsController.instance.sounds = this;
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

    public void Play(Vector3 position, int i, int sourceIndex)
    {
        PV.RPC("Play_Local", RpcTarget.AllViaServer, position, i, sourceIndex);
    }

    [PunRPC]
    public void Play_Local(Vector3 position, int i, int sourceIndex)
    {
            float volume = 0;
            switch (sourceIndex)
            {
                case 0:
                    source.pitch = Random.Range(0.9f, 1.1f);
                    volume = Random.Range(0.60f, 0.70f);
                    AudioSource.PlayClipAtPoint(GetAudioClip(i), position, volume);
                    break;
                case 1:
                    source2.pitch = Random.Range(0.9f, 1.1f);
                    volume = Random.Range(0.60f, 0.70f);
                    AudioSource.PlayClipAtPoint(GetAudioClip(i), position, volume);
                    break;
                case 2:
                    source2.pitch = Random.Range(0.9f, 1.1f);
                    volume = Random.Range(0.20f, 0.30f);
                    AudioSource.PlayClipAtPoint(GetAudioClip(i), position, volume);
                    break;
                default:
                    source.pitch = Random.Range(0.9f, 1.1f);
                    volume = Random.Range(0.60f, 0.70f);
                    AudioSource.PlayClipAtPoint(GetAudioClip(i), position, volume);
                break;
            }

        


        
    }

    public void PlayBreathing(int pv, int i)
    {
        PV.RPC("PlayBreathing_RPC", RpcTarget.AllViaServer,pv, i);
    }

    [PunRPC]
    public void PlayBreathing_RPC(int pv, int i)
    {
        if(PV.ViewID == pv)
        {
            if (i == 0) //breathing IN
            {
                source3.clip = Breathing[0];
                source3.loop = true;
                source3.volume = 0.05f;
                source3.Play();
                StartCoroutine(FadeIn(source3, 0.8f, 0.35f));
            }
            else
            {
                StartCoroutine(FadeOut(source3, 2f));
            }
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
            case 4:
                clips = Woosh;
                break;
            case 5:
                clips = Landing;
                break;
            case 6:
                clips = Breathing;
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

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0.000f)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        Debug.Log("stopped");
    }

    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime, float maxVolume)
    {
        float startVolume = 0.1f;

        audioSource.volume = 0;
        audioSource.Play();

        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        Debug.Log("max volume");
        audioSource.volume = maxVolume;
    }
}
