using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class BranchPostLockPanel : MonoBehaviour
{
    [SerializeField]
    private PasswordInputField passwordField;
    [SerializeField]
    private TMP_Text wrongText;
    [SerializeField]
    private TMP_Text hintText;
    [SerializeField]
    private Button checkBtn;
    private BranchPostDataSO currentPostData;

    public void Show(BranchPostDataSO postData)
    {
        passwordField.InputField.text = "";
        currentPostData = postData;
        hintText.text = postData.postPasswordHint;
        passwordField.SetPassword(postData.postPassword);
        passwordField.OnSuccessLogin += SuccessLogin;
        passwordField.OnFailLogin += FailLogin;

        checkBtn.onClick.RemoveAllListeners();
        checkBtn.onClick.AddListener(ClickCheckBtn);
    }

    private void SuccessLogin()
    {
        DataManager.Inst.AddBranchUnLock(currentPostData);

        gameObject.SetActive(false);
        EventManager.TriggerEvent(EBranchEvent.ShowPost, new object[1] { currentPostData });
    }

    private void FailLogin()
    {
        passwordField.InputField.text = "";
        wrongText.text = "�߸��� ��й�ȣ �Դϴ�.";
    }

    private void ClickCheckBtn()
    {
        passwordField.TryLogin();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
