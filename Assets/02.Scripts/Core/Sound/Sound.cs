using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public partial class Sound : MonoBehaviour
{
    public static Func<EAudioType, float> OnPlaySound { get; private set; }
    public static Action<EAudioType> OnImmediatelyStop { get; private set; }

    [SerializeField]
    private SoundPlayer soundPlayerPrefab;

    private List<AudioAssetSO> audioAssetList ;

    private List<SoundPlayer> soundPlayerList;
    private Queue<SoundPlayer> soundPlayerPool;

    private void Awake()
    {
        audioAssetList = new List<AudioAssetSO>();
        soundPlayerList = new List<SoundPlayer>();
        soundPlayerPool = new Queue<SoundPlayer>();
    }

    private void Start()
    {
        LoadSoundAssets();

        OnPlaySound += CreateSoundPlayer;

        OnImmediatelyStop += ImmediatelyStop;
    }


    private async void LoadSoundAssets()
    {
        var handle = Addressables.LoadResourceLocationsAsync("Sound", typeof(AudioAssetSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<AudioAssetSO>(handle.Result[i]).Task;
            await task;
            audioAssetList.Add(task.Result);
        }

        Addressables.Release(handle);

        foreach(var clip in audioAssetList)
        {
            Debug.Log(clip.name);
        }
    }

    private float CreateSoundPlayer(EAudioType audioType)
    {
        AudioAssetSO audioAssetData = audioAssetList.Find(x => x.AudioType == audioType);
        if (audioAssetData == null) return -1f;

        object[] ps = new object[1] { audioAssetData.AudioType };

        if (audioType < EAudioType.BGMEnd)
        {
            EventManager.TriggerEvent(ECoreEvent.ChangeBGM, ps);
        }

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

        soundPlayer.Init(audioAssetData);
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
