using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ProfilerPanelButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text btnText;

    private Button button;

    private RectTransform rectTransform;

    public RectTransform RectTrm
    {
        get
        {
            rectTransform ??= GetComponent<RectTransform>();
            return rectTransform;
        }
    }

    public void AddListening(UnityAction action)
    {
        button ??= GetComponent<Button>();
        button.onClick?.AddListener(action);
    }

    public void Setting(ProfilerPanelButton beforeButton)
    {
        button ??= GetComponent<Button>();

        if (beforeButton == this)
        {
            button.image.color = Color.black;
            btnText.color = Color.white;
        }
        else
        {
            button.image.color = Color.white;
            btnText.color = Color.black;
        }
    }
}
