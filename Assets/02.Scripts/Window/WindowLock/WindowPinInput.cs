﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class WindowPinInput : Window
{
    private bool isShaking = false;

    [Header("Pin UI")]
    [SerializeField]
    private AutoAnswerInputFiled autoPinAnswerFiled;
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

    private WindowLockDataSO windowLockData;

    protected override void Init()
    {
        base.Init();

        windowLockData = ResourceManager.Inst.GetFileLockData(file.id);

        confirmButton.onClick?.AddListener(CheckPinPassword);
        closeButton.onClick?.AddListener(CloseWindowPinLock);
    }

    public override void WindowOpen()   
    {
        PinOpen();
        SetAnswerDatas();

        base.WindowOpen();
    }

    private void PinOpen()
    {
        windowBar.SetNameText("[ " + file.fileName + " - 잠금 안내 ]");
        pinGuideText.SetText(windowLockData.windowPinHintGuide);

        InputManager.Inst.AddKeyInput(KeyCode.Return, onKeyDown: CheckPinPassword);

        EventManager.TriggerEvent(EGuideEventType.GuideConditionCheck, new object[] { file });
    }

    private void SetAnswerDatas()
    {
        if(!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        Debug.Log(windowLockData.answerData.answer);
        if (windowLockData.answerData.answer == "")
        {
            return;
        }

        autoPinAnswerFiled.autoAnswerDatas.Add(windowLockData.answerData);

        if (windowLockData.answerData != null)
        {
            List<MonologLockDecision> infoDatas = windowLockData.answerData.infoData;
            foreach (MonologLockDecision infoData in infoDatas)
            {
                int infoID = infoData.key;

                if(DataManager.Inst.IsProfilerInfoData(infoID))
                {
                    autoPinAnswerFiled.inputSystem.ShowPanel(autoPinAnswerFiled.inputField, autoPinAnswerFiled.autoAnswerDatas);
                }
            }
        }

    }

    private void CheckPinPassword()
    {
        string inputText = pinInputField.text.Replace(" ", "");
        // 입력한거 공백 제거해서 들고 옴

        string[] answerArr = windowLockData.windowPin.Split(',');
        // 파일 정답 목록들 구분 해서 들고 옴

        foreach (string answerText in answerArr)
        {
            if (inputText == answerText)
            {
                StartCoroutine(PinAnswerTextChange());
                pinInputField.text = "";
                return;
            }
        }
        
        PinWrongAnswer();
        pinInputField.text = "";
    }

    private IEnumerator PinAnswerTextChange()
    {
        answerMarkText.color = answerTextColor;

        answerMarkText.SetText("정답입니다.");
        Debug.Log("Bingo");
        yield return new WaitForSeconds(0.6f);

        answerMarkText.SetText("");

        if (windowLockData.answerData != null)
        {
            List<MonologLockDecision> infoDatas = windowLockData.answerData.infoData;
            foreach (MonologLockDecision infoData in infoDatas)
            {
                int infoID = infoData.key;

                EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[1] { infoID });
            }
        }

        DataManager.Inst.SetFileLock(file.id, false);
        WindowManager.Inst.WindowOpen(file.windowType, file);

        EventManager.TriggerEvent(EGuideEventType.GuideConditionCheck, new object[] { file });

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
}
