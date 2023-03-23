using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
public class TopFileButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TMP_Text fileName;
    [SerializeField]
    private Image fileImage;
    [SerializeField]
    private GameObject highrightedImage;
    private DirectorySO currentDirectory;

    [Header("Tutorial")]

    [SerializeField]
    private Image yellowUI;

    private bool isSign;

    public void Init()
    {
        fileName.text = "";
        fileImage.sprite = null;
        currentDirectory = null;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OpenFIle();
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        highrightedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highrightedImage.gameObject.SetActive(false);
    }

    public void SetDirectory(DirectorySO directoryData)
    {
        highrightedImage.gameObject.SetActive(false);
        currentDirectory = directoryData;
        fileName.text = directoryData.fileName;
        fileImage.sprite = directoryData.iconSprite;

        if (currentDirectory.fileName == "User\\")
        {
            EventManager.StartListening(ETutorialEvent.LibraryUserButtonStart, delegate
            {
                if (gameObject.activeSelf)
                    GuideUISystem.OnGuide?.Invoke((RectTransform)transform);

                EventManager.StartListening(ETutorialEvent.LibraryUserButtonEnd, delegate { StopTutorialEvent(); });
            });
        }
    }

    private void OpenFIle()
    {
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
        //EventManager.StopListening(ETutorialEvent.LibraryUserButtonStart, delegate { StartCoroutine(YellowSignCor()); });
        //EventManager.StopListening(ETutorialEvent.LibraryUserButtonEnd, delegate { StopCor(); });

        //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck);
        GuideUISystem.EndGuide?.Invoke();
    }
    #endregion
}
