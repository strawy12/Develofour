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

    public Action OnFind;

    private bool isGuide;
    public void Init(ProfileInfoTextDataSO data)
    {
        infoData = data;
        guideBtn.onClick.AddListener(PlayGuide);
        infoNameText.text = data.infoName;

    }

    private void PlayGuide()
    {
        if (isGuide)
            return;

        isGuide = true;
        GuideManager.OnPlayGuide?.Invoke(infoData.guideTopicName, 0.1f);
        
    }

    private void EndGuide()
    {

    }



}
