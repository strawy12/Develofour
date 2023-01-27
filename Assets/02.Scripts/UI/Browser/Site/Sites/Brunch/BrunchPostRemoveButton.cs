using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BrunchPostRemoveButton : MonoBehaviour
{
    private Button removeBtn;
    
    public UnityEvent Onclick { get { return removeBtn.onClick; } }
    private bool isOpen;

    private void Awake()
    {
        removeBtn = GetComponent<Button>();
    }
    public void Init()
    {
        removeBtn ??= GetComponent<Button>();
    }
    public void Open()
    {
        gameObject.SetActive(true);
        isOpen = true;
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }
    private void CheckClose(object[] hits)
    {
        if (!isOpen) return;
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }
    public void Close()
    {
        isOpen = false;
        gameObject.SetActive(false);
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

}
