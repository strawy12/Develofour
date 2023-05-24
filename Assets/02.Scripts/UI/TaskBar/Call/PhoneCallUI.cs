using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static System.Net.Mime.MediaTypeNames;
using System;

public class PhoneCallUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text phoneNumberText;

    [SerializeField]
    private Button eraseButton;
    [SerializeField]
    private Button callButton;
    [SerializeField]
    private CallTopPanel callTopPanel;
    [SerializeField]
    private GameObject buttonPad;

    private string currentNumber;

    public Func<bool> OnCloseIngnoreFlag;

    void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void Open()
    {
        AllEraseText();
        GetButtonAction();
        gameObject.SetActive(true);
    }

    public void Init()
    {
        eraseButton.onClick?.AddListener(EraseButton);
        callButton.onClick?.AddListener(CallButton);
        callTopPanel.Init();
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);

    }

    private void GetButtonAction()
    {
        NumberPadInput[] btns = GetComponentsInChildren<NumberPadInput>();

        foreach (NumberPadInput btn in btns)
        {
            btn.OnClick += OnClickBtn;
        }
    }

    private void ResetButtonAction()
    {
        NumberPadInput[] btns = GetComponentsInChildren<NumberPadInput>();

        foreach (NumberPadInput btn in btns)
        {
            btn.OnClick -= OnClickBtn;
        }
    }

    private void OnClickBtn(string data)
    {
        if(currentNumber.Length > 12)
        {
            return;
        }

        currentNumber += data;

        phoneNumberText.SetText(currentNumber);
    }

    private void CheckClose(object[] hits)
    {
        if (OnCloseIngnoreFlag != null && OnCloseIngnoreFlag.Invoke())
            return;

        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        AllEraseText();
        ResetButtonAction();

        gameObject.SetActive(false);

        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    private void EraseButton()
    {
        if (currentNumber.Length <= 0)
        {
            return;
        }

        string EraseText = currentNumber.Substring(0, currentNumber.Length - 1);
        currentNumber = EraseText;

        phoneNumberText.SetText(currentNumber);
    }

    private void CallButton()
    {
        if(string.IsNullOrEmpty(phoneNumberText.text))
        {
            return;
        }

        CharacterInfoDataSO data = ResourceManager.Inst.GetCharacterDataSO(phoneNumberText.text);
        if (data == null)
        {
            data = new CharacterInfoDataSO();
            data.characterName = "";
            data.phoneNum = phoneNumberText.text;
        }
        CallSystem.Inst.OnRequestCall(data);

        Close();
    }
    public void SetNumberText(string number)
    {
        currentNumber = number;
        phoneNumberText.SetText(currentNumber);
    }
    private void AllEraseText()
    {
        currentNumber = "";

        phoneNumberText.SetText(currentNumber);
    }
}
