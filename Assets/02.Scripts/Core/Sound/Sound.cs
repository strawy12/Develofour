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
    public static Action<bool> OnStopBGM { get; private set; }
    public static Action OnPlayLastBGM { get; private set; }

    [SerializeField]
    private SoundPlayer soundPlayerPrefab;

    [SerializeField]
    private AudioMixerGroup bgmMixerGroup;
    [SerializeField]
    private AudioMixerGroup effectMixerGroup;

    private List<SoundPlayer> soundPlayerList;
    private Queue<SoundPlayer> soundPlayerPool;

    private EAudioType lastBGMAudioType = EAudioType.StartMainBGM;

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
        OnStopBGM += BGMStop;
        OnPlayLastBGM += LastBGMStart;
    }

    private float CreateSoundPlayer(EAudioType audioType)
    {
        AudioAssetSO audioAssetData = ResourceManager.Inst.GetAudioResource(audioType);
        if (audioAssetData == null) return -1f;

        SoundPlayer soundPlayer = null;

        if (soundPlayerPool.Count != 0)
        {
            soundPlayer = soundPlayerPool.Dequeue();
        }

        else
        {
            soundPlayer = Instantiate(soundPlayerPrefab, transform);
        }
        if (audioAssetData.SoundPlayerType == ESoundPlayerType.BGM)
        {
            BGMStop(false);
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

    private void BGMStop(bool isAddLastList)
    {
        var list = soundPlayerList.FindAll(x => x.AudioData.SoundPlayerType == ESoundPlayerType.BGM);
        foreach (var player in list)
        {
            if (player.isPlaying)
            {
                if (isAddLastList)
                    lastBGMAudioType = player.AudioType;
                player.ImmediatelyStop();
                soundPlayerList.Remove(player);
            }
        }
    }

    private void LastBGMStart()
    {
        CreateSoundPlayer(lastBGMAudioType);
    }

    private void ImmediatelyStop(EAudioType type)
    {
        if (type == EAudioType.None) return;
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
        OnStopBGM -= BGMStop;
        OnPlayLastBGM -= LastBGMStart;
    }
}
