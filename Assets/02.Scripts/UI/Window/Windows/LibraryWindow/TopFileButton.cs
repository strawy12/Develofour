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
        fileName.text = directoryData.windowName;
        fileImage.sprite = directoryData.iconSprite;

        if (currentDirectory.windowName == "User")
        {
            EventManager.StartListening(ETutorialEvent.LibraryUserButtonStart, delegate
            {
                if (gameObject.activeSelf)
                    StartCoroutine(YellowSignCor());

                EventManager.StartListening(ETutorialEvent.LibraryUserButtonEnd, delegate { StopCor(); StopTutorialEvent(); });
            });
        }
    }

    private void OpenFIle()
    {
        object[] ps = new object[1] { currentDirectory };

        EventManager.TriggerEvent(ELibraryEvent.ButtonOpenFile, ps);

        if (currentDirectory.windowName == "User")
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryUserButtonEnd);
        }
    }

    #region Tutorial
    public IEnumerator YellowSignCor()
    {
        yellowUI.gameObject.SetActive(true);
        isSign = true;
        while (isSign)
        {
            yellowUI.DOColor(new Color(255, 255, 255, 0), 2f);
            yield return new WaitForSeconds(2f);
            yellowUI.DOColor(new Color(255, 255, 255, 1), 2f);
            yield return new WaitForSeconds(2f);
        }
    }

    public void StopCor()
    {
        isSign = false;
        StopAllCoroutines();
        yellowUI.gameObject.SetActive(false);
        yellowUI.DOKill();
    }

    private void StopTutorialEvent()
    {
        //EventManager.StopListening(ETutorialEvent.LibraryUserButtonStart, delegate { StartCoroutine(YellowSignCor()); });
        //EventManager.StopListening(ETutorialEvent.LibraryUserButtonEnd, delegate { StopCor(); });

        //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck);
    }
    #endregion
}
