using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public partial class Sound : MonoBehaviour
{
    public static Action<int> OnPlaySound {get;private set;}
    public static Action<EBgm> OnPlayBGMSound { get; private set; }
    public static Action<EEffect> OnPlayEffectSound { get; private set; }

    private Dictionary<int, SoundPlayer> soundPlayerDictionary;
    private Dictionary<int, Queue<SoundPlayer>> soundPlayerPoolDictionary;

    private void Awake()
    {
        soundPlayerDictionary = new Dictionary<int, SoundPlayer>();
        soundPlayerPoolDictionary = new Dictionary<int, Queue<SoundPlayer>>();

        OnPlaySound += CreateSoundPlayer;
        OnPlayBGMSound += (id) => CreateSoundPlayer((int)id);
        OnPlayEffectSound += (id) => CreateSoundPlayer((int)id);

        AddSoundPlayer();
    }

    private void AddSoundPlayer()
    {
        int count = transform.childCount;

        for(int i = 0; i < count; i++)
        {
            SoundPlayer soundPlayer = transform.GetChild(i).GetComponent<SoundPlayer>();
            soundPlayer.Init();

            soundPlayerDictionary[soundPlayer.SoundID] = soundPlayer;

            soundPlayerPoolDictionary[soundPlayer.SoundID] = new Queue<SoundPlayer>();
            soundPlayerPoolDictionary[soundPlayer.SoundID].Enqueue(soundPlayer);
        }
    }

    private void CreateSoundPlayer(int soundID)
    {

        if (soundPlayerPoolDictionary.ContainsKey(soundID) == false &&
            soundPlayerDictionary.ContainsKey(soundID) == false) return;

        if(soundID < (int)EBgm.Count)
        {
            EventManager.TriggerEvent(EEvent.ChangeBGM, soundID);
        }

        SoundPlayer soundPlayer = null;

        if (soundPlayerPoolDictionary[soundID].Count <= 0)
        {
            soundPlayer = Instantiate(soundPlayerDictionary[soundID], transform);
        }
        else
        {
            soundPlayer = soundPlayerPoolDictionary[soundID].Dequeue();
        }

        soundPlayer.Init();
        soundPlayer.gameObject.SetActive(true);

        soundPlayer.PlayClip();
        soundPlayer.OnCompeleted += CompletedPlayer;
    }

    private void CompletedPlayer(SoundPlayer player)
    {
        int id = player.SoundID;
        if (soundPlayerPoolDictionary.ContainsKey(id) == false)
        {
            Debug.LogError("에러에러여레어렝");
            return;
        }

        player.Release();
        player.gameObject.SetActive(false);
        soundPlayerPoolDictionary[id].Enqueue(player);
    }

    private void OnDestroy()
    {
        OnPlaySound -= CreateSoundPlayer;
        OnPlayBGMSound -= (id) => CreateSoundPlayer((int)id);
        OnPlayEffectSound -= (id) => CreateSoundPlayer((int)id);
    }
}
