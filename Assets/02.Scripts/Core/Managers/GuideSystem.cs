﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class GuideSystem : MonoBehaviour
{
    public static Action<EGuideTopicName, float> OnPlayGuide;
    public static Action<ProfilerGuideDataSO> OnPlayInfoGuide;
    [SerializeField]
    private GuideDataListSO guideListData;

    private Dictionary<EGuideTopicName, GuideData> guideTopicDictionary;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        guideTopicDictionary = new Dictionary<EGuideTopicName, GuideData>();

        foreach (var guideData in guideListData.guideDataList)
        {
            guideTopicDictionary.Add(guideData.topicName, guideData);
        }

        OnPlayGuide += StartPlayGuide;
        EventManager.StartListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StartListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
    }

    private void StartPlayGuide(EGuideTopicName guideTopicName, float timer)
    {
        if (DataManager.Inst.IsGuideUse(guideTopicName))
        {
            return;
        }

        StartCoroutine(SetTimer(timer, guideTopicName));
    }

    private IEnumerator SetTimer(float timer, EGuideTopicName guideTopicName)
    {
        yield return new WaitForSeconds(timer);

        if (DataManager.Inst.IsGuideUse(guideTopicName))
        {
            yield break;
        }

        StartGudie(guideTopicName);
    }

    private void ThisClearGuideTopic(object[] ps)
    {
        if (ps[0] == null)
        {
            return;
        }

        EGuideTopicName eGuideTopic = (EGuideTopicName)ps[0];

        DataManager.Inst.SetGuide(eGuideTopic, true);
    }

    private void GuideConditionCheckClear(object[] ps)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }
        FileSO file = (FileSO)ps[0];

        if (file.ID == Constant.FileID.ZOOGLEPIN)
        {
            EventManager.TriggerEvent(EProfilerEvent.AddGuideButton, new object[1] { Constant.ZOOGLEPASSWORDGUIDE });
        }
    }

    //private void SendAiChattingGuide(string str, bool isSave)
    //{   
    //    ProfilerChattingSystem.OnPlayChat?.Invoke(str, isSave, false);
    //}

    //private void SendAiChattingGuide(string[] strList, float delay, bool isSave)
    //{
    //    ProfilerChattingSystem.OnPlayChatList?.Invoke(strList.ToList(), delay, isSave);
    //}

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StopListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
    }

}