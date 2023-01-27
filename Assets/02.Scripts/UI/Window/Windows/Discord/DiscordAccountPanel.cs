using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class DiscordAccountPanel : MonoBehaviour
{
    private Button btn;

    public Action<string> OnClick;

    public TextMeshProUGUI text;

    public void Init()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(SetText);
    }

    public void SetText()
    {
        OnClick?.Invoke(text.text);
        this.gameObject.SetActive(false);
    }

}
