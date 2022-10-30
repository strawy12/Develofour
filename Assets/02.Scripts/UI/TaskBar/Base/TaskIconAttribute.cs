using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TaskIconAttribute : MonoBehaviour
{

    [SerializeField]
    private Button openButton;

    [SerializeField]
    private Button closeButton;

    public Action OnClose;
    public Action OnOpen;

    void OnEnable()
    {
        EventManager.StartListening(EEvent.LeftButtonClick, (x) => Close());
    }

    void OnDisable()
    {
        EventManager.StopListening(EEvent.LeftButtonClick, (x) => Close());
    }

    public void Release()
    {
        EventManager.StopListening(EEvent.LeftButtonClick, (x) => Close());
    }

    public void Init()
    {
        //TODO 이거 안되는거 고치기
        EventManager.StartListening(EEvent.LeftButtonClick, (x) =>  Close());
        openButton.onClick.AddListener(AttributeOpen);
        closeButton.onClick.AddListener(AttributeClose);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        WindowManager.Inst.IsTaskIconAttribute = true;
        
    }

    public void Close()
    {
        gameObject.SetActive(false);
        WindowManager.Inst.IsTaskIconAttribute = false;
    }

    public void AttributeClose()
    {
        OnClose?.Invoke();
        Close();
    }

    public void AttributeOpen()
    {
        OnOpen?.Invoke();
        Close();
    }

    
}
