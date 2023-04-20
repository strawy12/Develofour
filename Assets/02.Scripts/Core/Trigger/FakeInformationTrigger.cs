using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FakeInformationTrigger : InformationTrigger
{
    [SerializeField]
    private EMonologTextDataType monologType;
    [SerializeField]
    private float delay;

    public override void OnPointerClick(PointerEventData eventData)
    {
        MonologSystem.OnStartMonolog?.Invoke(monologType, delay, true);
    }

}
