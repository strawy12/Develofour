using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EImageExtensionType
{
    Png,
    Jpeg,
    Jpg,
    Gif,
    Bmp,
}

[CreateAssetMenu(menuName = "SO/Window/ImageViewer/Data")] 
public class ImageViewerDataSO : ScriptableObject
{
    public Sprite imageSprite;
    public string imageName;
    public EImageExtensionType extensionType;

    public string imagePin;
}
