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
            InformationTrigger[] triggerArray = GetComponentsInChildren<InformationTrigger>();

            triggerCount.Clear();
            for(int i = 0; i < triggerArray.Length; i++)
            {
                triggerCount.Add(triggerArray[i]);
            }

            if(triggerCount == null || triggerCount.Count == 0)
            {
                return;
            }

            if(triggerCount.Count != 0)
            {
                fileID = triggerCount[0].fileID;
            }
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