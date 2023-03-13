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
        windowBar.SetNameText("[ " + file.name + " - ��� �ȳ� ]");
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
        // ������ Ȯ�� ���ϰ� �ؾ���
        // �����Դϴ� �� �ߵ� �����Դϴٰ� �ߵ�
        // �� �� ����Ʈ�� �־ �� ����Ʈ�� ���� �� �� ���� �� �Լ��� ���� �Ǹ� �ȵ�

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

        answerMarkText.SetText("�����Դϴ�.");
        answerPanel.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        answerMarkText.SetText("");
        answerPanel.gameObject.SetActive(false);

        WindowManager.Inst.WindowOpen(file.windowType, file);

        // �̰� ���� �̷��� �� �ʿ� ����
        // �� �� Ŀ�ø��� ������ �ʰ� 

        // ��� ���� Ȯ��

        // �̺�Ʈ �Ŵ����� PinLock Clear �̺�Ʈ�� ��� �Ű������� fileSO ���� �ְ� ���̵� �Ŵ����� �̰� ������ �� �����ڵ忡 �̰� �ֵ��� ����
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

        InputManager.Inst.RemoveKeyInput(KeyCode.Return, onKeyDown: CheckPinPassword);

        WindowClose();
    }

    private void OnApplicationQuit()
    {
        Debug.LogError("������� ���� ���ϵ��� LockClear ����� ��� �����մϴ�");

        foreach (FileSO file in additionFileList)
        {
            // file.isWindowLockClear = false;
        }
    }
}
