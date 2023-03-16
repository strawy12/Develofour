using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EGuideTopicName
{
    None,
    ProfilerDownGuide,
    ClickPinNotePadHint,
    ClearPinNotePadQuiz,
    Count
}


public class GuideManager: MonoBehaviour
{

    [SerializeField]
    private GuideDataListSO guideListData;

    private Dictionary<EGuideTopicName, GuideData> guideTopicDictionary;

    private EGuideTopicName guideType;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        EventManager.StartListening(EGuideEventType.OpenPlayGuide, OnPlayGuide);
        EventManager.StartListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StartListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
        guideTopicDictionary = new Dictionary<EGuideTopicName, GuideData>();

        foreach (var guide in guideListData.guideDataList)
        {
            guideTopicDictionary.Add(guide.topicName, guide);
        }
    }


    private void OnPlayGuide(object[] ps)
    {
        if (ps[0] == null)
        {
            return;
        }

        guideType = (EGuideTopicName)ps[0];

        if (DataManager.Inst.IsGuideUse(guideType))
        {
            return;
        }

        StartCoroutine(SetTimer(guideTopicDictionary[guideType].timer));
    }

    private IEnumerator SetTimer(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (DataManager.Inst.IsGuideUse(guideType))
        {
            yield break;
        }
        StartGudie(guideType);
    }

    private void StartGudie(EGuideTopicName guideTopic)
    {
        switch (guideTopic)
        {
            case EGuideTopicName.ProfilerDownGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(ETextDataType.GuideLog1, 0.2f, 1);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.ClickPinNotePadHint:
                {
                    EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { guideTopicDictionary[guideType].guideTexts[0] });
                    DataManager.Inst.SetGuide(guideTopic, true);


                    break;
                }
            case EGuideTopicName.ClearPinNotePadQuiz:
                {
                    StartCoroutine(SendAiMessageTexts(guideTopicDictionary[guideType].guideTexts));
                    DataManager.Inst.SetGuide(guideTopic, true);

                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private IEnumerator SendAiMessageTexts(string[] values)
    {
        foreach (string str in values)
        {
            EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
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

        OnPlayGuide(new object[1] { eGuideTopic });
    }
    private void GuideConditionCheckClear(object[] ps)
    {

        FileSO file = (FileSO)ps[0];
        bool isZooglePinHintNoteOpen = DataManager.Inst.SaveData.isZooglePinHintNoteOpen;

        string fileName = file.fileName;

        if (fileName == "ZooglePassword")
        {
            if (!isZooglePinHintNoteOpen)
            {
                DataManager.Inst.SaveData.isZooglePinHintNoteOpen = true;

                OnPlayGuide(new object[1] { EGuideTopicName.ClearPinNotePadQuiz });
            }
            else if (isZooglePinHintNoteOpen)
            {
                DataManager.Inst.SetGuide(EGuideTopicName.ClearPinNotePadQuiz, true);
            }
            if (fileName == "ZooglePIN번호" && isZooglePinHintNoteOpen)
            {
                DataManager.Inst.SetGuide(EGuideTopicName.ClickPinNotePadHint, true);
                OnPlayGuide(new object[1] { EGuideTopicName.ClearPinNotePadQuiz });
            }
        }
    }
    private void OnApplicationQuit()
    {
        EventManager.StopListening(EGuideEventType.OpenPlayGuide, OnPlayGuide);
        EventManager.StopListening(EGuideEventType.ClearGuideType, ThisClearGuideTopic);
        EventManager.StopListening(EGuideEventType.GuideConditionCheck, GuideConditionCheckClear);
    }
}
