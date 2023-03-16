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

    private int clickCount = 0;
    protected bool isSelected = false;

    private Window targetWindow = null;
    private Sprite sprite;

    public Image yellowUI;

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
    private bool isRegisterEvent = false;
    private bool isUSBEvent = false;
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
        if (newFileData == null)
        {
            return;
        }
        fileData = newFileData;
        iconNameText.text = fileData.fileName;

        float x1, y1, x2, y2;

        if (newFileData.iconSprite.rect.width != newFileData.iconSprite.rect.height)
        {
            x1 = newFileData.iconSprite.rect.width;
            y1 = newFileData.iconSprite.rect.height;
            if (x1 > y1)
            {
                x2 = 100;
                y2 = y1 * x2 / x1;
            }
            else
            {
                y2 = 100;
                x2 = x1 * y2 / y1;
            }
            iconImage.rectTransform.sizeDelta = new Vector2(x2, y2);
        }

        iconImage.sprite = newFileData.iconSprite;
      
        if (isRegisterEvent == false && fileData.fileName == "������ �������Ϸ�")    
        {
            isRegisterEvent = true;
            EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoStart, LibraryRequesterInfoStart);
        }

        if (isUSBEvent == false && fileData.fileName == "BestUSB")
        {
            isUSBEvent = true;
            EventManager.StartListening(ETutorialEvent.LibraryUSBStart, LibraryUSBStart);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (clickCount != 0)
            {
                EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
                // ���⿡�� �̺�Ʈ ��
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

                if (fileData.fileName == "�Ƿ��� ����")
                {
                    Debug.Log("�Ƿ��� ���� " + GameManager.Inst.GameState);
                    if(GameManager.Inst.GameState == EGameState.Tutorial)
                    {
                        EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoEnd);
                    }
                }
                if (fileData.fileName == "BestUSB")
                {
                    EventManager.TriggerEvent(ETutorialEvent.LibraryUSBEnd);
                }
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
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        UnSelect(); 
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    private void OpenWindow()
    {
        if (fileData is DirectorySO && isBackground == false)
        {
            EventManager.TriggerEvent(ELibraryEvent.IconClickOpenFile, new object[1] { fileData });
            return;
        }

        targetWindow = WindowManager.Inst.WindowOpen(fileData.windowType, fileData);

        if (targetWindow == null || targetWindow.File.windowType == EWindowType.WindowPinLock )
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

    void Start()
    {
        if (gameObject.name == "ExplorerIcon")
        {
            EventManager.StartListening(ETutorialEvent.BackgroundSignStart, BackgroundSignStart);
        }
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

    #region Tutorial
    public IEnumerator YellowSignCor()
    {
        yellowUI.gameObject.SetActive(true);
        isSign = true;
        while (isSign)
        {
            yellowUI.DOColor(new Color(255, 255, 255, 0.5f), 2f);
            yield return new WaitForSeconds(2f);
            yellowUI.DOColor(new Color(255, 255, 255, 1), 2f);
            yield return new WaitForSeconds(2f);
        }
    }

    public void StopYellowUICor()
    {
        isSign = false;
        StopAllCoroutines();
        yellowUI.gameObject.SetActive(false);
        yellowUI.DOKill();
    }

    private void BackgroundEventStop()
    {
        EventManager.StopListening(ETutorialEvent.BackgroundSignStart, BackgroundSignStart);
        EventManager.StopListening(ETutorialEvent.BackgroundSignEnd, delegate { StopYellowUICor(); });

        Debug.Log("library");
        EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck);
    }

    private void RequesterInfoEventStop()
    {
        //EventManager.StopListening(ETutorialEvent.LibraryRequesterInfoStart, LibraryRequesterInfoStart);
        //EventManager.StopListening(ETutorialEvent.LibraryRequesterInfoEnd, delegate { StopCor(); });
    }

    private void USBEventStop()
    {
        //EventManager.StopListening(ETutorialEvent.LibraryUSBStart, LibraryUSBStart);
        //EventManager.StopListening(ETutorialEvent.LibraryUSBEnd, delegate { StopCor(); });

        //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck);
    }

    public void LibraryUSBStart(object[] ps)
    {
        if (this.gameObject != null)
        {
            if (gameObject.activeSelf) StartCoroutine(YellowSignCor());
        }
        EventManager.StartListening(ETutorialEvent.LibraryUSBEnd, delegate { StopYellowUICor(); USBEventStop(); });
    }

    public void LibraryRequesterInfoStart(object[] ps)
    {
        if (this.gameObject != null)
        {
            if (gameObject.activeSelf)
                StartCoroutine(YellowSignCor());
        }
        EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoEnd, delegate { StopYellowUICor(); RequesterInfoEventStop(); });
    }

    public void BackgroundSignStart(object[] ps)
    {
        StartCoroutine(YellowSignCor());
        
        EventManager.StartListening(ETutorialEvent.BackgroundSignEnd, delegate { StopYellowUICor(); BackgroundEventStop(); });
    }
    #endregion
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    private void OnDestroy()
    {
        StopAllCoroutines();

        EventManager.StopListening(ETutorialEvent.LibraryUSBStart, LibraryUSBStart);
        EventManager.StopListening(ETutorialEvent.LibraryUSBEnd, delegate { StopYellowUICor(); });
        EventManager.StopListening(ETutorialEvent.LibraryRequesterInfoStart, LibraryRequesterInfoStart);
        EventManager.StopListening(ETutorialEvent.LibraryRequesterInfoEnd, delegate { StopYellowUICor(); });
        //EventManager.StopListening(ETutorialEvent.BackgroundSignStart, delegate { StartCoroutine(YellowSignCor()); });
        EventManager.StopListening(ETutorialEvent.BackgroundSignEnd, delegate { StopYellowUICor(); });
    }
}

