using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileInfoSystem : MonoBehaviour
{

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoList;

    private void Awake()
    {
        infoList =  ResourceManager.Inst.GetProfileCategoryDataList();
    }

    private void ChangeValue(object[] ps) // string 값으로 들고옴
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];
        string str = ps[1] as string;

        if (ps[2] != null)
        {
            List<string> strList = ps[2] as List<string>;
            foreach (var temp in strList)
            {
                if (!DataManager.Inst.IsProfileInfoData(category, temp))
                {
                    return;
                }
            }
        }

        ProfileCategoryDataSO categoryData = infoList[category];

        if (!DataManager.Inst.IsProfileInfoData(category, str))
        {
            DataManager.Inst.AddProfileinfoData(category,str);
   
        }
        else
        {
            return;
        }

        if (!DataManager.Inst.GetProfileSaveData(category).isShowCategory)
        {
            DataManager.Inst.SetCategoryData(category, true);
        }

    }
    public void FindAlarm(string category, string key)
    {
        EventManager.TriggerEvent(ENoticeEvent.GeneratedProfileFindNotice, new object[2] { category, key });
    }

}
