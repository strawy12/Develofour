using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileFindInfoEvent : MonoBehaviour
{
    private void Start()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, FindInfoEvent);
    }

    private void FindInfoEvent(object[] ps)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];
        string key = ps[1] as string;

        if (ps[2] != null)
        {
            List<ProfileInfoTextDataSO> strList = ps[2] as List<ProfileInfoTextDataSO>;
            foreach (var temp in strList)
            {
                if (!DataManager.Inst.IsProfileInfoData(temp.category, temp.key))
                {
                    return;
                }
            }
            CheckEvent(category, key, strList);
        }


    }

    private void CheckEvent(EProfileCategory category, string key, List<ProfileInfoTextDataSO> strList = null) 
    {
        if(strList != null)
        {
            if (category == EProfileCategory.SuspectProfileExtensionInformation && key == Constant.ProfileInfoKey.SUSPECTRESIDENCE)
            {
                MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.SuspectResidence, 0.1f, true);
            }
            if(category == EProfileCategory.VictimProfileInformation && key == Constant.ProfileInfoKey.VICTIMUNIVERSITY)
            {
                MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.VictimUniversity, 0.1f, true);

            }
        }
    }
    
}
