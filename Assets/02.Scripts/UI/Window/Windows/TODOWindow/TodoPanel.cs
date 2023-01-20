using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TodoPanel : MonoBehaviour, IPointerEnterHandler
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
    private Button expendBtn;

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

    public void OnPointerEnter(PointerEventData eventData)
    {
        selectedPanel.UnSelected();
        selectedPanel = this;
        selectedPanel.Selected();
    }

    public void Selected()
    {

    }

    public void UnSelected()
    {
        
    }
}
