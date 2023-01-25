using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private List<ImageViewerDataSO> imageVierwerList = new List<ImageViewerDataSO>();

    private Dictionary<string, ImageViewerDataSO> imageFileDictionary = new Dictionary<string, ImageViewerDataSO>();

    private void Awake()
    {
        InitDictionary();
    }

    private void InitDictionary()
    {
        foreach(ImageViewerDataSO imageData in imageVierwerList)
        {
            imageFileDictionary.Add(imageData.name, imageData);
        }
    }

    public ImageViewerDataSO SetImageData(string windowName)
    {
        return imageFileDictionary[windowName];
    }
}
