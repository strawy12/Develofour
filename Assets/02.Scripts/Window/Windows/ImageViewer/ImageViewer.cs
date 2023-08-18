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

        imageData = ResourceManager.Inst.GetImageViewerData(file.id);
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
        if (imageData.fileId == Constant.FileID.INCIDENTREPORT && DataManager.Inst.IsProfilerTutorial())
        {
            OnSelected += (() => GuideUISystem.OnEndAllGuide?.Invoke());
        }
        OnUnSelected += OverlayClose;
        OverlayOpen();
    }

    public override void WindowOpen()
    {
        base.WindowOpen();
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
        OnSelected = null;
        OnUnSelected = null;
    }
}
