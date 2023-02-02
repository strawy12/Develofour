using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Runtime.CompilerServices;

public class WindowIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform rectTranstform { get; set; }

    private int clickCount = 0;
    protected bool isSelected = false;

    private Window targetWindow = null;
    private Sprite sprite;

    [SerializeField]
    private FileSO fileData;

    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;

    [SerializeField]
    private TMP_Text iconNameText;
    [SerializeField]
    private bool isBackground;

    private bool isInit = false;

    public FileSO File => fileData;


    public void Bind()
    {
        rectTranstform ??= GetComponent<RectTransform>();
    }

    public void Init(bool isBackground = false)
    {
        if (isInit) return;
        isInit = true;

        Bind();

        this.isBackground = isBackground;

        pointerStayImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);

    }

    public void SetFileData(FileSO newFileData)
    {
        if(newFileData == null)
        {
            return;
        }
        fileData = newFileData;
        iconNameText.text = fileData.windowName;
        iconImage.sprite = newFileData.iconSprite;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (clickCount != 0)
            {
                // 여기에서 이벤트 쏨
                clickCount = 0;

                if (targetWindow == null)
                {
                    OpenWindow();
                    //TaskBar.OnAddIcon?.Invoke(targetWindow);
                }
                else
                {
                    targetWindow.WindowOpen();
                }
                UnSelect();
            }
            else
            {
                Select();
                clickCount++;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Select();
            CreateAttributeUI(eventData);
        }
    }

    private void OpenWindow()
    {
        if (fileData is DirectorySO && isBackground == false)
        {
            EventManager.TriggerEvent(ELibraryEvent.IconClickOpenFile, new object[1] { fileData });
            return;
        }

        targetWindow = WindowManager.Inst.WindowOpen(fileData.windowType, fileData);

        if (targetWindow == null)
        {
            return;
        }

        targetWindow.OnClosed += CloseTargetWindow;
        targetWindow.WindowOpen();
    }


    public void SelectedIcon(bool isSelected)
    {
        if (!isSelected)
        {
            clickCount = 0;
        }

        this.isSelected = isSelected;
        selectedImage.gameObject.SetActive(isSelected);
    }

    public void CloseTargetWindow(string a)
    {
        targetWindow.OnClosed -= CloseTargetWindow;
        targetWindow = null;
    }

    private void CreateAttributeUI(PointerEventData eventData)
    {
        Vector3 mousePos = eventData.position;
        mousePos.x -= Constant.MAX_CANVAS_POS.x;
        mousePos.y -= Constant.MAX_CANVAS_POS.y;
        mousePos.z = 0f;

        WindowIconAttributeUI.OnCreateMenu?.Invoke(mousePos, fileData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(false);
    }


    protected virtual void Select()
    {
        EventManager.TriggerEvent(ELibraryEvent.SelectIcon, new object[1] { this });
    }

    protected virtual void UnSelect()
    {
        EventManager.TriggerEvent(ELibraryEvent.SelectNull);
    }
}
