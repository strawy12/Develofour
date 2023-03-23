using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DiscordIdentification : MonoBehaviour
{
    [Header("비밀번호")]
    public string identificationAnswerText;
    public string PINAnswerText;

    //public DiscordHideAndShow identificationHidePanel;
    public DiscordHideAndShow PINHidePanel;
    public TMP_InputField identificationInputfield;
    public TextMove identificationTextmove;
    public TMP_InputField PINInputfield;
    public TextMove PINTextmove;

    public GameObject loginPanel;

    public Button loginBtn;

    public TextMove wrongText;

    public DiscordRomePuzzle romePuzzle;

    void Start()
    {
        Debug.Log("DiscordIdentification 스크립트 11 디버그 코드 사용중");
    }

    public void Init(string IDAnswer, string PINAnswer)
    {
        identificationAnswerText = IDAnswer;
        PINAnswerText = PINAnswer;
        loginBtn.onClick.AddListener(OnClickSubmisstion);
        romePuzzle.Init();

        identificationInputfield.onSubmit.AddListener(delegate { OnClickSubmisstion(); });
        PINInputfield.onSubmit.AddListener(delegate { OnClickSubmisstion(); });
        romePuzzle.inputField.onSubmit.AddListener(delegate { OnClickSubmisstion(); });
        PINInputfield.onValueChanged.AddListener((a) => Input.imeCompositionMode = IMECompositionMode.Off);
        InputManager.Inst.AddKeyInput(KeyCode.Tab, onKeyDown: OnInputTap);
    }

    private void OnInputTap()
    {
        if(identificationInputfield.isFocused)
        {
            PINInputfield.ActivateInputField();
        }
        else if(PINInputfield.isFocused)
        {
            romePuzzle.inputField.ActivateInputField();
        }
    }

    public void OnClickSubmisstion()
    {
        Debug.Log("DiscordIdentification 스크립트 11 디버그 코드 ");

        if(!romePuzzle.IsAnswer())
        {
            wrongText.FaliedInput("보안 문자가 틀렸습니다.");
            return;
        }

        if (identificationInputfield.text == identificationAnswerText && PINInputfield.text == PINAnswerText
            || identificationInputfield.text == "11" && PINInputfield.text == "11")
        {
            if(identificationInputfield.text == "11")
            {
                
            }
            Clear();
        }
        else
        {
            wrongText.FaliedInput("본인 확인 질문이 틀렸습니다.");
            if (identificationInputfield.text != identificationAnswerText)
            {
                identificationInputfield.gameObject.SetActive(true);
                //identificationTextmove.FaliedInput("<b>이메일 또는 전화번호 </b>- <i><size=85%> 유효하지 않은 아이디입니다.</i>");
            }

            if (PINInputfield.text != PINAnswerText)
            {
                PINInputfield.gameObject.SetActive(true);
                //PINTextmove.FaliedInput("<b>비밀번호 </b>- <i><size=85%> 유효하지 않은 비밀번호입니다.</i>");
            }
        }
    }

    private void Clear()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.Tab, onKeyDown: OnInputTap);
        loginPanel.SetActive(false);
    }
}
