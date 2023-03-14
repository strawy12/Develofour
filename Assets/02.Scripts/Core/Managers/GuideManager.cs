using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EGuideTopicName
{
    None,
    ProfilerDownGuide,
    BrowserConnectGuide,
    ClickPinNotePadHint,
    ClearPinNotePadQuiz,
}

public enum EGuideEventType
{
    OpenPlayGuide,
    ClearGuideType,
    GuideConditionCheck,
}

[System.Serializable]
public class GuideStatusData
{
    public bool isCompleted;
    public EGuideTopicName guideTopic;
}

public class GuideManager : MonoSingleton<GuideManager>
{
    public bool isZooglePinNotePadOpenCheck = false;

    public Dictionary<EGuideTopicName, bool> guidesDictionary;
    
    [SerializeField]
    private List<GuideStatusData> guideStatusesList;

    [SerializeField]
    private string browserConnectGuideHint;
    [SerializeField]
    private string notePadHintClickChatting;
    [SerializeField]
    private string[] pinNotePadHintChatting;

    private EGuideTopicName guideType;

    void Start()
    {
        guidesDictionary = new Dictionary<EGuideTopicName, bool>();

        Init();
    }

    private void Init()
    {
        EventManager.StartListening(EGuideEventType.OpenPlayGuide, OnPlayGuide);
        EventManager.StartListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StartListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);

        DictionaryInit();
    }

    private void DictionaryInit()
    {
        foreach (GuideStatusData guideStatus in guideStatusesList)
        {
            guidesDictionary.Add(guideStatus.guideTopic, guideStatus.isCompleted);
        }
    }    

    private void OnPlayGuide(object[] ps)
    {
        if (ps[0] == null || ps[1] == null)
        {
            return;
        }

        float timer = (float)ps[0];
        guideType = (EGuideTopicName)ps[1];

        StartCoroutine(SetTimer(timer));
    }

    private IEnumerator SetTimer(float timer)
    {
        yield return new WaitForSeconds(timer);

        if (!guidesDictionary[guideType]) // 완료되어 있지 않다면
        {
            StartGudie(guideType);
        }
    }

    private void StartGudie(EGuideTopicName guideTopic)
    {
        switch(guideTopic)
        {
            case EGuideTopicName.ProfilerDownGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(ETextDataType.GuideLog1, 0.2f, 1);
                    guidesDictionary[guideType] = true;

                    break;
                }
            case EGuideTopicName.BrowserConnectGuide:
                {
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { browserConnectGuideHint });
                    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
                    guidesDictionary[guideType] = true;
                    
                    break;
                }
            case EGuideTopicName.ClickPinNotePadHint:
                {
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { notePadHintClickChatting });
                    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
                    guidesDictionary[guideType] = true;

                    break;
                }
            case EGuideTopicName.ClearPinNotePadQuiz:
                {
                    StartCoroutine(SendHintMessage());
                    guidesDictionary[guideType] = true;

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private IEnumerator SendHintMessage()
    {
        foreach (string str in pinNotePadHintChatting)
        {
            EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
            yield return new WaitForSeconds(1f);

            NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 1f);
        }
    }

    private void ThisClearGuideTopic(object[] ps)
    {
        if (ps[0] == null) 
        {
            return;
        }

        EGuideTopicName eGuideTopic = (EGuideTopicName)ps[0];

        guidesDictionary[eGuideTopic] = true;
    }


    private void GuideConditionCheckClear(object[] ps)
    {
        string fileName = ps[0].ToString();
        EGuideTopicName guideType = (EGuideTopicName)ps[1];

        if(fileName == "ZooglePassword")
        {
            if(!isZooglePinNotePadOpenCheck) 
            {
                isZooglePinNotePadOpenCheck = true;

                EventManager.TriggerEvent(EGuideEventType.OpenPlayGuide, new object[2] { 1200f, guideType });
            }
            else if(isZooglePinNotePadOpenCheck)
            {
                guidesDictionary[guideType] = true;
            }
        }

        if (fileName == "Zoogle PIN번호" && isZooglePinNotePadOpenCheck)
        {
            guidesDictionary[EGuideTopicName.ClickPinNotePadHint] = true;

            EventManager.TriggerEvent(EGuideEventType.OpenPlayGuide, new object[2] { 40f, guideType });
        }
    }

    private void OnApplicationQuit()
    {
        isZooglePinNotePadOpenCheck = false;

        EventManager.StopListening(EGuideEventType.OpenPlayGuide, OnPlayGuide);
        EventManager.StopListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StopListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
    }
}
