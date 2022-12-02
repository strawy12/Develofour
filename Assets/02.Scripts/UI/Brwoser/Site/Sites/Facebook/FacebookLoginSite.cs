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
    private TMP_InputField facebookpasswordInputField;
    [SerializeField]
    private TextMove textMove;

    private ESiteLink requestSite;


    public override void Init()
    {
        base.Init();
        facebookIDInputField.asteriskChar = '¡¤';
        facebookpasswordInputField.asteriskChar = '¡¤';

        LoginBtn.onClick.AddListener(Login);
        facebookIDInputField.onSelect.AddListener((a) => SelectInputField(true));
        facebookIDInputField.onDeselect.AddListener((a) => SelectInputField(false));

        facebookpasswordInputField.contentType = TMP_InputField.ContentType.Password;
        facebookpasswordInputField.ActivateInputField();
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
        if(facebookIDInputField.text == loginID)
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
            
        }
    }
}
