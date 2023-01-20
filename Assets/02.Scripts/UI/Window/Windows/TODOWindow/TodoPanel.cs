using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TodoPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TodoData todoData;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text categoryText;
    [SerializeField]
    private SuccessRateBar successRateBar;
    [SerializeField]
    private CanvasGroupBtn expendBtn;

    private static TodoPanel selectedPanel;


    public void Init(TodoData data)
    {
        todoData = data;

        SetUI(); 
    }

    public void SetUI()
    {
        nameText.text = todoData.todoName;
        categoryText.text = todoData.category;
        successRateBar.SetRateBar(todoData.SuccessRate);
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
        if(selectedPanel == this)
        {
            selectedPanel.UnSelected();
            selectedPanel = null;
        }
    }
}
