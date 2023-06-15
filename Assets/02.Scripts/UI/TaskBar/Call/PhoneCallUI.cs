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

    private Dictionary<int, Action> actionDictionary = new Dictionary<int, Action>();


    void Start()
    {
        DictionaryAdd();
        this.gameObject.SetActive(false);
    }

    private void DictionaryAdd()
    {
        for(int i = 0; i < 10; i++)
        {
            int idx = i;
            actionDictionary.Add(idx, new Action(() => KeyboardEventAdd(idx)));
        }
    }

    public void Open()
    {
        AllEraseText();
        GetButtonAction();
        gameObject.SetActive(true);
        KeyboardEventAdd();
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    private void KeyboardEventAdd()
    {
        for(int i = 0; i < 10; i++)
        {
            int idx = i;
            string str = "Keypad" + idx.ToString();

            InputManager.Inst.AddKeyInput((KeyCode)Enum.Parse(typeof(KeyCode), str), onKeyDown: actionDictionary[idx]) ;
        }
        InputManager.Inst.AddKeyInput(KeyCode.Backspace, onKeyDown: EraseButton );
    }

    private void KeyboardEventAdd(int value)
    {
        currentNumber += value.ToString();
        phoneNumberText.SetText(currentNumber);
    }

    private void KeyboardEventRemove()
    {
        for (int i = 0; i < 10; i++)
        {
            int idx = i;
            string str = "Keypad" + idx.ToString();
            InputManager.Inst.RemoveKeyInput((KeyCode)Enum.Parse(typeof(KeyCode), str), onKeyDown: actionDictionary[idx]);
        }
        InputManager.Inst.RemoveKeyInput(KeyCode.Backspace, onKeyDown: EraseButton);
    }


    public void Init()
    {
        eraseButton.onClick?.AddListener(EraseButton);
        callButton.onClick?.AddListener(CallButton);
        callTopPanel.Init();
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
        //if (OnCloseIngnoreFlag != null && OnCloseIngnoreFlag.Invoke())
        //    return;

        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        AllEraseText();
        ResetButtonAction();
        KeyboardEventRemove();
        KeyboardEventRemove();
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
            data.characterType = ECharacterDataType.None;
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
