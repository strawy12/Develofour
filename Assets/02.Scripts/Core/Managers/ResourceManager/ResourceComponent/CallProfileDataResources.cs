using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CallProfileDataResources : ResourcesComponent
{
    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<CallProfileDataSO>(callBack);
    }
}
