using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWindow : Window
{
    [SerializeField]
    private Button agreeBtn;

    [SerializeField]
    private Button degreeBtn;

    protected override void Init()
    {
        base.Init();
        degreeBtn.onClick.AddListener(Close);
        agreeBtn.onClick.AddListener(Agree);
    }
   
    private void Close()
    {
        EventManager.TriggerEvent(EProfileSearchTutorialEvent.EndTutorial);
        WindowClose();
    }
    
    private void Agree()
    {
        EventManager.TriggerEvent(EProfileSearchTutorialEvent.TutorialStart);
        WindowClose();
    }
}
