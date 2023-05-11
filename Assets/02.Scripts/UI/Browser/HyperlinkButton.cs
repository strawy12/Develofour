using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HyperlinkButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text linkText;
    private Button btn;


    public void Init()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(MoveLink);
    }

    public void MoveLink()
    {
        Browser.currentBrowser.AddressInputField.text = linkText.text;
        Browser.currentBrowser.AddressChangeSite(linkText.text);
    }
}
