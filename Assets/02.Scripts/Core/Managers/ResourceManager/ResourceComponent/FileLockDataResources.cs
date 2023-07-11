using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class FileLockDataResources : ResourcesComponent
{
    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<PinLockDataSO>(callBack);
    }
}
