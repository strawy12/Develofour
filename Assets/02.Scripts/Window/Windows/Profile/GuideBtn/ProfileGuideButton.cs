using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuideButton : MonoBehaviour
{
    private ProfileGuideDataSO guideData;
    public ProfileGuideDataSO GuideData
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
    
    public void Init(ProfileGuideDataSO data)
    {
        guideData = data;
        guideBtn.onClick.AddListener(PlayGuide);
        infoNameText.text = data.guideName;
        EventManager.StartListening(EProfileEvent.EndGuide, EndGuide);
    }

    private void PlayGuide()
    {
        if (isGuide)
        {
            return;
        }
        isGuide = true;
        StartAiChatting();
    }

    private void StartAiChatting()
    {
        ProfileChattingSystem.OnPlayChatList?.Invoke(guideData.guideTextList, 1.5f, false);
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
        EventManager.StopListening(EProfileEvent.EndGuide, EndGuide);
    }
}
