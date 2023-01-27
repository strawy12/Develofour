using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowPinInput : MonoUI
{
    private bool isHideFinSeeStay;

    [SerializeField]
    private TMP_Text pinGuideText;

    [SerializeField]
    private TMP_InputField pinInputField;

    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button hidePinSeeButton;


    private FileSO currentFile;

    private List<FileSO> additionFileList = new List<FileSO>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isHideFinSeeStay = false;

        pinInputField.asteriskChar = '●';
        pinInputField.contentType = TMP_InputField.ContentType.Pin;

        pinInputField.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);

        confirmButton.onClick?.AddListener(CheckPinPassword);
        closeButton.onClick?.AddListener(CloseWindowPinLock);
        hidePinSeeButton.onClick?.AddListener(HidePinMarkSee);
        
        EventManager.StartListening(EWindowEvent.OpenWindowPin, PinOpen);
    }

    private void PinOpen(object[] ps)
    {
        if (ps == null || ps.Length == 0 || !(ps[0] is FileSO))
        {
            Debug.LogError("WindowPin에 들어온 파일이 올바르지 않습니다.");
            return;
        }

        SetActive(true);

        currentFile = (FileSO)ps[0];
        pinGuideText.SetText(currentFile.windowPinHintGuide);
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

    private void HidePinMarkSee() 
    {
        if(!isHideFinSeeStay)
        {
            pinInputField.contentType = TMP_InputField.ContentType.IntegerNumber;
            isHideFinSeeStay = true;
        }
        else if(isHideFinSeeStay)
        {
            pinInputField.contentType = TMP_InputField.ContentType.Pin;
            isHideFinSeeStay = false;
        }

        pinInputField.ForceLabelUpdate();
    }


    private void CloseWindowPinLock()
    {
        pinInputField.text = "";
        pinInputField.contentType = TMP_InputField.ContentType.Pin;

        SetActive(false);
    }

    private void OnApplicationQuit()
    {
        Debug.LogError("디버깅을 위해 파일들의 LockClear 기록을 모두 제거합니다");

        foreach (FileSO file in additionFileList)
        {
            file.isWindowLockClear = false;
        }
    }
}
