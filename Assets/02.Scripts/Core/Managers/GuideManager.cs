using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GuideManager : MonoBehaviour
{
    public static Action<EGuideTopicName, float> OnPlayGuide;
    public static Action<EGuideTopicName> OnPlayInfoGuide;
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
        OnPlayInfoGuide += StartProfileInfoGuide;
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

    private void StartProfileInfoGuide(EGuideTopicName guideTopic)
    {
        StartGudie(guideTopic);
    }

    private void EndProfileGuide()
    {
        EventManager.TriggerEvent(EProfileEvent.EndGuide);
    }

    private IEnumerator SendAiMessageTexts(string[] values)
    {
        foreach (string str in values)
        {
            Debug.Log(str);
            EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { str });
            yield return new WaitForSeconds(1f);
        }
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
        FileSO file = (FileSO)ps[0];
        bool isZooglePinHintNoteOpen = DataManager.Inst.SaveData.isZooglePinHintNoteOpen;

        string fileLocation = file.GetFileLocation();

        if (fileLocation == "User\\C\\내 문서\\Zoogle\\ZooglePIN번호\\")
        {
            Debug.Log("비번 가이드 조건 충족");
            OnPlayGuide(EGuideTopicName.ClearPinNotePadQuiz, 40);
        }
    }
    private void SendProfileGuide(EGuideTopicName topicName)
    {
        string temp = topicName.ToString();
        EAIChattingTextDataType textType = Enum.Parse<EAIChattingTextDataType>(temp);
        ProfileChattingSystem.OnChatEnd += EndProfileGuide;
        EventManager.TriggerEvent(EProfileEvent.SendGuide, new object[1] { textType });
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StopListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
    }

}