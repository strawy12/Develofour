using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
    public string fileName;
    public string imageName;
    public Sprite imageSprite;
    public EImageExtensionType extensionType;
}
