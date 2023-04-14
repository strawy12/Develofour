using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class BranchPostLine : MonoBehaviour, IPointerClickHandler
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

    public void Release()
    {
        novelTitleText.text = "";
        novelScriptsText.text = "";
        novelInfoText.text = "";
        novelImage.sprite = null;
        this.postData = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(EBranchEvent.ShowPost, new object[1] { postData });
    }
}
