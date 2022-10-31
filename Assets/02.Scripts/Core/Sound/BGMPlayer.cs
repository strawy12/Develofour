using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMPlayer : SoundPlayer
{
    [SerializeField]
    private Sound.EBgm bgmSoundID;

    public override void Init()
    {
        soundID = (int)bgmSoundID;
        base.Init();

        EventManager.StartListening(EEvent.ChangeBGM, CheckStopBGM);

        if(audioSource.loop == false)
        {
            audioSource.loop = true;
        }
    }

    private void CheckStopBGM(object soundID)
    {
        if (!(soundID is int) && (int)soundID != this.soundID) return;

        ImmediatelyStop();
    }

    public override void Release()
    {
        base.Release();
        EventManager.StopListening(EEvent.ChangeBGM, (x) => ImmediatelyStop());
    }

    private void Reset()
    {
        bgmSoundID = Sound.EBgm.None;
    }

    [ContextMenu("Refresh")]
    public void Refresh()
    {
        if(bgmSoundID != (Sound.EBgm)transform.GetSiblingIndex())
        {
            bgmSoundID = (Sound.EBgm)transform.GetSiblingIndex();
        }
    }
}
