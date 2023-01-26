using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowPinInput : MonoUI
{
    [SerializeField]
    private TMP_InputField pinInputField;

    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button closeButton;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        pinInputField.asteriskChar = '¡Ü';
        pinInputField.contentType = TMP_InputField.ContentType.Pin;

        pinInputField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);

        confirmButton.onClick?.AddListener(CheckPinPassword);
        closeButton.onClick?.AddListener(CloseWindowPinLock);
    }

    private void CheckPinPassword()
    {
        if (pinInputField.text == WindowPinManager.Inst.windowPin)
        {
            DataManager.Inst.CurrentPlayer.CurrentChapterData.isWindowPinPasswordClear = true;
            CloseWindowPinLock();
        }
        else
        {
            pinInputField.text = "";
        }
    }

    private void CloseWindowPinLock()
    {
        SetActive(false);
    }

}
