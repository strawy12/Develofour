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
    [SerializeField]
    private BranchSentMailPanel sentPanel;
    [SerializeField]
    private BranchChangeMailPanel changePanel;

    public override void Init()
    {
        base.Init();
        passwordField.InputField.asteriskChar = '·';
        passwordField.SetPassword(DataManager.Inst.SaveData.branchPassword);
        findPasswordBtn.Init(id);
        sentPanel.Init();
        changePanel.Init();
        loginBtn.onClick?.AddListener(LoginButtonClick);
    }

    private void LoginButtonClick()
    {
        if (passwordField.GetTryLoginBoolean() && branchIDField.text == "11")
        {
            SuccessLogin();
        }

        if (passwordField.InputField.text == password && branchIDField.text == id)
        {
            SuccessLogin();
        }
        else if (branchIDField.text.Length <= 0)
        {
            wrongText.text = "아이디를 입력해주세요.";
        }
        else if (branchIDField.text != id)
        {
            wrongText.text = "등록되지 않은 아이디 입니다.";
        }
        else if(passwordField.InputField.text.Length <= 0)
        {
            wrongText.text = "비밀번호를 입력해주세요.";
        }
        else
        {
            wrongText.text = "잘못된 비밀번호 입니다.";
        }
    }

    private void SuccessLogin()
    {
        if(!DataManager.Inst.IsProfilerInfoData(Constant.ProfilerInfoKey.BRANCHID))
        {
            EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[] { EProfilerCategory.InvisibleInformation, Constant.ProfilerInfoKey.BRANCHID });
        }
        DataManager.Inst.SetIsLogin(ELoginType.Branch,true);
        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.Branch, Constant.LOADING_DELAY });
    }

    protected override void ResetSite()
    {
        base.ResetSite();
        findPasswordBtn.Release();
    }
}
