using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnEnableTrigger : InformationTrigger
{


    //디버그 안해본 코드. 오류 가능성 있음.

    private void OnEnable()
    {
        GetInfo();
    }
}
