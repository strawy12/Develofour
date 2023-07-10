using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class OutStarLoginPanel : MonoBehaviour
{
    public TMP_InputField idInputField;
    public TMP_InputField passwordInputField;
    public Button showPasswordBtn;
    public TMP_Text showBtnText;

    public OutStarRomePuzzle romePuzzle;

    #region ����
    public string idAnswer;
    public string passwordAnswer;
    #endregion

    public TMP_Text guideText;

    public Button loginBtn;

    void Start()
    {
        Init();
    }

    public void Init()
    {
        showPasswordBtn.onClick.AddListener(OnShowPasswordButton);
        loginBtn.onClick.AddListener(OnLoginButton);
        romePuzzle.Init();
        gameObject.SetActive(true);
        guideText.text = "";
    }

    public void OnLoginButton()
    {
        if(CheckInputField() && CheckSecurity())
        {
            this.gameObject.SetActive(false);
            DataManager.Inst.SaveData.isOutStarLogin = true;
        }
    }

    private bool CheckInputField()
    {
        if(idInputField.text != idAnswer)
        {
            guideText.text = "ID�� �߸��Ǿ����ϴ�.";
            return false;
        }

        if(passwordInputField.text != passwordAnswer)
        {
            guideText.text = "��й�ȣ�� �߸��Ǿ����ϴ�.";
            return false;
        }

        return true;
    }

    private bool CheckSecurity()
    {
        if(!romePuzzle.IsAnswer())
        {
            guideText.text = "���� ���ڰ� �߸��Ǿ����ϴ�.";
            return false;
        }
        return true;
    }

    private void OnShowPasswordButton()
    {
        if(passwordInputField.contentType != TMP_InputField.ContentType.Standard)
        {
            passwordInputField.contentType = TMP_InputField.ContentType.Standard;
            passwordInputField.textComponent.SetAllDirty();
            showBtnText.text = "�����";
        }
        else
        {
            passwordInputField.contentType = TMP_InputField.ContentType.Password;
            passwordInputField.textComponent.SetAllDirty();
            showBtnText.text = "��й�ȣ ǥ��";
        }
    }
}
