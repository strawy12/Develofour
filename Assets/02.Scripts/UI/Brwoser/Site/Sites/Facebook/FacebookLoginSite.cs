using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FacebookLoginSite : Site
{

    [SerializeField]
    private string loginID;
    [SerializeField]
    private string passWord;
    [SerializeField]
    private Button LoginBtn;
    [SerializeField]
    private TMP_InputField facebookIDInputField;
    [SerializeField]
    private TMP_InputField facebookPasswordInputField;
    [SerializeField]
    private TextMove textMove;

    private ESiteLink requestSite;


    public override void Init()
    {
        base.Init();
        facebookIDInputField.asteriskChar = '·';
        facebookPasswordInputField.asteriskChar = '·';

        LoginBtn.onClick.AddListener(Login);
        facebookIDInputField.onSelect.AddListener((a) => SelectInputField(true));
        facebookIDInputField.onDeselect.AddListener((a) => SelectInputField(false));

        facebookPasswordInputField.contentType = TMP_InputField.ContentType.Password;
        facebookPasswordInputField.ActivateInputField();
    }

    private void SelectInputField(bool isSelected)
    {

        Input.imeCompositionMode = isSelected ? IMECompositionMode.Off : IMECompositionMode.Auto;
        textMove.PlaceholderEffect(isSelected);
    }

    private void Update()
    {

    }

    private void Login()
    {
        if (facebookIDInputField.text == loginID)
        {
            if(facebookPasswordInputField.text == passWord)
            {
                if (requestSite == ESiteLink.None)
                {
                    EventManager.TriggerEvent(EBrowserEvent.OnUndoSite);
                }
                else
                {
                    EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { requestSite, Constant.LOADING_DELAY });
                    requestSite = ESiteLink.None;
                }
            }
            else
            {
                //비밀번호가 달라 로그인실패
            }
        }
        else
        {
            //아이디가 달라 로그인실패
        }
    }
}
