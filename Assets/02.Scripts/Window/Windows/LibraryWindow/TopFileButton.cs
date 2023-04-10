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
    private DirectorySO currentDirectory;

    [Header("Tutorial")]

    [SerializeField]
    private Image yellowUI;

    private bool isSign;
    private RectTransform rectTransform;
    public void Init()
    {
        Bind();

        fileName.text = "";
        fileImage.sprite = null;
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

        if (currentDirectory.GetFileLocation() == "User\\")
        {
            EventManager.StartListening(ETutorialEvent.LibraryUserButtonStart, delegate
            {
                if(gameObject.activeSelf)
                {
                    GuideUISystem.OnGuide?.Invoke(rectTransform);
                    EventManager.StartListening(ETutorialEvent.LibraryUserButtonEnd, delegate { StopTutorialEvent(); });
                }
            });
        }
    }

    private void OpenFile()
    {
        EventManager.TriggerEvent(ELibraryEvent.AddUndoStack);
        EventManager.TriggerEvent(ELibraryEvent.ResetRedoStack);
        object[] ps = new object[1] { currentDirectory };

        EventManager.TriggerEvent(ELibraryEvent.ButtonOpenFile, ps);
        
        if (currentDirectory.fileName == "User\\")
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryUserButtonEnd);
        }
    }

    #region Tutorial

    private void StopTutorialEvent()
    {
        GuideUISystem.EndGuide?.Invoke();
    }
    #endregion


    private void OnDestroy()
    {
        EventManager.StopAllListening(ETutorialEvent.LibraryUserButtonStart);
        GuideUISystem.EndGuide?.Invoke();
    }
}
