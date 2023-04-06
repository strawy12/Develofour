using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DiscordIdentification : MonoBehaviour
{
    private string identificationAnswerText;

    //public DiscordHideAndShow identificationHidePanel;
    public TMP_InputField identificationInputfield;
    public TextMove identificationTextmove;

    public GameObject loginPanel;

    public Button loginBtn;

    public TextMove wrongText;

    public DiscordRomePuzzle romePuzzle;

    public void Init(string IDAnswer)
    {
        identificationAnswerText = IDAnswer;
        loginBtn.onClick.AddListener(OnClickSubmisstion);
        romePuzzle.Init();

        identificationInputfield.onSubmit.AddListener(delegate { OnClickSubmisstion(); });
        romePuzzle.inputField.onSubmit.AddListener(delegate { OnClickSubmisstion(); });
        InputManager.Inst.AddKeyInput(KeyCode.Tab, onKeyDown: OnInputTap);
    }

    private void OnInputTap()
    {
        romePuzzle.inputField.ActivateInputField();
    }

    public void OnClickSubmisstion()
    {
#if UNITY_EDITOR
        if(identificationInputfield.text == "11")
        {
            Clear();
            return;
        }
#endif
        if (identificationInputfield.text == identificationAnswerText && romePuzzle.IsAnswer())
        {
            Clear();
        }
        else
        {
            if (identificationInputfield.text != identificationAnswerText)
            {
                identificationInputfield.gameObject.SetActive(true);
                wrongText.FaliedInput("본인 확인 질문이 틀렸습니다.");
            }
            else if(!romePuzzle.IsAnswer())
            {
                wrongText.FaliedInput("보안 문자를 입력해주세요.");
            }
        }
    }

    private void Clear()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.Tab, onKeyDown: OnInputTap);
        loginPanel.SetActive(false);
    }
}
