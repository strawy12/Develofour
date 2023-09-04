using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ProfilerGuideButtonParent : MonoBehaviour
{
    public Action OnClickGuideButton;

    [SerializeField]
    private Sprite profileSprite;

    [SerializeField]
    private ProfilerGuideButton guideButtonPrefab;

    private List<ProfilerGuideButton> guideButtonList = new List<ProfilerGuideButton>();
    private List<ProfilerGuideDataSO> guideDataList = new List<ProfilerGuideDataSO>();

    public bool isWeightSizeUp;
    [SerializeField]
    private Button nextBtn;
    [SerializeField]
    private Button prevBtn;
    private int currentIndex = 0;
    public void Init()
    {
        guideButtonList = new List<ProfilerGuideButton>();
        guideDataList = ResourceManager.Inst.GetResourceList<ProfilerGuideDataSO>();
        EventManager.StartListening(EProfilerEvent.AddGuideButton, AddGuideButton);
        prevBtn.onClick.AddListener(ClickPrev);
        nextBtn.onClick.AddListener(ClickNext);
        SaveSetting();
    }
    #region ButtonSetting
    private void SaveSetting()
    {
        if (DataManager.Inst.IsClearTutorial())
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
                if (data.isAddGuideButton == false && DataManager.Inst.CheckGuideButtonSaveData(data.guideName) == false)
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
            string noticeBody = $"{data.guideName} 가이드 버튼이 추가되었습니다. 프로파일러의 가이드 버튼 패널을 들어가 확인해보세요";
            NoticeSystem.OnNotice?.Invoke("가이드 버튼이 추가되었습니다", noticeBody, 0.1f, false, profileSprite, Color.white, ENoticeTag.Profiler, Constant.FileID.PROFILER);
        }
        DataManager.Inst.AddGuideButtonSaveData(data.guideName);
        guideButtonList.Add(button);
    }

    public void SetActiveBtn(bool value)
    {
        nextBtn.gameObject.SetActive(value);
        prevBtn.gameObject.SetActive(value);
    }
    #region Page
    public void UpdateButton()
    {
        if (guideButtonList.Count == 0)
        {
            return;
        }

        if (currentIndex >= guideButtonList.Count)
        {
            currentIndex = currentIndex <= 0 ? 0 : guideButtonList.Count - 1;
        }

        if (isWeightSizeUp == false)
        {
            for (int i = 0; i < guideButtonList.Count; i++)
            {
                guideButtonList[i].gameObject.SetActive(false);
            }
            if (guideButtonList.Count != 0)
            {
                guideButtonList[currentIndex].gameObject.SetActive(true);
            }
        }
        else
        {
            for (int i = 0; i < guideButtonList.Count; i++)
            {
                guideButtonList[i].gameObject.SetActive(false);
            }
            guideButtonList[currentIndex].gameObject.SetActive(true);
            int tempIndex = currentIndex;
            for (int i = 0; i < 2; i++)
            {
                tempIndex++;
                if (tempIndex >= guideButtonList.Count)
                {
                    break;
                }
                guideButtonList[tempIndex].gameObject.SetActive(true);
            }
        }
    }
    private void ClickNext()
    {
        if (currentIndex + 1 >= guideButtonList.Count)
        {
            return;
        }
        if (isWeightSizeUp)
        {
            currentIndex += 3;
            if (currentIndex >= guideButtonList.Count)
            {
                int value = (currentIndex) % 3;
                currentIndex = currentIndex - (3 - value);
            }
        }
        else
        {
            currentIndex++;
        }


        UpdateButton();
    }
    private void ClickPrev()
    {
        if (currentIndex - 1 < 0)
        {
            return;
        }
        if (isWeightSizeUp)
        {
            currentIndex -= 3;
            if (currentIndex < 0)
            {
                currentIndex = 0;
            }
        }
        else
        {
            currentIndex--;

        }

        UpdateButton();
    }
    #endregion

    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.AddGuideButton, AddGuideButton);
    }
    #endregion
}