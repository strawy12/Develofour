using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProfileOverlayOpenTrigger : MonoBehaviour
{
    public int fileID;

    public List<InformationTrigger> triggerCount;

    private bool isSetting;

    public void Open()
    {
        if (!isSetting)
        {
            isSetting = true;
            triggerCount.ForEach((trigger) => { trigger.fileID = fileID; });
        }
        ProfileOverlaySystem.OnOpen?.Invoke(fileID, triggerCount);
    }

    public void OpenByIntList(int fileID, List<int> listInt)
    {
        ProfileOverlaySystem.OnOpenInt?.Invoke(fileID, listInt);
    }

    //private void CheckClose(object[] hits)
    //{
    //    if (Define.ExistInHits(gameObject, hits[0]) == false)
    //    {
    //        Close();
    //    }
    //}

    public void Close()
    {
        ProfileOverlaySystem.OnClose?.Invoke();
    }

    //다른거 누를때 꺼지게하는거
}