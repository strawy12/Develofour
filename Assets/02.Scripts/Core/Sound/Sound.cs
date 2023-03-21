using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Audio;
using UnityEngine.UI;

public partial class Sound : MonoBehaviour
{
    public static Func<EAudioType, float> OnPlaySound { get; private set; }
    public static Action<EAudioType> OnImmediatelyStop { get; private set; }

    [SerializeField]
    private SoundPlayer soundPlayerPrefab;

    [SerializeField]
    private AudioMixerGroup bgmMixerGroup;
    [SerializeField]
    private AudioMixerGroup effectMixerGroup;

    private List<SoundPlayer> soundPlayerList;
    private Queue<SoundPlayer> soundPlayerPool;

    private void Awake()
    {
        soundPlayerList = new List<SoundPlayer>();
        soundPlayerPool = new Queue<SoundPlayer>();
    }

    private void Start()
    {
        GameManager.Inst.OnStartCallback += SoundStartCallback;
    }

    private void SoundStartCallback()
    {
        OnPlaySound += CreateSoundPlayer;
        OnImmediatelyStop += ImmediatelyStop;
    }

    private float CreateSoundPlayer(EAudioType audioType)
    {
        AudioAssetSO audioAssetData = ResourceManager.Inst.GetAudioResource(audioType);
        if (audioAssetData == null) return -1f;

        SoundPlayer soundPlayer = null;

        if(soundPlayerPool.Count != 0)
        {
            soundPlayer = soundPlayerPool.Dequeue();
        }

        else
        {
            soundPlayer = Instantiate(soundPlayerPrefab, transform);
        }

        soundPlayerList.Add(soundPlayer);

        AudioMixerGroup mixerGroup = (audioAssetData.SoundPlayerType == ESoundPlayerType.BGM) ? bgmMixerGroup : effectMixerGroup;

        soundPlayer.Init(audioAssetData, mixerGroup);
        soundPlayer.gameObject.SetActive(true);

        soundPlayer.PlayClip();
        soundPlayer.OnCompeleted += CompletedPlayer;

        return soundPlayer.AudioClipLength;
    }

    private void CompletedPlayer(SoundPlayer player)
    {
        player.Release();
        player.gameObject.SetActive(false);
        soundPlayerPool.Enqueue(player);
        soundPlayerList.Remove(player);
    }

    private void ImmediatelyStop(EAudioType type)
    {
        var list = soundPlayerList.FindAll(x => x.AudioType == type);

        foreach (SoundPlayer player in list)
        {
            player.ImmediatelyStop();
            soundPlayerList.Remove(player);
        }
    }

    private void OnDestroy()
    {
        OnPlaySound -= CreateSoundPlayer;
        OnImmediatelyStop -= ImmediatelyStop;
    }
}
