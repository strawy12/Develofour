using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    [HideInInspector]
    public Dictionary<string, ImageViewerDataSO> imageFile;

    private void Awake()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        
    }

}
