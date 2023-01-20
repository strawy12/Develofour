using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TopFileButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TMP_Text fileName;
    [SerializeField]
    private Image fileImage;
    [SerializeField]
    private GameObject highrightedImage;
    private DirectorySO currentDirectory;

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
    }

    private void OpenFIle()
    {
        object[] ps = new object[1] { currentDirectory };
        EventManager.TriggerEvent(ELibraryEvent.OpenFile, ps);
    }


}
