using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartInstallScreen : InstallerScreen
{
    public override void EnterScreen()
    {
        BackBtn.gameObject.SetActive(false);

        UseNextButton();
        // � ��ư���� ����Ǿ���ϴ���
        // �� ��ư�鿡�� � ��ɵ��� �ִ���
        // ���ʿ� �ʿ��� ��ɵ��� �ִ��� 
    }


}
