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

    [Header("Panels")]
    [SerializeField]
    private ProfilerUsingDocument profilerUsingDocuments;
    [SerializeField]
    private ProfilerPanel profilerPanel;
    [SerializeField]
    private ProfileGuidePanel profilerGuidePanel;
    [SerializeField]
    private ProfilerChatting profilerChatting;

    [Header("Buttons")]
    [SerializeField]
    private ProfilerPanelButton infoPanelBtn;
    [SerializeField]
    private ProfilerPanelButton aiChattingPanelBtn;

    [Header("UIEtc")]
    [SerializeField]
    private RectTransform area;

    //DataManager
    private ProfilerPanelButton beforeClickButton;

    protected override void Init()
    {
        base.Init();

        profilerChatting.Init();
        profilerPanel.Init();
        profilerUsingDocuments.Init();
        profilerGuidePanel.Init();
        OnSelected += ProfilerSelected;

        infoPanelBtn.AddListening(OnClickShowProfiling);
        aiChattingPanelBtn.AddListening(OnClickShowChatting);

        EventManager.StartListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
        EventManager.StartListening(EProfilerEvent.ClickGuideButton, OnClickShowChatting);
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
        if (profilerPanel.gameObject.activeSelf == false)
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

        ShowProfilePanel();
        ButtonBlackSetting();
    }
    private void OnClickShowChatting(object[] ps)
    {
        OnClickShowChatting();
    }
    private void OnClickShowChatting()
    {
        Debug.Log("ShowChatting");

        if (beforeClickButton == aiChattingPanelBtn)
        {
            return;
        }

        beforeClickButton = aiChattingPanelBtn;

        ShowChattingPanel();
        ButtonBlackSetting();
    }

    private void ButtonBlackSetting()
    {
        infoPanelBtn.Setting(beforeClickButton);
        aiChattingPanelBtn.Setting(beforeClickButton);
    }

    private void ShowProfilePanel()
    {
        HidePanel();
        profilerPanel.Show();
    }

    private void ShowChattingPanel()
    {
        HidePanel();
        profilerChatting.Show();
    }

    private void HidePanel()
    {
        profilerPanel.Hide();
        profilerChatting.Hide();
    }

    public override void WindowMinimum()
    {
        base.WindowMinimum();
        EventManager.TriggerEvent(EProfilerEvent.Minimum);
    }
    public override void WindowMaximum()
    {
        base.WindowMaximum();
        EventManager.TriggerEvent(EProfilerEvent.Maximum);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
        EventManager.StopListening(EProfilerEvent.ClickGuideButton, OnClickShowChatting);

    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
        EventManager.StopListening(EProfilerEvent.ClickGuideButton, OnClickShowChatting);
    }

}
