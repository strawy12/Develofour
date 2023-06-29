using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilerGuideButton : MonoBehaviour
{
    private ProfilerGuideDataSO guideData;
    public ProfilerGuideDataSO GuideData
    {
        get
        {
            return guideData;
        }
    }
    [SerializeField]
    private Button guideBtn;
    [SerializeField]
    private TMP_Text infoNameText;
    public Button.ButtonClickedEvent OnClick
    {
        get => guideBtn.onClick;
    }
    private bool isGuide = false;
    
    public void Init(ProfilerGuideDataSO data)
    {
        guideData = data;
        guideBtn.onClick.AddListener(PlayGuide);
        infoNameText.text = data.guideName;
        EventManager.StartListening(EProfilerEvent.EndGuide, EndGuide);
    }

    private void PlayGuide()
    {
        EventManager.TriggerEvent(EProfilerEvent.ClickGuideButton);

        if (isGuide)
        {
            return;
        }
        isGuide = true;
        StartAiChatting();
    }

    private void StartAiChatting()
    {
        ProfilerChattingSystem.OnPlayChatList?.Invoke(guideData.guideTextList, 1.5f, true);
    } 

    private void EndGuide(object[] ps)
    {
        isGuide = false;
        EventManager.TriggerEvent(EGuideButtonTutorialEvent.ClickAnyBtn);
    }

    public void Releasse()
    {
        guideData = null;
        infoNameText.text = "";
        EventManager.StopListening(EProfilerEvent.EndGuide, EndGuide);
    }
}
