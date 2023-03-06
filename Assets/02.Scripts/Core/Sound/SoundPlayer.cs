using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ESoundPlayerType
{
    None = -1,
    BGM,
    Effect,
}

[RequireComponent(typeof(AudioSource))]
public class SoundPlayer : MonoBehaviour
{
    private AudioAssetSO audioData;

    protected AudioSource audioSource;

    public Action<SoundPlayer> OnCompeleted;
    public float AudioClipLength => audioData.Clip.length;
    public ESoundPlayerType PlayerType => audioData.SoundPlayerType;
    public Sound.EAudioType AudioType => audioData.AudioType;

    private bool isInit;
    private float basePitch;

    public virtual void Init(AudioAssetSO data)
    {
        if (isInit) return;
        isInit = true;

        audioData = data;
        gameObject.name = $"{PlayerType.ToString()}_{data.AudioType.ToString()}";

        if (audioData == null)
        {
            Debug.LogError($"{gameObject.name}'s audioData is Null!");
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

    public void AudioSourceInit()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.clip = audioData.Clip;

        audioSource.loop = PlayerType == ESoundPlayerType.BGM;
    }

    public void PlayClipWithVariablePitch()
    {
        float randomPitch = Random.Range(-audioData.PitchRandmness, audioData.PitchRandmness);
        audioSource.pitch = basePitch + randomPitch;
        PlayClip();
    }

    public void PlayClip()
    {
        audioSource.Stop();
        audioSource.clip = audioData.Clip;
        audioSource.Play();

        if (audioSource.loop == false)
        {
            float delay = AudioClipLength;
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
