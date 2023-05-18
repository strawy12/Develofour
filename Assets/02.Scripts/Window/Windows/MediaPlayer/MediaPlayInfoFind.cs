using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MediaPlayer))]
public class MediaPlayInfoFind : MonoBehaviour
{
    //이건 다 봤을때 정보 트리거 있어야할때
    //다시 손봐야할듯
    private EProfileCategory category;
    private List<int> infoID = new List<int>();
    private void GetInfo()
    {
        foreach(var id in infoID)
        {
            EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, id, null });
        }

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
