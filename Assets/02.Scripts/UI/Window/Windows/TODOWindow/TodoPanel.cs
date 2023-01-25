using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TodoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private QuestDataSO questData;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text categoryText;
    [SerializeField]
    private SuccessRateBar successRateBar;
    [SerializeField]
    private CanvasGroupBtn expendBtn;
    [SerializeField]
    private TMP_Text valueText;

    public Action<TodoPanel> OnClick;
    public QuestDataSO QuestData => questData;

    // 해당 변수는 Expend 기준이 아닌 Enter가 기준이다.
    private static TodoPanel selectedPanel;

    public void Init(QuestDataSO data)
    {
        questData = data;
        expendBtn.onClick.AddListener(ClickExpendBtn);

        SetUI();
    }

    public void SetUI()
    {
        nameText.text = questData.questText.head;
        categoryText.text = questData.category.ToString();
        successRateBar.SetRateBar(questData.SuccessRate);
    }

    public void ClickExpendBtn()
    {
        OnClick?.Invoke(this);
    }



    public void Selected()
    {
        expendBtn.canvasGroup.DOKill();
        expendBtn.canvasGroup.DOFade(1f, 0.2f);
        expendBtn.canvasGroup.interactable = true;
        expendBtn.canvasGroup.blocksRaycasts = true;
    }

    public void UnSelected()
    {
        expendBtn.canvasGroup.DOKill();
        expendBtn.canvasGroup.DOFade(0f, 0.2f);
        expendBtn.canvasGroup.interactable = false;
        expendBtn.canvasGroup.blocksRaycasts = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedPanel?.UnSelected();
        selectedPanel = this;
        selectedPanel?.Selected();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (selectedPanel == this)
        {
            selectedPanel.UnSelected();
            selectedPanel = null;
        }
    }
}
