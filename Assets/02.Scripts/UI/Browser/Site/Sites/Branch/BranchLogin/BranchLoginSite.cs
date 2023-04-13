using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class BranchLoginSite : Site
{
    [SerializeField]
    private TMP_InputField branchIDField;
    [SerializeField]
    private PasswordInputField passwordField;
    [SerializeField]
    private string id;
    [SerializeField]
    private string password;
    [SerializeField]
    private TMP_Text wrongText;
    [SerializeField]
    private Button loginBtn;
    [SerializeField]
    private BranchFindPasswordButton findPasswordBtn;

    public override void Init()
    {
        base.Init();
        passwordField.InputField.asteriskChar = '��';
        passwordField.SetPassword(password);
        findPasswordBtn.Init(id);
        loginBtn.onClick?.AddListener(LoginButtonClick);
    }

    private void LoginButtonClick()
    {
        if (passwordField.GetTryLoginBoolean() && branchIDField.text == id)
        {
            SuccessLogin();
        }
        else if(branchIDField.text == "")
        {
            wrongText.text = "���̵� �Է����ּ���.";
        }
        else if (branchIDField.text != id)
        {
            wrongText.text = "��ϵ��� ���� ���̵� �Դϴ�.";
        }
        else
        {
            wrongText.text = "�߸��� ��й�ȣ �Դϴ�.";
        }
    }

    private void SuccessLogin()
    {
        DataManager.Inst.SetIsLogin(ELoginType.Branch,true);
        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.Branch, Constant.LOADING_DELAY });
    }

    
}
