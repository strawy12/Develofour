using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewerBody : MonoBehaviour
{
    private Image currentImage;
    public ImageEnlargement imageEnlargement { get; private set; }

    public Sprite sprite => currentImage.sprite;
    public RectTransform rectTransform => currentImage.rectTransform;

    public void Init()
    {
        currentImage = GetComponent<Image>();
        imageEnlargement = GetComponent<ImageEnlargement>();
    }
}
