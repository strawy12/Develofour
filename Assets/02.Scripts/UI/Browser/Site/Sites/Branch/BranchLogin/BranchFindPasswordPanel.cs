﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BranchFindPasswordPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField branchIDField;
    [SerializeField]
    private Button checkBtn;
    //SerializeField]
    //private TMP_Text signText;
    private string id;

    public BranchSentMailPanel sentPanel;
    public void Init(string id)
    {
        this.id = id;
        checkBtn.onClick.AddListener(CheckID);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void CheckID()
    {
        if(branchIDField.text == id)
        {
            Success();
        }
        else if(branchIDField.text == "")
        {
            branchIDField.text = "아이디를 입력해 주세요.";
        }
        else
        {
            branchIDField.text = "존재하지 않는 아이디입니다.";
        }
    }

    private void Success()
    {
        //signText.SetText("비밀번호 변경메일을 전송했습니다.");
        sentPanel.gameObject.SetActive(true);
    }

}
