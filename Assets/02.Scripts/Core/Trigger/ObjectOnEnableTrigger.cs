using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnEnableTrigger : MonoBehaviour
{
    [SerializeField] protected ProfileInfoTextDataSO infomaitionData; //해금되는 정보
    [SerializeField] protected List<ProfileInfoTextDataSO> needInformaitonList;
    [SerializeField] protected List<ProfileInfoTextDataSO> linkInformaitonList;
    public int monoLogType;


    //디버그 안해본 코드. 오류 가능성 있음.

    private void OnEnable()
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall) { return; }

        if (infomaitionData == null || infomaitionData.category == EProfileCategory.None)
        {
            MonologSystem.OnStartMonolog?.Invoke(monoLogType, 0, true);
            return;
        }

        if (!DataManager.Inst.IsProfileInfoData(infomaitionData.category, infomaitionData.key))
        {
            if (needInformaitonList.Count == 0)
            {
                GetInfo();
                return;
            }
            else
            {
                foreach (ProfileInfoTextDataSO needData in needInformaitonList)
                {
                    if (!DataManager.Inst.IsProfileInfoData(needData.category, needData.key))
                    {
                        if (monoLogType == -1)
                            return;
                        MonologSystem.OnStartMonolog?.Invoke(monoLogType, 0, true);
                        return;
                    }
                }
                GetInfo();
                return;
            }
        }
    }

    public void GetInfo()
    {
        MonologSystem.OnStartMonolog?.Invoke(monoLogType, 0, true);
        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[2] { infomaitionData.category, infomaitionData.key });
        TriggerList.CheckLinkInfos();
    }
}
