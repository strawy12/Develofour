using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Linq.Expressions;

public class ProfileWindow : Window
{
    [Header("ProfilerPanel")]
    [SerializeField]
    private ProfilePanel profilePanel;
    [SerializeField]
    private FileSearchPanel fileSearchPanel;

    [Header("ProfilerChatUI")]
    [SerializeField]
    private ProfileChatting profileChatting;

    [SerializeField]
    private ProfilePanelButton infoPanelBtn;
    [SerializeField]
    private ProfilePanelButton searchPanelBtn;

    [Header("UIPanelButton")]
    [SerializeField]
    private Button moveDownPanelBtn;
    [SerializeField]
    private Button movePopUpPanelBtn;

    [Header("UIEtc")]
    [SerializeField]
    private RectTransform area;

    [SerializeField]
    private float moveDelay = 0.75f;

    private bool isPanelOpen;
    private bool isMoving;

    //DataManager

    private ProfilePanelButton beforeClickButton;


    protected override void Init()
    {
        isPanelOpen = true;
        isMoving = false;

        base.Init();

        profileChatting.Init();
        profilePanel.Init();
        fileSearchPanel.Init();

        OnSelected += ProfilerSelected;

        infoPanelBtn.button.onClick?.AddListener(OnClickShowProfiling);
        searchPanelBtn.button.onClick?.AddListener(OnClickShowFileSearch);

        moveDownPanelBtn.onClick.AddListener(MoveButtonClick);
        movePopUpPanelBtn.onClick.AddListener(MoveButtonClick);

        EventManager.StartListening(EProfileSearchTutorialEvent.GuideSearchButton, GuideSearchButton);
        TutorialStart();
        EventManager.StartListening(EProfileEvent.FindInfoText, CheckProfilerOnOff);

        beforeClickButton = infoPanelBtn;

        ButtonBlackSetting();
    }

    private void ProfilerSelected()
    {
        if (DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
        {
            EventManager.TriggerEvent(EGuideButtonTutorialEvent.TutorialStart);
            OnSelected -= ProfilerSelected;
        }
    }

    private void CheckProfilerOnOff(object[] emptyPs)
    {
        if (profilePanel.gameObject.activeSelf == false)
        {
            OnClickShowProfiling();
        }
    }

    private void OnClickShowProfiling()
    {
        Debug.Log("ShowFileInfo");

        if (beforeClickButton == infoPanelBtn)
        {
            return;
        }

        beforeClickButton = infoPanelBtn;

        ButtonBlackSetting();
        StartCoroutine(OnShowProfile());
    }

    private void OnClickShowFileSearch()
    {
        if (!DataManager.Inst.GetIsStartTutorial(ETutorialType.Search))
        {
            ProfileChattingSystem.OnPlayChat(new TextData() { text = "아직은 열람할 수 없는 기능입니다.", color = new Color(255, 255, 255, 100) }, false, false);
            return;
        }

        if (beforeClickButton == searchPanelBtn)
        {
            return;
        }

        beforeClickButton = searchPanelBtn;

        ButtonBlackSetting();
        StartCoroutine(OnShowFileSearch());
    }

    private void ButtonBlackSetting()
    {
        infoPanelBtn.Setting(beforeClickButton);
        searchPanelBtn.Setting(beforeClickButton);
    }

    private void TutorialStart()
    {
        if (DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler) == false)
        {
            EventManager.TriggerEvent(ETutorialEvent.TutorialStart);
            EventManager.StartListening(ETutorialEvent.ProfileMidiumStart, StartGuideMinimumBtn);
            DataManager.Inst.SetIsStartTutorial(ETutorialType.Profiler, true);
        }
    }

    private void MoveButtonClick()
    {
        if (isPanelOpen)
        {
            area.DOAnchorPosY(-1000, moveDelay);

            movePopUpPanelBtn.gameObject.SetActive(true);
            moveDownPanelBtn.gameObject.SetActive(false);

            isPanelOpen = false;
        }
        else if (!isPanelOpen)
        {
            area.DOAnchorPosY(-50, moveDelay);

            moveDownPanelBtn.gameObject.SetActive(true);
            movePopUpPanelBtn.gameObject.SetActive(false);

            isPanelOpen = true;
        }
    }

    private IEnumerator HideAllPanel()
    {
        isMoving = true;

        area.DOAnchorPosY(-1000, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            profilePanel.gameObject.SetActive(false);
            fileSearchPanel.gameObject.SetActive(false);
        });

        yield return new WaitForSeconds(moveDelay + 0.05f);
        isMoving = false;
    }

    private IEnumerator OnShowProfile()
    {
        if (!isMoving)
        {
            isMoving = true;
            yield return StartCoroutine(HideAllPanel());
            ShowProfileCategoryPanel();
        }
    }

    private IEnumerator OnShowFileSearch()
    {
        if (!isMoving)
        {
            isMoving = true;
            yield return StartCoroutine(HideAllPanel());
            ShowFileSearchPanel();
        }
    }

    private void ShowProfileCategoryPanel()
    {
        ShowPanel(profilePanel.gameObject);
    }

    private void ShowFileSearchPanel()
    {
        ShowPanel(fileSearchPanel.gameObject);
    }

    private void ShowPanel(GameObject panel)
    {
        isMoving = true;
        panel.gameObject.SetActive(true);

        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isPanelOpen = true;
            isMoving = false;
            if (DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler) && DataManager.Inst.GetIsStartTutorial(ETutorialType.Search))
            {
                EventManager.TriggerEvent(EProfileSearchTutorialEvent.ClickSearchButton);
            }
        });
    }

    private void StartGuideMinimumBtn(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke((RectTransform)windowBar.MinimumBtn.transform);
        EventManager.StartListening(ETutorialEvent.ProfileMidiumEnd, TutotrialClickMinimumBtn);
    }
    private void TutotrialClickMinimumBtn(object[] ps)
    {
        GuideUISystem.EndGuide?.Invoke();
        EventManager.StopListening(ETutorialEvent.ProfileMidiumEnd, TutotrialClickMinimumBtn);
        EventManager.StopListening(ETutorialEvent.ProfileMidiumStart, StartGuideMinimumBtn);
        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart);
    }
    public override void WindowMinimum()
    {
        base.WindowMinimum();

        EventManager.TriggerEvent(ETutorialEvent.ProfileMidiumEnd);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, CheckProfilerOnOff);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EProfileEvent.FindInfoText, CheckProfilerOnOff);
    }
    private void GuideSearchButton(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke(searchPanelBtn.RectTrm);
    }
}
