using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : Window
{
    [SerializeField]
    private ImageViewerDataSO imageData;

    [SerializeField]
    private Image imageViewerImage;

    [SerializeField]
    private ImageEnlargement imageEnlargement;

    private readonly Vector2 MAXSIZE = new Vector2(1173.333f, 660f);

    private const float RATIO = 1.636363636363636f;

    private float saveImageScale = 1f;

    protected override void Init()
    {
        base.Init();

        imageData = ResourceManager.Inst.GetImageData(file.name);

        windowBar.SetNameText($"{imageData.imageName}.{imageData.extensionType.ToString().ToLower()}");

        imageViewerImage.sprite = imageData.imageSprite;

        SetImageSizeReset();
    }

    public void SetImageSizeReset()
    {
        Vector2 size = imageViewerImage.sprite.rect.size;
        Vector2 originSize = size;

        size.x /= RATIO;
        size.y /= RATIO;

        imageViewerImage.rectTransform.sizeDelta = size;

        float scale = 1f;
        if(size.y > MAXSIZE.y)
        {
            scale = MAXSIZE.y / size.y;
        }
        else if(size.x > MAXSIZE.x)
        {
            scale = MAXSIZE.x / size.x;
        }

        imageViewerImage.transform.localScale = Vector3.one * scale;

        imageEnlargement.imageScale = imageViewerImage.transform.localScale.x;
    }
}
