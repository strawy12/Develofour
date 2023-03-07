using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(menuName = "SO/AudioAsset")]
public class AudioAssetSO : ScriptableObject
{
    [ContextMenu("SetName")]
    public void SetName()
    {
        name = $"{audioType}";
    }
    [SerializeField]
    private AudioClip clip;

    [SerializeField]
    private Sound.EAudioType audioType;

    [SerializeField]
    private ESoundPlayerType playerType;

    [SerializeField]
    private float pitchRandmness;

    [SerializeField]
    [Range(0.1f, 1f)]
    private float volume = 1f;

    public AudioClip Clip => clip;
    public Sound.EAudioType AudioType => audioType;
    public ESoundPlayerType SoundPlayerType => playerType;
    public float PitchRandmness => pitchRandmness;
    public float Volume => volume;

    public AudioAssetSO(AudioClip clip, Sound.EAudioType audioType, ESoundPlayerType playerType, float pitchRandmness, float volume)
    {
        this.clip = clip;
        this.audioType = audioType;
        this.playerType = playerType;
        this.pitchRandmness = pitchRandmness;
        this.volume = volume;
    }
}
