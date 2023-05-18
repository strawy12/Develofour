using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    [SerializeField]
    private int playTime; //초 단위로 올라감

    private float timeCount;
    private bool isLoadingEnd;

    void Awake()
    {
        GameManager.Inst.OnStartCallback += () => { playTime = DataManager.Inst.GetCurrentTime(); isLoadingEnd = true; };

    }

    void FixedUpdate()
    {
        if (!isLoadingEnd) return;

        timeCount += Time.fixedDeltaTime;
        if(timeCount >= 1)
        {
            timeCount = 0;
            playTime += 1;
            DataManager.Inst.SetCurrentTime(playTime);
            TimeCount();
            
        }

    }

    private void TimeCount()
    {
        DateTime newDateTime = Constant.DEFAULTDATE.AddSeconds(playTime);

        EventManager.TriggerEvent(ETimeEvent.ChangeTime, new object[] { newDateTime });
    }
}
