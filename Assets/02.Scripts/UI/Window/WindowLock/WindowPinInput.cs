using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI; 

public class WindowPinInput : Window
{
    private static List<FileSO> additionFileList;
    
    private bool isShaking = false;

    [SerializeField]
    private FileSO pinFileSO;

    [Header("Pin UI")]
    [SerializeField]
    private TMP_Text pinWindowNameBarText;
    [SerializeField]
    private TMP_Text pinGuideText;
    [SerializeField]
    private TMP_Text answerMarkText;

    [Header("PinWindowBar")]
    [SerializeField]
    private TMP_Text pinBarText;
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private TMP_InputField pinInputField;
     
    [SerializeField]
    private Button confirmButton;

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


    protected override void Init()
    {
        base.Init();

        confirmButton.onClick?.AddListener(CheckPinPassword);
        closeButton.onClick?.AddListener(CloseWindowPinLock);

        additionFileList = new List<FileSO>();
    }

    public override void WindowOpen()
    {
        PinOpen();
        base.WindowOpen();
    }

    private void PinOpen()
    {
        windowBar.SetNameText("[ " + file.name + " - 잠금 안내 ]");
        pinGuideText.SetText(file.windowPinHintGuide);

        Input.imeCompositionMode = IMECompositionMode.On;

        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyDown: CheckPinPassword);
    }

    private void CheckPinPassword()
    {
        if (pinInputField.text == file.windowPin)
        {
            file.isWindowLockClear = true;

            PinAnswerTextChange();
        
            additionFileList.Add(file);
        }
        else
        {
            PinWrongAnswer();
        }

        pinInputField.text = "";
    }

    private void PinAnswerTextChange()
    {
        answerMarkText.color = answerTextColor;

        answerMarkText.SetText("정답입니다.");
        answerPanel.gameObject.SetActive(true);
        
        answerMarkText.rectTransform.DOShakePosition(1, 0, 0).OnComplete(() =>
        {
            answerMarkText.SetText("");
            answerPanel.gameObject.SetActive(false);

            WindowManager.Inst.WindowOpen(file.windowType, file);

            if(file.name == "ZooglePassword")
            {
                GuideManager.Inst.guidesDictionary[EGuideType.ClickPinNotePadHint] = true;
            }

            CloseWindowPinLock();
        });
    }

    private void PinWrongAnswer()
    {
        if (isShaking) return;

        answerMarkText.color = wrongAnswerTextColor;
        answerMarkText.SetText("오답입니다.");

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

        Input.imeCompositionMode = IMECompositionMode.Off;

        InputManager.Inst.RemoveKeyInput(KeyCode.Return, onKeyDown: CheckPinPassword);

        WindowClose();
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
