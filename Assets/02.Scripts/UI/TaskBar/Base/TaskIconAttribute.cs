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

    public Action OnCloseTaskIcon;
    /// <summary>
    /// ���� Fixed��� Open, �ƴϸ� ù��°â �������ϱ�
    /// </summary>
    public Action OnOpenWindow;

    public void Init()
    {
        //TODO �̰� �ȵǴ°� ��ġ��
        EventManager.StartListening(EEvent.LeftButtonClick, CheckClose);
        openButton.onClick.AddListener(AttributeOpen);
        closeButton.onClick.AddListener(AttributeClose);
    }

    private void Open()
    {
        gameObject.SetActive(true);
    }
    public void CheckClose(object hits)
    {
        if (gameObject.activeSelf == false) return;
        if (Define.ExistInHits(gameObject, hits) == false)
        {
            Close();
        }
    }
    private void Close()
    {
        gameObject.SetActive(false);
    }

    public void AttributeClose()
    {
        OnCloseTaskIcon?.Invoke();
        Close();
    }

    public void AttributeOpen()
    {
        OnOpenWindow?.Invoke();
        Close();
    }

    
}
