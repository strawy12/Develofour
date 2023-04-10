using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EProfileCategory
{
    None,
    SuspectProfileInformation,
    SuspectProfileExtensionInformation,
    VictimProfileInformation,
    PetInformation,
    MurderEvidence,
    MurderTrigger,
    InvisibleInformation,
    Count,
}

public class ProfilePanel : MonoBehaviour
{
    [SerializeField]
    private List<ProfileInfoPanel> infoPanelList = new List<ProfileInfoPanel>();

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoList;

    [SerializeField]
    private Sprite profilerSpeite;

    public void Init()
    {
        infoList = ResourceManager.Inst.GetProfileCategoryDataList();

        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);

        Debug.Log(infoList.Count);
        foreach (var info in infoList)
        {
            foreach(var infoPanel in infoPanelList)
            {
                if(infoPanel.category == info.Key)
                {
                    infoPanel.Init(info.Value);
                }
            }
        }
    }

    //이벤트 매니저 등록
    private void ChangeValue(object[] ps) // string 값으로 들고옴
    {
        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];

        string key = ps[1] as string;

        if (ps[2] != null)
        {
            List<ProfileInfoTextDataSO> strList = ps[2] as List<ProfileInfoTextDataSO>;
            foreach(var temp in strList)
            {
                if (!DataManager.Inst.IsProfileInfoData(temp.category, temp.key))
                {
                    return;
                }
            }
        }

        if (category == EProfileCategory.InvisibleInformation)
        {
            return;
        }

        foreach(var infoPanel in infoPanelList)
        {
            if(infoPanel.category == category)
            {
                Debug.Log($"{infoPanel.gameObject.name}");

                infoPanel.ShowPost();
                infoPanel.ChangeValue(key);
            }
        }
    }

    public void SendAlarm(object[] ps)
    {
        if (!(ps[0] is string) || !(ps[1] is string))
        {
            return;
        }

        string key = ps[1] as string;
        string answer;
        string temp = "nullError";
        foreach (var infoPanel in infoPanelList)
        {
            answer = infoPanel.SetInfoText(key);

            if (string.IsNullOrEmpty(answer) == false)
            {
                temp = answer.Replace(": ", "");
            }
        }



        string text = ps[0] as string + " 카테고리의 " + temp + "정보가 업데이트 되었습니다.";
        NoticeSystem.OnNotice?.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profilerSpeite, Color.white, ENoticeTag.Profiler);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, ChangeValue);
    }
}
