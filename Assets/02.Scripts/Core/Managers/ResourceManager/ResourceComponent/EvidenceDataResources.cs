using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvidenceDataResources : ResourcesComponent
{
    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<CharacterInfoDataSO>(callBack);
    }
}
