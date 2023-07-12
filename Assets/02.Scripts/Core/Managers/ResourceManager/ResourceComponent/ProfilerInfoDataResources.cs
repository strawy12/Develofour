using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ProfilerInfoDataResources : ResourcesComponent
{
    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<ProfilerInfoDataSO>(callBack);
    }
}
