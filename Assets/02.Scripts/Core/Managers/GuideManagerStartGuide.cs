using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GuideSystem : MonoBehaviour
{
    private void StartGudie(EGuideTopicName guideTopic)
    {
        switch (guideTopic)
        {
            case EGuideTopicName.LibraryOpenGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(Constant.MonologKey.LIBRARY_NOT_OPEN, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            case EGuideTopicName.FirstLoginGuide:
                {
                    MonologSystem.OnStartMonolog.Invoke(Constant.MonologKey.Lock_HINT, true);
                    DataManager.Inst.SetGuide(guideTopic, true);
                    break;
                }
            default:
                {
                    break;
                }
        }
    }
}
