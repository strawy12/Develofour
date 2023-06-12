using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GuideManager : MonoBehaviour
{
    private void StartGudie(EGuideTopicName guideTopic)
    {
        switch (guideTopic)
        {
            case EGuideTopicName.LibraryOpenGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(Constant.MonologKey.LIBRARY_NOT_OPEN, 0.2f, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.FirstLoginGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(Constant.MonologKey.FIRST_LOGIN_GUIDE, 0.1f, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            //case EGuideTopicName.ClickPinNotePadHint:
            //    {
            //        EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[1] { guideTopicDictionary[guideTopic].guideTexts[0] });

            //        DataManager.Inst.SetGuide(guideTopic, true);

            //        break;
            //    }
            //case EGuideTopicName.ClearPinNotePadQuiz: // 주글 핀 번호 힌트
            //    {
            //        SendAiChattingGuide(guideTopicDictionary[guideTopic].guideTexts, 0.75f, true);
            //        DataManager.Inst.SetGuide(guideTopic, true);
            //        break;
            //    }
            default:
                {
                    break;
                }
        }
    }
}
