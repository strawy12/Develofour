using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System;
using System.Linq.Expressions;

public class ProfilerWindow : Window
{
    [Header("ProfilerPanel")]
    [SerializeField]
    private ProfilerPanel profilePanel;
    //[SerializeField]
    //private FileSearchPanel fileSearchPanel;
    [SerializeField]
    private ProfilerUsingDocument profilerUsingDocuments;

    [Header("ProfilerChatUI")]
    [SerializeField]
    private ProfilerChatting profileChatting;

    [SerializeField]
    private ProfilerPanelButton infoPanelBtn;
    [SerializeField]
    private ProfilerPanelButton searchPanelBtn;

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
    private ProfilerPanelButton beforeClickButton;

    protected override void Init()
    {
        isPanelOpen = true;
        isMoving = false;

        base.Init();

        profileChatting.Init();
        profilePanel.Init();
        profilerUsingDocuments.Init();
        //fileSearchPanel.Init();

        OnSelected += ProfilerSelected;

        infoPanelBtn.button.onClick?.AddListener(OnClickShowProfiling);
        //searchPanelBtn.button.onClick?.AddListener(OnClickShowFileSearch);

        moveDownPanelBtn.onClick.AddListener(MoveButtonClick);
        movePopUpPanelBtn.onClick.AddListener(MoveButtonClick);

        //EventManager.StartListening(ETutorialEvent.SearchBtnGuide, GuideSearchButton);
        EventManager.StartListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);


        beforeClickButton = infoPanelBtn;

        ButtonBlackSetting();
    }

    private void ProfilerSelected()
    {
        if (DataManager.Inst.GetIsClearTutorial())
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
            profilePanel.Hide();
            //fileSearchPanel.gameObject.SetActive(false);
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
    public override void WindowMaximum()
    {
        base.WindowMaximum();
        EventManager.TriggerEvent(EProfilerEvent.Maximum);
    }
    private void ShowProfileCategoryPanel()
    {
        ShowPanel();
        profilePanel.Show();
    }

    private void ShowFileSearchPanel()
    {
        ShowPanel();
        //fileSearchPanel.gameObject.SetActive(true);
        //EventManager.TriggerEvent(ETutorialEvent.ClickSearchBtn);
    }

    private void ShowPanel()
    {
        isMoving = true;

        area.DOAnchorPosY(-50, moveDelay).SetEase(Ease.Linear).OnComplete(() =>
        {
            isPanelOpen = true;
            isMoving = false;

        });
    }



    public override void WindowMinimum()
    {
        base.WindowMinimum();
        EventManager.TriggerEvent(EProfilerEvent.Minimum);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
    }
    private void GuideSearchButton(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke(searchPanelBtn.RectTrm);
    }
}
