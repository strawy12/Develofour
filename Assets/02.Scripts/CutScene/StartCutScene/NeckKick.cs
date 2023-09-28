using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeckKick : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> textList;

    int idx = 0;

    public Action OnEnd;

    public void Show()
    {
        gameObject.SetActive(true);
        InputManager.Inst.AddMouseInput(EMouseType.LeftClick, onKeyDown: ShowText);
    }

    public void Hide()
    {
        InputManager.Inst.RemoveMouseInput(EMouseType.LeftClick, onKeyDown: ShowText);
        OnEnd?.Invoke();
        gameObject.SetActive(false);
        OnEnd = null;
    }

    public void ShowText()
    {
        if (idx >= textList.Count)
        {
            Hide();
            return; 
        }

        textList[idx].SetActive(true);
        idx++;
    }
}

