using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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

    public AudioClip Clip => clip;
    public Sound.EAudioType AudioType => audioType;
    public ESoundPlayerType SoundPlayerType => playerType;
    public float PitchRandmness => pitchRandmness;

    public AudioAssetSO(AudioClip clip, Sound.EAudioType audioType, ESoundPlayerType playerType, float pitchRandmness)
    {
        this.clip = clip;
        this.audioType = audioType;
        this.playerType = playerType;
        this.pitchRandmness = pitchRandmness;
    }
}
