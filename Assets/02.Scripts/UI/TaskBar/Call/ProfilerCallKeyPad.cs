using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ProfilerCallKeyPad : MonoBehaviour
{
    [SerializeField]
    private TMP_Text phoneNumberText;

    [SerializeField]
    private Button eraseButton;
    [SerializeField]
    private Button callButton;
    [SerializeField]
    private Button clearButton;

    private string currentNumber;

    public Func<bool> OnCloseIngnoreFlag;

    private Dictionary<int, Action> actionDictionary = new Dictionary<int, Action>();

    private void DictionaryAdd()
    {
        for (int i = 0; i < 10; i++)
        {
            int idx = i;
            actionDictionary.Add(idx, () => KeyboardEventAdd(idx));
        }
    }
    public void Open()
    {
        AllEraseText();
        GetButtonAction();
        gameObject.SetActive(true);
        KeyboardEventAdd();
    }

    private void KeyboardEventAdd()
    {
        for (int i = 0; i < 10; i++)
        {
            int idx = i;
            string str = "Keypad" + idx.ToString();
            string str1 = "Alpha" + idx.ToString();

            Debug.Log((KeyCode)Enum.Parse(typeof(KeyCode), str));
            InputManager.Inst.AddKeyInput((KeyCode)Enum.Parse(typeof(KeyCode), str1), onKeyDown: actionDictionary[idx]);
            InputManager.Inst.AddKeyInput((KeyCode)Enum.Parse(typeof(KeyCode), str), onKeyDown: actionDictionary[idx]);
        }
        InputManager.Inst.AddKeyInput(KeyCode.Backspace, onKeyDown: EraseButton);
        InputManager.Inst.AddKeyInput(KeyCode.Escape, onKeyDown: ClearButton);

    }

    private void KeyboardEventAdd(int value)
    {
        currentNumber += value.ToString();
        phoneNumberText.SetText(currentNumber);
    }

    private void KeyboardEventRemove()
    {
        Debug.Log("RemoveKey");

        for (int i = 0; i < 10; i++)
        {
            int idx = i;
            string str = "Keypad" + idx.ToString();
            string str1 = "Alpha" + idx.ToString();
            InputManager.Inst.RemoveKeyInput((KeyCode)Enum.Parse(typeof(KeyCode), str1), onKeyDown: actionDictionary[idx]);
            InputManager.Inst.RemoveKeyInput((KeyCode)Enum.Parse(typeof(KeyCode), str), onKeyDown: actionDictionary[idx]);
        }
        InputManager.Inst.RemoveKeyInput(KeyCode.Backspace, onKeyDown: EraseButton);
        InputManager.Inst.RemoveKeyInput(KeyCode.Escape, onKeyDown: ClearButton);

    }


    public void Init()
    {
        DictionaryAdd();
        eraseButton.onClick?.AddListener(EraseButton);
        callButton.onClick?.AddListener(CallButton);
        clearButton.onClick?.AddListener(ClearButton);
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
        if (currentNumber.Length > 12)
        {
            return;
        }

        currentNumber += data;

        phoneNumberText.SetText(currentNumber);
    }

    public void Close()
    {
        AllEraseText();
        ResetButtonAction();
        KeyboardEventRemove();
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
    private void ClearButton()
    {
        AllEraseText();
    }
    private void CallButton()
    {
        if (string.IsNullOrEmpty(phoneNumberText.text))
        {
            return;
        }

        CharacterInfoDataSO data = ResourceManager.Inst.FindCharacterPhoneNumber(phoneNumberText.text);
        string callProfileID;
        if (data == null)
        {
            callProfileID = "CD_MS";
            data = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(callProfileID);
            data.phoneNum = phoneNumberText.text;
        }
        else
        {
            callProfileID = data.ID;
        }
        CallSystem.OnOutGoingCall?.Invoke(callProfileID);

        AllEraseText();
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
