using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndInstallScreen : InstallerScreen
{
    [SerializeField]
    private Toggle windowOpenToggle;
    

    public override void EnterScreen()
    {
        NextBtn.gameObject.SetActive(true);
        NextBtn.interactable = true;
        NextBtn.onClick.RemoveAllListeners();

        NextBtn.text = "¸¶Ä§";

        NextBtn.onClick.AddListener(InstallerClose);
    }

    private void InstallerClose()
    {
        installer.EndInstall(windowOpenToggle.isOn);
        installer.WindowClose();
    }

}
