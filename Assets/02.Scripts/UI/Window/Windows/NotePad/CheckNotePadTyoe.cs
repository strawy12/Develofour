using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckNotePadTyoe : MonoBehaviour
{
    private FileSO currentFile;

    private void Awake()
    {

    }
    public void Setting(FileSO file)
    {
        currentFile = file;
        CheckFileOwnerInfo();
    }

    private void CheckFileOwnerInfo()
    {
        Debug.Log("의뢰자 정보 메모장 클릭");
        if (currentFile.windowName == "의뢰자 정보")
        {
            EventManager.TriggerEvent(EDecisionEvent.ClickOwnerNameText);
        }
    }
}
