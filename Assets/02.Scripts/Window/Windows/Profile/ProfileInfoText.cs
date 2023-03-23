﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProfileInfoText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ProfileInfoTextDataSO textDataSO;

    public bool isTitleShow = false;
    public TMP_Text infoText;

    public ProfileShowInfoTextPanel showPanel;

    public TMP_Text infoTitleText;

    private string infoTitle;

    public bool isFind;
    public Action OnFindText;
    //이전 텍스트로 변경, 이후 텍스트로 변경해주는 함수

    public void Init()
    {
        infoTitle = infoTitleText.text;

        if (!isTitleShow)
        {
            string[] info = infoTitle.Split(" ");
            string str = "";
            for (int i = 0; i < info[0].Length; i++)
            {
                str += "?";
            }
            str += " :";
            infoTitleText.text = str;
        }
    }

    public void ShowTitle()
    {
        infoTitleText.text = infoTitle;
    }


    public void ChangeText()
    {
        //infoTitleText.text = infoTitle;
        infoText.text = textDataSO.afterText;
        isFind = true;
        OnFindText?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoText.text != textDataSO.afterText)
        {
            return;
        }

        showPanel.text.text = textDataSO.getInfoText;
        showPanel.transform.SetParent(gameObject.transform.parent);
        showPanel.GetComponent<RectTransform>().position = gameObject.GetComponent<RectTransform>().position;
        showPanel.transform.SetParent(showPanel.showPanelParent.transform);
        showPanel.GetComponent<RectTransform>().anchoredPosition += new Vector2(20, 35);
        showPanel.SetDownText();
        showPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showPanel == null) return;
        showPanel.gameObject.SetActive(false);
    }
}
