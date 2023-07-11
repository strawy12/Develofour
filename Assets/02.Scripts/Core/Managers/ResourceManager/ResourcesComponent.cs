using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ResourcesComponent : MonoBehaviour
{
    public string componentName;

    public Type soType;

    protected Dictionary<string, ResourceSO> resourceDictionary;

    public virtual async void LoadResourceDataAssets(Action callBack)
    {
        
    }

    public ResourceSO GetResource(string key)  
    {
        if (!resourceDictionary.ContainsKey(key)) return null;

        return resourceDictionary[key];
    }

    public Dictionary<string, ResourceSO> GetRsourceDictionary()
    {
        return resourceDictionary;
    }
}


