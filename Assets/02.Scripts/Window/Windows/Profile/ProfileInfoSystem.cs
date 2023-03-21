using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileInfoSystem : MonoBehaviour
{
    [SerializeField]
    private Sprite profileSprite;

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoList = new Dictionary<EProfileCategory, ProfileCategoryDataSO>();

    private IEnumerator Start()
    {
        Debug.Log("나중에 Start Callback 으로 만듭시다");
        yield return new WaitForSeconds(3f);

        infoList = ResourceManager.Inst.GetProfileCategoryDataList();
        Debug.Log(infoList.Count);
        foreach(var info in infoList)
        {
            Debug.Log($"{info.Key}");
        }

        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
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
            DataManager.Inst.AddProfileinfoData(category, str);
            SendAlarm(category, str);

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
    public void SendAlarm(EProfileCategory category, string key)
    {

        string answer;
        string temp = "nullError";
        ProfileCategoryDataSO categoryData = infoList[category];
        foreach (var infoText in categoryData.infoTextList)
        {
            if (key == infoText.key)
            {
                answer = infoText.key;
                temp = answer.Replace(": ", "");
            }
        }
        string text = categoryData.categoryTitle + " 카테고리의 " + temp + "정보가 업데이트 되었습니다.";
        NoticeSystem.OnNotice.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profileSprite,Color.white,  ENoticeTag.Profiler);
    }
}
