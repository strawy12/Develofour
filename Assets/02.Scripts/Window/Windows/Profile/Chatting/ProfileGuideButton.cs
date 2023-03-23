using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuideButton : MonoBehaviour
{
    private EProfileCategory category;

    public EProfileCategory Category
    {
        get => category;
    }

    private string infoKey;

    private bool isGuide;

    [SerializeField]
    private Button guideBtn;

    public string InfoKey
    {
        get => infoKey;
    }

    public void Init(EProfileCategory category, string infoKey)
    {
        this.category = category;
        this.infoKey = infoKey;

        guideBtn.onClick.AddListener(PlayGuide);
    }

    private void PlayGuide()
    {
        if (isGuide)
            return;

        


    }
    
    private void EndGuide()
    {

    }
}
