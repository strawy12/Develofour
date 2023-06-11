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
        if(!isSetting)
        {
            isSetting = true;
            triggerCount.ForEach((trigger) => { trigger.fildID = fileID; });
        }
        ProfileOverlaySystem.OnOpen?.Invoke(fileID, triggerCount);
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
