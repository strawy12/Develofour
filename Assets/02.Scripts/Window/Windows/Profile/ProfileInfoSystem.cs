using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileInfoSystem : MonoBehaviour
{
    [SerializeField]
    private Sprite profileSprite;

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoList = new Dictionary<EProfileCategory, ProfileCategoryDataSO>();

    private void Start()
    {
        GameManager.Inst.OnStartCallback += StartCallback;
    }

    private void StartCallback()
    {
        infoList = ResourceManager.Inst.GetProfileCategoryDataList();
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
    }

    private void ChangeValue(object[] ps) // string 값으로 들고옴
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

        if(DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler) && !DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
        {
            if(key != Constant.ProfileInfoKey.SUSPECTNAME)
            {
                return;
            }
        }


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
        }

        if (!DataManager.Inst.IsProfileInfoData(category, key))
        {
            DataManager.Inst.AddProfileinfoData(category, key);
            SendAlarm(category, key);
        }
        else
        {
            Debug.Log("이미 찾은 정보입니다");
            return;
        }

        if (!DataManager.Inst.GetProfileSaveData(category).isShowCategory)
        {
            DataManager.Inst.SetCategoryData(category, true);
        }
    }

    public void SendAlarm(EProfileCategory category, string key)
    {
        string answer;
        string temp = "nullError";
        ProfileCategoryDataSO categoryData = infoList[category];
        foreach (var infoText in categoryData.infoTextList)
        {
            if (key == infoText.key)
            {
                temp = infoText.infoName;
            }
        }
        string text;
        if (category != EProfileCategory.InvisibleInformation)
        {
            text = categoryData.categoryTitle + " 카테고리의 " + temp + "정보가 업데이트 되었습니다.";
            NoticeSystem.OnNotice.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profileSprite, Color.white, ENoticeTag.Profiler);
        }
        else
        {
            text = temp + " 정보가 확인되었습니다.";
            NoticeSystem.OnNotice.Invoke("Profiler 정보가 확인되었습니다!", text, 0, true, profileSprite, Color.white, ENoticeTag.Profiler);
        }
        
    }
}
