using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : Window
{
    [SerializeField]
    private ImageViewerBody imageViewerBody;

    [SerializeField]
    private TMP_Text imagePercentText;

    private ImageEnlargement imageEnlargement;

    private ImageViewerDataSO imageData;

    private readonly Vector2 MAXSIZE = new Vector2(1173.333f, 660f);

    private const float RATIO = 1.636363636363636f;

    private float saveImageScale = 1f;

    protected override void Init()
    {
        base.Init();

        imageData = ResourceManager.Inst.GetImageViewerData(file.GetFileLocation());

        windowBar.SetNameText($"{imageData.imageName}.{imageData.extensionType.ToString().ToLower()}");

        if(imageData.imageBody != null)
        {
            Transform parent = imageViewerBody.transform.parent;
            Destroy(imageViewerBody.gameObject);
            imageViewerBody = Instantiate(imageData.imageBody, parent);
        }
        imageViewerBody.Init();

        imageEnlargement = imageViewerBody.imageEnlargement;
        imageEnlargement.Init(imagePercentText);

        SetImageSizeReset();
        imageEnlargement.ReSetting();
    }

    public void SetImageSizeReset()
    {
        Vector2 size = imageViewerBody.sprite.rect.size;
        Vector2 originSize = size;

        size.x /= RATIO;
        size.y /= RATIO;

        imageViewerBody.rectTransform.sizeDelta = size;

        float scale = 1f;
        if(size.y > MAXSIZE.y)
        {
            scale = MAXSIZE.y / size.y;
        }
        else if(size.x > MAXSIZE.x)
        {
            scale = MAXSIZE.x / size.x;
        }
        imageViewerBody.transform.localScale = Vector3.one * scale;

        imageEnlargement.imageScale = imageViewerBody.transform.localScale.x;
    }
}
