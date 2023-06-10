using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProfileOverlayOpenTrigger : MonoBehaviour, IPointerClickHandler
{
    public int fileID;

    public List<InformationTrigger> triggerCount;

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        ProfileOverlaySystem.OnOpen?.Invoke(fileID, triggerCount);
    }

    private void CheckClose(object[] hits)
    {
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        ProfileOverlaySystem.OnClose?.Invoke();
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    //다른거 누를때 꺼지게하는거
}
