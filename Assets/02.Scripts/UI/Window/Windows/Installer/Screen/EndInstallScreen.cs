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

        NextBtn.text = "마침";

        NextBtn.onClick.AddListener(InstallerClose);
    }

    private void InstallerClose()
    {
        installer.CheckOpenWindow(windowOpenToggle.isOn);

        installer.WindowClose();
        MonologSystem.OnEndMonologEvent += StartTuto;
    }
    private void StartTuto()
    {
        EventManager.TriggerEvent(ETutorialEvent.TutorialStart, new object[0]);
        EventManager.StopAllListening(ETutorialEvent.TutorialStart);
        MonologSystem.OnEndMonologEvent -= StartTuto;
    }
}
