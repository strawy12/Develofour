using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
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

    public List<AudioMixerGroup> audioMixerGroups = new List<AudioMixerGroup>();

    private float basePitch;

    public AudioAssetSO AudioData
    {
        get
        {
            return audioData;
        }
    }

    public virtual void Init(AudioAssetSO data, AudioMixerGroup mixerGroup)
    {
        audioData = data;
        gameObject.name = $"{PlayerType}_{data.AudioType}";

        if (audioData == null)
        {
            Debug.LogError($"{gameObject.name}'s audioData is Null!");
            return;
        }

        if (audioSource == null)
        {
            AudioSourceInit();
        }

        audioSource.clip = audioData.Clip;
        audioSource.volume = audioData.Volume;
        audioSource.outputAudioMixerGroup = mixerGroup;

        audioSource.loop = PlayerType == ESoundPlayerType.BGM;
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
    }

    public void PlayClipWithVariablePitch()
    {
        if(audioData.PitchRandmness != 0f)
        {
            float randomPitch = Random.Range(-audioData.PitchRandmness, audioData.PitchRandmness);
            audioSource.pitch = basePitch + randomPitch;
        }
    }

    public void PlayClip()
    {
        PlayClipWithVariablePitch();
        audioSource.Stop();
        audioSource.clip = audioData.Clip;
        audioSource.outputAudioMixerGroup = audioMixerGroups[(int)audioData.SoundPlayerType];
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
