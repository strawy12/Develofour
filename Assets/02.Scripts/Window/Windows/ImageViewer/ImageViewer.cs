using Cinemachine;
using System;
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

    [SerializeField]
    private ProfileOverlayOpenTrigger overlayTrigger;

    protected override void Init()
    {
        base.Init();

        imageData = ResourceManager.Inst.GetResource<ImageViewerDataSO>(file.ID);
        Debug.Log(imageData);
        windowBar.SetNameText($"{imageData.imageName}.{imageData.extensionType.ToString().ToLower()}");

        if(imageData.imageBody != null)
        {
            Transform parent = imageViewerBody.transform.parent;
            Destroy(imageViewerBody.gameObject);
            imageViewerBody = Instantiate(imageData.imageBody, parent);
        }
       
        imageViewerBody.Init();

        imageEnlargement = imageViewerBody.imageEnlargement;
        imageEnlargement.Init(imagePercentText, imageData.sprite);

        enlargementButton.onClick?.AddListener(EnlargementButtonClick);
        reductionButton.onClick?.AddListener(ReductionButton);

        OnSelected += OverlayOpen;
        OnUnSelected += OverlayClose;
        OverlayOpen();
    }

    private void OverlayClose()
    {
        if (overlayTrigger == null) // 없다면 찾아와
        {
            overlayTrigger = imageViewerBody.GetComponent<ProfileOverlayOpenTrigger>();
            if (overlayTrigger == null) { return; }
        }
        overlayTrigger.Close();
    }

    private void OverlayOpen()
    {

        if (overlayTrigger == null) // 없다면 찾아와
        {
            overlayTrigger = imageViewerBody.GetComponent<ProfileOverlayOpenTrigger>();
            if (overlayTrigger == null) { return; }
        }
        overlayTrigger.Open();
    }

    private void EnlargementButtonClick()
    {
        imageEnlargement.enlargementClick?.Invoke();
    }

    private void ReductionButton()
    {
        imageEnlargement.reductionClick?.Invoke();
    }

    protected override void OnDestroyWindow()
    {
        base.OnDestroyWindow();
        OnSelected -= OverlayOpen;
        OnUnSelected -= OverlayClose;
    }
}
