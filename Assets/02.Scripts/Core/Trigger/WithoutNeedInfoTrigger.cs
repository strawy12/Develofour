using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WithoutNeedInfoTrigger : GetNeedInfomationTrigger
{

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (category == EProfileCategory.None)
        {
            MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
        }
        else
        {
            if (!DataManager.Inst.IsProfileInfoData(category, information))  // 찾은 정보인지 확인
            {
                if (needInformaitonList.Count == 0) // 찾는 조건이 없으면
                {
                    MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                    EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
                    OnPointerEnter(eventData);
                }
                else // 찾아야 하는 단어가 있으면
                {
                    foreach (ProfileInfoTextDataSO needData in needInformaitonList) // 리스트로 확인
                    {
                        if (!DataManager.Inst.IsProfileInfoData(needData.category, needData.key))
                        // 안 찾은게 있다면
                        {
                            MonologSystem.OnStartMonolog?.Invoke(notFinderNeedStringMonoLog, delay, true);
                            return;
                        }
                    }

                    // return 안되면 찾은거임
                    MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                    EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
                }
            }
        }
    }

}
