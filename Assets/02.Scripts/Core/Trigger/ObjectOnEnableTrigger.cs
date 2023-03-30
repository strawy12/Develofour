using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnEnableTrigger : MonoBehaviour
{
    public EProfileCategory category;
    public string information;

    private void OnEnable()
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
    }
}
