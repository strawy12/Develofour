using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class MonologTextDataResources : ResourcesComponent
{
    public int MonologDataListCount => resourceDictionary.Count;

    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<MonologTextDataSO>(callBack);
    }
}
