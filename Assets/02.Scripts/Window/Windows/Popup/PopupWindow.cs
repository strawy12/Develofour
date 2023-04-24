using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : Window
{
    [SerializeField]
    private Button agreeBtn;

    [SerializeField]
    private Button degreeBtn;

    [SerializeField]
    private TMP_Text proposalText;

    public Action AgreeAction;
    public Action DegreeAction;

    protected override void Init()
    {
        base.Init();

        degreeBtn.onClick.AddListener(Close);
        agreeBtn.onClick.AddListener(Agree);
    }

    public void Setting(string text, Action agreeAction, Action degreeAction)
    {
        proposalText.text = text;

        AgreeAction += agreeAction;
        if(degreeAction == null)
        {
            degreeBtn.gameObject.SetActive(false);
        }
        else
        {
            degreeBtn.gameObject.SetActive(true);
        }
        DegreeAction += degreeAction;
    }   

    private void Close()
    {
        DegreeAction?.Invoke();
        //EventManager.TriggerEvent(EProfileSearchTutorialEvent.EndTutorial);
        WindowClose();
    }
    
    private void Agree()
    {
        AgreeAction?.Invoke();
        //EventManager.TriggerEvent(EProfileSearchTutorialEvent.TutorialStart);
        WindowClose();
    }
}
