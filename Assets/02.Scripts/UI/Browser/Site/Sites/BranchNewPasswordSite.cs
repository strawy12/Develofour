using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchNewPasswordSite : Site
{
    [Header("비밀번호")]
    [SerializeField]
    private TMP_InputField passwordInput;
    [SerializeField]
    private Button successBtn;
    [SerializeField]
    private TMP_Text wrongText;
    public override void Init()
    {
        base.Init();
        successBtn.onClick.AddListener(CheckPassword);
  
    }

    private void CheckPassword()
    {
        if (passwordInput.text.Length <= 3)
        {
            wrongText.SetText("비밀번호는 3자리 이상부터 설정 가능합니다.");
            return;
        }
        wrongText.SetText("");
        DataManager.Inst.SetBranchPassword(passwordInput.text);

        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[2] { ESiteLink.Branch, Constant.LOADING_DELAY });
    }

}
