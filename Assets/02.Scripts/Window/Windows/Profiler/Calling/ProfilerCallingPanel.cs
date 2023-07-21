using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfilerCallingPanel : MonoBehaviour
{
    [SerializeField]
    private ProfilerCallKeyPad phoneCallUI;

    [SerializeField]
    private ProfilerCallUserListPanel userListPanel;

    
    public void Init()
    {
        phoneCallUI.Init();
        userListPanel.Init(phoneCallUI);
    }

    public void Show()
    {
        phoneCallUI.Open();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        phoneCallUI.Close();
        gameObject.SetActive(false);
    }
}
