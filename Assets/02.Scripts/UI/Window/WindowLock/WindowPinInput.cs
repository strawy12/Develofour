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

    private FileSO currentFile;

    private List<FileSO> additionFileList = new List<FileSO>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        pinInputField.asteriskChar = '��';
        pinInputField.contentType = TMP_InputField.ContentType.Pin;

        pinInputField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);

        confirmButton.onClick?.AddListener(CheckPinPassword);
        closeButton.onClick?.AddListener(CloseWindowPinLock);
        
        EventManager.StartListening(EWindowEvent.OpenWindowPin, PinOpen);
    }

    private void PinOpen(object[] ps)
    {
        if (ps == null || ps.Length == 0 || !(ps[0] is FileSO))
        {
            Debug.LogError("WindowPin�� ���� ������ �ùٸ��� �ʽ��ϴ�.");
            return;
        }

        SetActive(true);

        currentFile = (FileSO)ps[0];
    }

    private void CheckPinPassword()
    {
        if (pinInputField.text == currentFile.windowPin)
        {
            pinInputField.text = "";

            currentFile.isWindowLockClear = true;

            WindowManager.Inst.WindowOpen(currentFile.windowType, currentFile);
            CloseWindowPinLock();
            
            additionFileList.Add(currentFile);
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

    private void OnApplicationQuit()
    {
        Debug.LogError("������� ���� ���ϵ��� LockClear ����� ��� �����մϴ�");

        foreach (FileSO file in additionFileList)
        {
            file.isWindowLockClear = false;
        }
    }
}
