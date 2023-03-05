using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WindowPinInput : Window
{
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

    private FileSO currentFile;

    private List<FileSO> additionFileList = new List<FileSO>();

    private void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();

        confirmButton.onClick?.AddListener(CheckPinPassword);
        //closeButton.onClick?.AddListener(CloseWindowPinLock);
        
        EventManager.StartListening(EWindowEvent.OpenWindowPin, PinOpen);

    }
    
    private void PinOpen(object[] ps)
    {
        if (ps == null || ps.Length == 0 || !(ps[0] is FileSO))
        {
            Debug.LogError("WindowPin에 들어온 파일이 올바르지 않습니다.");
            return;
        }

        WindowManager.Inst.WindowOpen(pinFileSO.windowType, pinFileSO);

        currentFile = (FileSO)ps[0];
        pinGuideText.SetText(currentFile.windowPinHintGuide);

        Input.imeCompositionMode = IMECompositionMode.On;

        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyDown: CheckPinPassword);
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

    private void PinAnswerTextChange()
    {
        answerMarkText.color = answerTextColor;

        answerMarkText.SetText("정답입니다.");
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

        gameObject.SetActive(false);
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
