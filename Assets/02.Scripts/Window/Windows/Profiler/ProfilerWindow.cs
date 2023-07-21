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
        EventManager.StartListening(ETutorialEvent.CheckTutorialState, CheckTutorialState);
        beforeClickButton = infoPanelBtn;

        ButtonBlackSetting();
    }

    private void CheckTutorialState(object[] obj)
    {
        if(!DataManager.Inst.GetIsClearTutorial())
        {
            Define.CheckTutorialState(this);
        }
    }

    private void ProfilerSelected()
    {
        if (!DataManager.Inst.GetIsClearTutorial())
        {
            Define.CheckTutorialState(this);
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
        EventManager.StopListening(ETutorialEvent.CheckTutorialState, CheckTutorialState);

    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
        EventManager.StopListening(EProfilerEvent.ClickGuideButton, OnClickShowChatting);
        EventManager.StopListening(ETutorialEvent.CheckTutorialState, CheckTutorialState);
    }

}
