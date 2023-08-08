using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class TopFileButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TMP_Text fileName;
    [SerializeField]
    private Image fileImage;
    [SerializeField]
    private GameObject highlightedImage;
    public DirectorySO CurrentDirectory => currentDirectory;
    private DirectorySO currentDirectory;

    private RectTransform rectTransform;

    public GameObject tutorialSelectImage;
    public void Init()
    {
        Bind();

        fileName.text = "";
        fileImage.sprite = null;
        fileImage.color = Color.black;
        currentDirectory = null;
    }

    private void Bind()
    {
        rectTransform ??= GetComponent<RectTransform>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OpenFile();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(false);
    }

    public void SetDirectory(DirectorySO directoryData)
    {
        highlightedImage.gameObject.SetActive(false);
        currentDirectory = directoryData;
        fileName.text = directoryData.fileName;
        fileImage.sprite = directoryData.iconSprite;
    }

    private void OpenFile()
    {
        object[] ps = new object[1] { currentDirectory };

        EventManager.TriggerEvent(ELibraryEvent.IconClickOpenFile, ps);
        
    }

    private void OnDestroy()
    {
        //GuideUISystem.EndGuide?.Invoke(rectTransform);
    }
}
