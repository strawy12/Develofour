using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

        Debug.Log(11);
        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyDown: CheckPinPassword);

        if (file.name == "ZooglePassword" && !GuideManager.Inst.isZooglePinNotePadOpenCheck)
        {
            GuideManager.Inst.isZooglePinNotePadOpenCheck = true;

            EventManager.TriggerEvent(ECoreEvent.OpenPlayGuide, new object[2] { 1200f, EGuideType.ClickPinNotePadHint });
        }
    }



    private void CheckPinPassword()
    {
        // 여러번 확인 못하게 해야해
        // 오답입니다 가 뜨든 정답입니다가 뜨든
        // 둘 다 이펙트가 있어서 그 이펙트가 종료 될 때 까지 이 함수가 실행 되면 안돼

        if (pinInputField.text == file.windowPin)
        {
            DataManager.Inst.SetWindowLock(file.GetFileLocation(), false);

            StartCoroutine(PinAnswerTextChange());

            additionFileList.Add(file);
        }
        else
        {
            PinWrongAnswer();
        }

        pinInputField.text = "";
    }

    private IEnumerator PinAnswerTextChange()
    {
        answerMarkText.color = answerTextColor;

        answerMarkText.SetText("정답입니다.");
        answerPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        answerMarkText.SetText("");
        answerPanel.gameObject.SetActive(false);

        WindowManager.Inst.WindowOpen(file.windowType, file);

        // 이걸 굳이 이렇게 쓸 필요 없지
        // 좀 더 커플링이 심하지 않게 

        // 잠금 해제 확인

        // 이벤트 매니저로 PinLock Clear 이벤트를 쏘고 매개변수로 fileSO 같이 주고 가이드 매니저가 이걸 들으면 그 내부코드에 이게 있도록 수정
        if (file.name == "ZooglePassword" && GuideManager.Inst.isZooglePinNotePadOpenCheck)
        {
            GuideManager.Inst.guidesDictionary[EGuideType.ClearPinNotePadQuiz] = true;
        }

        CloseWindowPinLock();
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

        InputManager.Inst.RemoveKeyInput(KeyCode.Return, onKeyDown: CheckPinPassword);

        WindowClose();
    }

    private void OnApplicationQuit()
    {
        Debug.LogError("디버깅을 위해 파일들의 LockClear 기록을 모두 제거합니다");

        foreach (FileSO file in additionFileList)
        {
            // file.isWindowLockClear = false;
        }
    }
}
