using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class InstallerScreen : MonoBehaviour
{
    protected InstallerWindow installer;
    protected Button BackBtn => installer.BackBtn;
    protected TextButton NextBtn => installer.NextBtn;

    public void Init(InstallerWindow installer)
    {
        this.installer = installer;
    }

    public abstract void EnterScreen();

    protected void UseNextButton()
    {
        NextBtn.onClick.RemoveAllListeners();
        NextBtn.onClick.AddListener(installer.NextScreen);

        NextBtn.gameObject.SetActive(true);
    }

    protected void UseBackButton()
    {
        BackBtn.onClick.RemoveAllListeners();
        BackBtn.onClick.AddListener(installer.BackScreen);

        BackBtn.gameObject.SetActive(true);
    }
}
