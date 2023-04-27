using Cinemachine;
using System.Collections;
using System.Collections.Generic; 
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageViewer : Window
{
    [Header("EnlargementUI")]
    [SerializeField]
    private Button enlargementButton;
    [SerializeField]
    private Button reductionButton;

    [SerializeField]
    private ImageViewerBody imageViewerBody;

    [SerializeField]
    private TMP_Text imagePercentText;

    private ImageEnlargement imageEnlargement;
    private ImageViewerDataSO imageData;

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

        enlargementButton.onClick?.AddListener(EnlargementButtonClick);
        reductionButton.onClick?.AddListener(ReductionButton);
    }

    private void EnlargementButtonClick()
    {
        imageEnlargement.enlargementClick?.Invoke();
    }

    private void ReductionButton()
    {
        imageEnlargement.reductionClick?.Invoke();
    }
}
