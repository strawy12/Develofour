using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class MediaPlayerDataResources : ResourcesComponent
{

    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<MediaPlayerDataSO>(callBack);
    }
}
