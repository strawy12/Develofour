using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public enum EImageExtensionType
{
    Png,
    Jpg,
    Gif,
    Bmp,
}

[CreateAssetMenu(menuName = "SO/Window/ImageViewer/Data")]  
public class ImageViewerDataSO : ScriptableObject
{
    public int fileId;
    public string imageName;
    public ImageViewerBody imageBody;
    public Sprite sprite;
    public EImageExtensionType extensionType;

    [ContextMenu("SetSprite")]
    public void SetSprite()
    {
        if(imageBody != null)
        {
            sprite = imageBody.GetComponent<UnityEngine.UI.Image>().sprite;
        }
    }
}
