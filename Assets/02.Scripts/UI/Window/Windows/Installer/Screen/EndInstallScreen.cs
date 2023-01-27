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
        if(windowOpenToggle.isOn)
        {
            // 여기서 투두 프로그램 깔거고, 
            // fileSO 추가해줄거임
            //WindowManager.Inst.OpenWindow()
        }

        installer.WindowClose();
    }

}
