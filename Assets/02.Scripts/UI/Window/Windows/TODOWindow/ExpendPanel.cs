using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class ExpendPanel : MonoBehaviour
{
    [SerializeField]
    private HighlightBtn closeBtn;
    [SerializeField]
    private HighlightBtn backBtn;
    [SerializeField]
    private HighlightBtn nextBtn;

    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text statusText;
    [SerializeField]
    private TMP_Text categoryText;
    [SerializeField]
    private TMP_Text successRateText;
    [SerializeField]
    private TMP_Text bodyText;

    public TodoPanel currentTodoPanel { get; private set; }

    public Action OnClickBackBtn 
    {
        get => backBtn.OnClick;

        set
        {
            backBtn.OnClick = value;
        }
    }

    public Action OnClickNextBtn
    {
        get => nextBtn.OnClick;
        set
        {
            nextBtn.OnClick = value;
        }
    }

    public void Init()
    {
        closeBtn.OnClick += HideExpendPanel;
    }


    public void ShowExpendPanel()
    {
        if (transform.localScale.x == 1f)
            return;
        transform.DOKill();
        transform.DOScaleX(1f, 0.25f);
    }

    public void HideExpendPanel()
    {
        if (transform.localScale.x == 0f)
            return;
        transform.DOKill();
        transform.DOScaleX(0f, 0.25f);
    }

    public void SetCurrentTodoPanel(TodoPanel panel)
    {
        currentTodoPanel = panel;
        SetUI(currentTodoPanel.QuestData);
    }
    
    public void SetUI(QuestDataSO data)
    {
        ShowExpendPanel();

        titleText.text = data.questText.head;
        statusText.text = data.isClear ? "완료" : "진행중";
        categoryText.text = data.category.ToString();
        successRateText.text = $"{data.SuccessRate.ToString()}%";

        bodyText.text = data.questText.detailBody;
    }
}
