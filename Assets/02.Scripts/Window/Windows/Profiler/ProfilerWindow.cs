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
    [SerializeField]
    private ProfilerCallingPanel profilerCallingPanel;

    //[SerializeField]
    //private ProfilerAIChatAlarm profilerAlarm;

    [Header("Buttons")]
    [SerializeField]
    private ProfilerPanelButton infoPanelBtn;
    [SerializeField]
    private ProfilerPanelButton callPanelBtn;

    [Header("UIEtc")]
    [SerializeField]
    private RectTransform area;

    //DataManager
    private ProfilerPanelButton beforeClickButton;

    public static ProfilerWindow CurrentProfiler;

    protected override void Init()
    {
        base.Init();

        profilerChatting.Init();
        profilerPanel.Init();
        profilerUsingDocuments.Init();
        profilerGuidePanel.Init();
        profilerCallingPanel.Init();
        //profilerAlarm.Init();

        #region 튜토리얼
        EventManager.StartListening(ETutorialEvent.CheckTutorialState, CheckTutorialState);
        OnSelected += CheckChattingPanel;
        OnSelected += CheckIsNewChatImage;
        #endregion

        infoPanelBtn.AddListening(OnClickShowProfiling);
        callPanelBtn.AddListening(OnClickShowCalling);
        //EventManager.StartListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);

        beforeClickButton = infoPanelBtn;

        ButtonBlackSetting();
    }

    #region 튜토리얼

    private void CheckChattingPanel()
    {
        if(profilerChatting.isActiveAndEnabled)
        {
            ProfilerChattingSelected();
            //profilerAlarm.CloseAlarm();
        }
    }

    private void CheckIsNewChatImage()
    {
        if (profilerChatting.isActiveAndEnabled)
        {
            if (profilerChatting.isUsingNewImage)
            {
                profilerChatting.isUsingNewImage = false;
            }
        }

    }

    private void CheckTutorialState(object[] obj)
    {
        ProfilerChattingSelected();
    }

    private void ProfilerChattingSelected()
    {
        //if (!DataManager.Inst.IsClearTutorial())
        //{
        //    Define.CheckTutorialState(this);
        //}
    }

    #endregion

    //private void CheckProfilerOnOff(object[] emptyPs)
    //{
    //    if (profilerPanel.gameObject.activeSelf == false)
    //    {
    //        OnClickShowProfiling();
    //    }
    //}

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

    private void OnClickShowCalling()
    {
        if (beforeClickButton == callPanelBtn)
        {
            return;
        }

        beforeClickButton = callPanelBtn;
        ShowCallingPanel();
        ButtonBlackSetting();
    }
    private void ButtonBlackSetting()
    {
        infoPanelBtn.Setting(beforeClickButton);
    }

    private void ShowProfilePanel()
    {
        HidePanel();
        profilerPanel.Show();
    }
    private void ShowCallingPanel()
    {
        HidePanel();
        profilerCallingPanel.Show();
    }
    private void ShowChattingPanel()
    {
        HidePanel();
    }

    private void HidePanel()
    {
        profilerPanel.Hide();
        profilerCallingPanel.Hide();
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
        HidePanel();
        //EventManager.StopListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
        EventManager.StopListening(ETutorialEvent.CheckTutorialState, CheckTutorialState);

    }

    private void OnApplicationQuit()
    {
        HidePanel();
        //EventManager.StopListening(EProfilerEvent.FindInfoText, CheckProfilerOnOff);
        EventManager.StopListening(ETutorialEvent.CheckTutorialState, CheckTutorialState);
    }

}
