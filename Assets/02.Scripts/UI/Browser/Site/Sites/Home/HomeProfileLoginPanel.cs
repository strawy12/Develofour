using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeProfileLoginPanel : MonoBehaviour
{
    public Button loginButton;

    public void Init()
    {
        loginButton.onClick.AddListener(ChangeLoginSite);
        CheckLogin();
    }

    private void OnEnable()
    {
        CheckLogin();
    }

    private void CheckLogin()
    {
        if (DataManager.GetSaveData<bool>(ESaveDataType.IsWindowsLoginAdminMode))
        {
            loginButton.gameObject.SetActive(false);
        }
        else
        {
            loginButton.gameObject.SetActive(true);
        }
    }

    public void ChangeLoginSite()
    {
        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.GoogleLogin, Constant.LOADING_DELAY });
    }

}
