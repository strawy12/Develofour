using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EGuideType
{
    None,
    ProfilerDownGuide,
    BrowserConnectGuide,
    ClickPinNotePadHint,
    ClearPinNotePadQuiz,
}

[System.Serializable]
public class GuideStatusData
{
    public bool isCompleted;
    public EGuideType guideTopic;
}

public class GuideManager : MonoSingleton<GuideManager>
{
    public bool isZooglePinNotePadOpenCheck = false;

    public Dictionary<EGuideType, bool> guidesDictionary;
    
    [SerializeField]
    private List<GuideStatusData> guideStatusesList;

    [SerializeField]
    private string browserConnectGuideHint;
    [SerializeField]
    private string notePadHintClickChatting;
    [SerializeField]
    private string[] pinNotePadHintChatting;

    private EGuideType guideType;

    void Start()
    {
        guidesDictionary = new Dictionary<EGuideType, bool>();

        Init();
    }

    private void Init()
    {
        EventManager.StartListening(ECoreEvent.OpenPlayGuide, OnPlayGuide);

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
        guideType = (EGuideType)ps[1];

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

    private void StartGudie(EGuideType guideTopic)
    {
        switch(guideTopic)
        {
            case EGuideType.ProfilerDownGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(ETextDataType.GuideLog1, 0.2f, 1);
                    guidesDictionary[guideType] = true;

                    break;
                }
            case EGuideType.BrowserConnectGuide:
                {
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { browserConnectGuideHint });
                    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
                    guidesDictionary[guideType] = true;
                    
                    break;
                }
            case EGuideType.ClickPinNotePadHint:
                {
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { notePadHintClickChatting });
                    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
                    guidesDictionary[guideType] = true;

                    break;
                }
            case EGuideType.ClearPinNotePadQuiz:
                {
                    StartCoroutine("SendHintMessage");
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

        }
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 1f);
    }

    private void OnApplicationQuit()
    {
        isZooglePinNotePadOpenCheck = false;

        EventManager.StopListening(ECoreEvent.OpenPlayGuide, OnPlayGuide);
    }
}
