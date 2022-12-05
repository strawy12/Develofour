using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DebugEvent
{
    public KeyCode keyCode;
    public UnityEvent debugEvent;
}

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private List<DebugEvent> debugEventList;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSiteAll);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            EventManager.TriggerEvent(ECutSceneEvent.SkipCutScene);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Time.timeScale = 10;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[] { EMailType.Default });
        }

        foreach (DebugEvent e in debugEventList)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                e.debugEvent?.Invoke();
            }
        }
    }

}
