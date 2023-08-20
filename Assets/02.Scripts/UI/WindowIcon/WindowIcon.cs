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
        rectTranstform.localScale = Vector3.one;
        Debug.Log(rectTranstform.localScale.z);
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

        iconImage.rectTransform.sizeDelta = new Vector3(x2, y2, 1);
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
        rectTranstform.localScale = Vector3.one;
        Debug.Log(rectTranstform.localScale.z);
    }

    public void ChangeIcon(Sprite icon, Color color)
    {
        iconImage.sprite = icon;

        iconImage.color = color;
    }

    private void YellowUI(object[] obj)
    {
        GuideUISystem.OnEndAllGuide?.Invoke();
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
                    targetWindow.WindowOpen(false);
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
        iconNameText.text = "";
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

        if(DataManager.Inst.SaveData.isProfilerInstall == false) //튜토리얼 체크
        {
            if(fileData.windowType != EWindowType.Directory 
                && fileData.windowType != EWindowType.ProfilerWindow 
                && fileData.windowType != EWindowType.Installer)
            {
                MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.NOTPROFILERINSTALL, 0, false);
                return;
            }
            Debug.Log("중간 포인트");
            if(fileData.windowType == EWindowType.Directory)
            {
                if (fileData.id != Constant.FileID.MYPC
                    && fileData.id != Constant.FileID.USB
                    && fileData.id != Constant.FileID.BACKGROUND)
                {
                    MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.NOTPROFILERINSTALL, 0, false);
                    return;
                }
            }
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
        Debug.Log("SelectedImage의 bool값은 " + selectedImage.gameObject.activeSelf + "     기본 값 " + isSelected);
    }

    public void CloseTargetWindow(string a)
    {
        if (targetWindow == null) return;
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

    }
}

