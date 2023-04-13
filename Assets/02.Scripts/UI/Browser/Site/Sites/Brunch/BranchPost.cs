using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BranchPost : MonoBehaviour
{
    public TextMeshProUGUI novelTitleText;
    public TextMeshProUGUI novelScriptsText;
    public TextMeshProUGUI novelInfoText;
    public Image novelImage;

    private BranchPostDataSO postData;
    public BranchPostDataSO PostData { get { return postData; } }


    public void Init(BranchPostDataSO postData)
    {
        novelTitleText.text = postData.wirteTitle;
        novelScriptsText.text = postData.wirteInfo;
        novelInfoText.text = postData.wirteDate;
        novelImage.sprite = postData.writeImage;
        this.postData = postData;

    }

}
