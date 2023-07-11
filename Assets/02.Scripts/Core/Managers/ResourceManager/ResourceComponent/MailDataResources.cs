using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class MailDataResources : ResourcesComponent
{
    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<MailDataSO>(callBack);
    }
}
