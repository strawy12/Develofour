using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BranchFindPasswordPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField branchIDField;
    [SerializeField]
    private Button checkBtn;
    [SerializeField]
    private TMP_Text signText;
    private string id;
    public void Init(string id)
    {
        this.id = id;
        checkBtn.onClick.AddListener(CheckID);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void CheckID()
    {
        if(branchIDField.text == id)
        {
            Success();
        }
    }

    private void Success()
    {
        signText.SetText("��й�ȣ ��������� �����߽��ϴ�.");
        EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[2] { EMailType.BranchCertificationMail, 0.5f});
    }

}
