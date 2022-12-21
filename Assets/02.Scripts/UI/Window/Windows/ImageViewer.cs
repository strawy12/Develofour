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
    private Image imageViwerImage;

    private readonly Vector2 MAXSIZE = new Vector2(1173.333f, 660f);

    private const float RATIO = 1.636363636363636f;

    protected override void Init()
    {
        base.Init();

        windowBar.SetNameText($"{imageData.imageName}.{imageData.extensionType.ToString().ToLower()}");

        imageViwerImage.sprite = imageData.imageSprite;

        SetImageResolusion();
    }


    public void SetImageResolusion()
    {
        Vector2 size = imageViwerImage.sprite.rect.size;
        Vector2 originSize = size;

        size.x /= RATIO;
        size.y /= RATIO;

        if(size.y > MAXSIZE.y)
        {
            size.y = MAXSIZE.y;
            size.x = (originSize.x * size.y) / originSize.y;
        }

        else if(size.x > MAXSIZE.x)
        {
            size.x = MAXSIZE.x;
            size.y = (size.x * originSize.y) / originSize.x;
        }

        imageViwerImage.rectTransform.sizeDelta = size;
    }

    

}
