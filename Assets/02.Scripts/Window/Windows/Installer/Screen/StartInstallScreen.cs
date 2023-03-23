using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInstallScreen : InstallerScreen
{
    public override void EnterScreen()
    {
        BackBtn.gameObject.SetActive(false);

        UseNextButton();
        // 어떤 버튼들이 실행되어야하는지
        // 그 버튼들에는 어떤 기능들이 있는지
        // 안쪽에 필요한 기능들이 있는지 
    }


}
