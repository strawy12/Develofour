using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuideButton : MonoBehaviour
{
    private ProfileInfoTextDataSO infoData;

    public ProfileInfoTextDataSO InfoData
    {
        get => infoData;
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
    public void Init(ProfileInfoTextDataSO data)
    {
        infoData = data;
        guideBtn.onClick.AddListener(PlayGuide);
        infoNameText.text = data.infoName;
        EventManager.StartListening(EProfileEvent.EndGuide, EndGuide);

    }

    private void PlayGuide()
    {

        if (isGuide)
        {
            return;
        }

        isGuide = true;
        GuideManager.OnPlayInfoGuide?.Invoke(infoData.guideTopicName);
        
    }

    private void EndGuide(object[] ps)
    {
        isGuide = false;
    }

    public void Releasse()
    {
        infoData = null;
        infoNameText.text = "";
        EventManager.StopListening(EProfileEvent.EndGuide, EndGuide);
    }



}
