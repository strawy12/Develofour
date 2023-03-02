using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public partial class Sound : MonoBehaviour
{
    public static Func<EBgm, float> OnPlayBGMSound { get; private set; }
    public static Func<EEffect, float> OnPlayEffectSound { get; private set; }
    public static Action<int> OnImmediatelyStop { get; private set; }

    private Dictionary<int, SoundPlayer> soundPlayerDictionary;
    private Dictionary<int, Queue<SoundPlayer>> soundPlayerPoolDictionary;

    private List<SoundPlayer> soundPlayerList;

    private void Awake()
    {
        soundPlayerDictionary = new Dictionary<int, SoundPlayer>();
        soundPlayerPoolDictionary = new Dictionary<int, Queue<SoundPlayer>>();
        soundPlayerList = new List<SoundPlayer>();

        OnPlayBGMSound += CreateBGMSoundPlayer;
        OnPlayEffectSound += CreateEffectSoundPlayer;

        OnImmediatelyStop += ImmediatelyStop;

        AddSoundPlayers();
    }

    private void AddSoundPlayers()
    {
        List<SoundPlayer> soundPlayers = new List<SoundPlayer>();

        for (int n = 0; n < 2; n++)
        {
            for (int i = 0; i < transform.GetChild(n).childCount; i++)
            {
                SoundPlayer soundPlayer = transform.GetChild(n).GetChild(i).GetComponent<SoundPlayer>();
                soundPlayer.Init();

                soundPlayerDictionary[soundPlayer.SoundID] = soundPlayer;

                soundPlayerPoolDictionary[soundPlayer.SoundID] = new Queue<SoundPlayer>();
                soundPlayerPoolDictionary[soundPlayer.SoundID].Enqueue(soundPlayer);
            }
        }

    }

    private float CreateBGMSoundPlayer(EBgm type)
    {
        return CreateSoundPlayer((int)type);
    }
    private float CreateEffectSoundPlayer(EEffect type)
    {
        return CreateSoundPlayer((int)type);
    }


    private float CreateSoundPlayer(int soundID)
    {
        if (soundPlayerPoolDictionary.ContainsKey(soundID) == false &&
            soundPlayerDictionary.ContainsKey(soundID) == false) return -1f;

        object[] ps = new object[1] { soundID };

        if (soundID < (int)EBgm.End)
        {
            EventManager.TriggerEvent(ECoreEvent.ChangeBGM, ps);
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

        soundPlayerList.Add(soundPlayer);

        soundPlayer.Init();
        soundPlayer.gameObject.SetActive(true);

        soundPlayer.PlayClip();
        soundPlayer.OnCompeleted += CompletedPlayer;
        Debug.Log(soundPlayer.name);
        return soundPlayer.AudioClipLength;
    }

    private void CompletedPlayer(SoundPlayer player)
    {
        int id = player.SoundID;
        if (soundPlayerPoolDictionary.ContainsKey(id) == false)
        {
            Debug.LogError("soundError");
            return;
        }

        player.Release();
        player.gameObject.SetActive(false);
        soundPlayerPoolDictionary[id].Enqueue(player);
        soundPlayerList.Remove(player);
    }

    private void ImmediatelyStop(int soundID)
    {
        var list = soundPlayerList.FindAll(x => x.SoundID == soundID);

        foreach (SoundPlayer player in list)
        {
            player.ImmediatelyStop();
            soundPlayerList.Remove(player);
        }
    }

    private void OnDestroy()
    {
        OnPlayBGMSound -= CreateBGMSoundPlayer;
        OnPlayEffectSound -= CreateEffectSoundPlayer;
        OnImmediatelyStop -= ImmediatelyStop;
    }
}
