using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchNewPasswordSite : Site
{
    [Header("��й�ȣ")]
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
            wrongText.SetText("��й�ȣ�� 3�ڸ� �̻���� ���� �����մϴ�.");
            return;
        }
        wrongText.SetText("");
        DataManager.Inst.SetBranchPassword(passwordInput.text);

        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[2] { ESiteLink.Branch, Constant.LOADING_DELAY });
    }

}
