using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;
using System.Runtime.CompilerServices;
using DG.Tweening;

public class WindowIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public RectTransform rectTranstform { get; set; }

    protected int clickCount = 0;
    protected bool isSelected = false;

    private Window targetWindow = null;

    [SerializeField]
    private FileSO fileData;

    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image selectedImage;
    
    [SerializeField]
    private Image pointerStayImage;
    public Image PointerStayImage => pointerStayImage;
    [SerializeField]
    private TMP_Text iconNameText;
    [SerializeField]
    private bool isBackground;

    private bool isInit = false;
    public FileSO File => fileData;

    private int IconDefaultSize => isBackground ? 60 : 100;

    public void Bind()
    {
        rectTranstform = GetComponent<RectTransform>();
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

    public void SetFileData(FileSO newFileData, float size = 0f)
    {
        if (newFileData == null)
        {
            return;
        }

        if(size == 0f)
        {
            size = IconDefaultSize;
        }
        fileData = newFileData;
        iconNameText.text = fileData.fileName;
        float x1, y1, x2, y2;
        if(newFileData.iconSprite == null)
        {
            return;
        } 
        if (newFileData.iconSprite.rect.width != newFileData.iconSprite.rect.height)
        {
            x1 = newFileData.iconSprite.rect.width;
            y1 = newFileData.iconSprite.rect.height;
            if (x1 > y1)
            {
                x2 = size;
                y2 = y1 * x2 / x1;
            }
            else
            {
                y2 = size;
                x2 = x1 * y2 / y1;
            }
        }
        else
        {
            x2 = y2 = size;
        }

        iconImage.rectTransform.sizeDelta = new Vector2(x2, y2);

        iconImage.sprite = newFileData.iconSprite;

        iconImage.color = newFileData.color;

        if (fileData.id == 6 ) //usb
        {
            EventManager.StopListening(ETutorialEvent.USBTutorial, YellowUI);
            EventManager.StartListening(ETutorialEvent.USBTutorial, YellowUI);
        }

        if (fileData.id == 5 ) //사건보고서
        {
            EventManager.StopListening(ETutorialEvent.ReportTutorial, YellowUI);
            EventManager.StartListening(ETutorialEvent.ReportTutorial, YellowUI);
        }
        //데이터 매니저로 확인하고
        //gamestate가 튜토리얼이라면
        //library, usb, report에 각각 이벤트 넣어줘~
    }

    public void ChangeIcon(Sprite icon, Color color)
    {
        iconImage.sprite = icon;

        iconImage.color = color;
    }

    private void YellowUI(object[] obj)
    {
        GuideUISystem.EndAllGuide?.Invoke();
        GuideUISystem.OnGuide(this.rectTranstform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (clickCount != 0)
            {
                EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
                // 여기에서 이벤트 쏨
                clickCount = 0;

                if (targetWindow == null)
                {
                    OpenWindow();
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
                EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
                EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
            }

        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Select();
            CreateAttributeUI(eventData);
            EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
            EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        }
    }

    private void CheckClose(object[] hits)
    {
        if (Define.ExistInFirstHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        UnSelect();
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    public void Release()
    {
        UnSelect();
        fileData = null;
        iconImage.sprite = null;
        targetWindow = null;
        
    }

    private void OpenWindow()
    {
        WindowLockDataSO windowLock = ResourceManager.Inst.GetFileLockData(fileData.id);
        bool isLock = false;

        if (windowLock != null)
        {
            isLock = true;
        }


        if (fileData.windowType == EWindowType.Dummy)
        {
            return;
        }

        if (fileData is DirectorySO && isBackground == false)
        {
            if (isLock && DataManager.Inst.IsFileLock(fileData.id))
            {
                targetWindow = WindowManager.Inst.WindowOpen(EWindowType.WindowPinLock, fileData);
            }
            else
            {
                Library window;
                if(WindowManager.Inst.CurrentWindowCount(EWindowType.Directory) == 0)
                {
                    window = WindowManager.Inst.CreateWindow(EWindowType.Directory, fileData) as Library;

                }
                else
                {
                    window = WindowManager.Inst.WindowOpen(EWindowType.Directory, fileData) as Library;
                }
            }
            return;
        }

        targetWindow = WindowManager.Inst.WindowOpen(fileData.windowType, fileData);

        if (targetWindow == null || targetWindow.File.windowType == EWindowType.WindowPinLock)
        {
            return;
        }

        targetWindow.OnClosed += CloseTargetWindow;

    }

    public void SelectedIcon(bool isSelected)
    {
        if (!isSelected)
        {
            clickCount = 0;
        }
        else
        {
            pointerStayImage.gameObject.SetActive(false);
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
        if(isSelected)
        {
            pointerStayImage.gameObject.SetActive(false);
            return;
        }
        pointerStayImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(false);
    }

    private bool isSign;


    protected virtual void Select()
    {
        EventManager.TriggerEvent(ELibraryEvent.SelectIcon, new object[1] { this });
    }

    protected virtual void UnSelect()
    {
        EventManager.TriggerEvent(ELibraryEvent.SelectNull);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        if (GameManager.Inst.isApplicationQuit) return;
        //GuideUISystem.EndGuide?.Invoke(rectTranstform);
    }
}

