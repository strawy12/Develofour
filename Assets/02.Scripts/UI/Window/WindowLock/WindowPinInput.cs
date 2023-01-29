using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowPinInput : MonoUI
{
    private bool isShaking = false;
    private bool isHideFinSeeStay;

    [SerializeField]
    private TMP_Text pinGuideText;
    [SerializeField]
    private TMP_Text answerMarkText;

    [SerializeField]
    private TMP_InputField pinInputField;

    [SerializeField]
    private Button confirmButton;
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button hidePinSeeButton;

    [SerializeField]
    private Image answerPanel;

    [Header("Shaking Data")]
    [SerializeField]
    private int strength;
    [SerializeField]
    private int vibrato;
    [SerializeField]
    private float duration;
    [SerializeField]
    private Color answerTextColor;
    [SerializeField]
    private Color wrongAnswerTextColor;

    private FileSO currentFile;

    private List<FileSO> additionFileList = new List<FileSO>();

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        isHideFinSeeStay = false;

        pinInputField.asteriskChar = '��';
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
            Debug.LogError("WindowPin�� ���� ������ �ùٸ��� �ʽ��ϴ�.");
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
            currentFile.isWindowLockClear = true;

            PinAnswerTextChange();
        
            additionFileList.Add(currentFile);
        }
        else
        {
            PinWrongAnswer();
        }

            pinInputField.text = "";
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

    private void PinAnswerTextChange()
    {
        answerMarkText.color = answerTextColor;

        answerMarkText.SetText("�����Դϴ�.");
        answerPanel.gameObject.SetActive(true);

        answerMarkText.rectTransform.DOShakePosition(1, 0, 0).OnComplete(() =>
        {
            answerMarkText.SetText("");
            answerPanel.gameObject.SetActive(false);

            WindowManager.Inst.WindowOpen(currentFile.windowType, currentFile);

            CloseWindowPinLock();

        });
    }

    private void PinWrongAnswer()
    {
        if (isShaking) return;

        answerMarkText.color = wrongAnswerTextColor;
        answerMarkText.SetText("�����Դϴ�.");

        isShaking = true;

        answerMarkText.rectTransform.DOKill(true);
        answerMarkText.rectTransform.DOShakePosition(duration, strength, vibrato).OnComplete(() =>
        {
            answerMarkText.SetText("");
            isShaking = false;
        });
    }

    private void CloseWindowPinLock()
    {
        pinInputField.text = "";
        pinInputField.contentType = TMP_InputField.ContentType.Pin;

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
