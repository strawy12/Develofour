using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowLoginMonologObj : MonoBehaviour
{
    public void SeccessLogin()
    {
        StartCoroutine(LoginCoroutine());
    }


    private IEnumerator LoginCoroutine()
    {
        yield return new WaitForSeconds(1f);
        EventManager.TriggerEvent(ECoreEvent.OpenTextBox, new object[2] { ETextDataType.USBMonolog, TextBox.ETextBoxType.Simple });
        yield return new WaitForSeconds(2f);
        NoticeSystem.OnGeneratedNotice(ENoticeType.ConnectUSB, 0f);
    }
}
