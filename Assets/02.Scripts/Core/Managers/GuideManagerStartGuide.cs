using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GuideManager : MonoBehaviour
{
    private void StartGudie(EGuideTopicName guideTopic)
    {
        Debug.Log(guideTopic);
        switch (guideTopic)
        {
            case EGuideTopicName.None:
                break;
            case EGuideTopicName.GuestLoginGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(EMonologTextDataType.NotebookLoginGuideLog, 0.5f, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.LibraryOpenGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(EMonologTextDataType.GuideLog1, 0.2f, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.ClickPinNotePadHint:
                {
                    EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { guideTopicDictionary[guideTopic].guideTexts[0] });

                    DataManager.Inst.SetGuide(guideTopic, true);

                    break;
                }
            case EGuideTopicName.ClearPinNotePadQuiz: // 주글 핀 번호 힌트
                {
                    SendAiChattingGuide(guideTopicDictionary[guideTopic].guideTexts, 0.75f, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.Count:
                break;
            default:
                {
                    break;
                }
        }

        EndProfileGuide();
    }
}
