using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MediaPlayer))]
public class MediaPlayInfoFind : MonoBehaviour
{
    private EProfileCategory category;
    private int infoID;
    private void GetInfo()
    {
        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, infoID, null });
    }

    public void Init(MediaPlayer mediaPlayer)
    {
        if (mediaPlayer == null)
            return;

        if (mediaPlayer.mediaPlayerData.category != EProfileCategory.None)
        {
            category = mediaPlayer.mediaPlayerData.category;
            infoID = mediaPlayer.mediaPlayerData.infoID;
            mediaPlayer.OnEnd -= GetInfo;
            mediaPlayer.OnEnd += GetInfo;
        }
    }
}
