using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfilerGuideButtonParent : MonoBehaviour
{
    [SerializeField]
    private Sprite profileSprite;

    [SerializeField]
    private ProfilerGuideButton guideButtonPrefab;

    private List<ProfilerGuideButton> guideButtonList;
    private List<ProfilerGuideDataSO> guideDataList;

    public void Init()
    {
        guideButtonList = new List<ProfilerGuideButton>();
        guideDataList = ResourceManager.Inst.GetResourceList<ProfilerGuideDataSO>();
        EventManager.StartListening(EProfilerEvent.AddGuideButton, AddGuideButton);
        SaveSetting();
    }
    #region ButtonSetting
    private void SaveSetting()
    {
        if (DataManager.Inst.GetIsClearTutorial())
        {
            AddGuideButton();
        }
    }

    private void AddGuideButton(object[] ps = null)
    {
        if (ps == null)
        {
            foreach (var data in guideDataList)
            {
                if (data.isAddTutorial == false && DataManager.Inst.CheckGuideButtonSaveData(data.guideName) == false)
                {
                    continue;
                }
                AddButton(data);
            }
        }
        else
        {
            if (!(ps[0] is string)) return;
            string name = ps[0] as string;
            AddButton(guideDataList.Find(x => x.guideName == name));
        }
    }

    public void AddButton(ProfilerGuideDataSO data)
    {
        ProfilerGuideButton button = guideButtonList.Find(x => x.GuideData == data);
        if (button != null)
        {
            return;
        }
        button = Instantiate(guideButtonPrefab, transform);
        button.Init(data);
        button.gameObject.SetActive(true);
        if (!DataManager.Inst.CheckGuideButtonSaveData(data.guideName))
        {
            string noticeBody = $"{data.guideName} ���̵� ��ư�� �߰��Ǿ����ϴ�. �������Ϸ��� ���̵� ��ư �г��� �� Ȯ���غ�����";
            NoticeSystem.OnNotice?.Invoke("���̵� ��ư�� �߰��Ǿ����ϴ�", noticeBody, 0.1f, false, profileSprite, Color.white, ENoticeTag.Profiler);
        }
        DataManager.Inst.AddGuideButtonSaveData(data.guideName);
        guideButtonList.Add(button);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.AddGuideButton, AddGuideButton);
    }
    #endregion
}