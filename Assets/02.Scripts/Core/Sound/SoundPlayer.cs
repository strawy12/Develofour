using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public abstract class SoundPlayer : MonoBehaviour
{
    [SerializeField]
    protected AudioClip audioClip;

    [SerializeField]
    private float pitchRandmness;

    private float basePitch;

    protected AudioSource audioSource;

    public Action<SoundPlayer> OnCompeleted;

    protected int soundID;
    public int SoundID => soundID;
    public float AudioClipLength => audioClip.length;


    private bool isInit;

    public virtual void Init()
    {
        if (isInit) return;
        isInit = true;

        if(audioClip == null)
        {
            Debug.LogError($"{gameObject.name}'s audioClip is Null!");
            return;
        }

        if (audioSource == null)
        {
            AudioSourceInit();
        }

        basePitch = audioSource.pitch;
    }

    public virtual void Release()
    {
        audioSource.Stop();
        audioSource.clip = null;
        OnCompeleted = null;
    }

    private void AudioSourceInit()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

    }

    public void PlayClipWithVariablePitch()
    {
        float randomPitch = Random.Range(-pitchRandmness, pitchRandmness);
        audioSource.pitch = basePitch + randomPitch;
        PlayClip();
    }

    public void PlayClip()
    {
        audioSource.Stop();
        audioSource.clip = audioClip;
        audioSource.Play();

        if(audioSource.loop == false)
        {
            float delay = audioClip.length;
            StartCoroutine(DelayCoroutine(delay));
        }
    }

    private IEnumerator DelayCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay + 0.3f);

        OnCompeleted?.Invoke(this);
    }

    public void ImmediatelyStop()
    {
        audioSource.Stop();
        OnCompeleted?.Invoke(this);
    }
}
