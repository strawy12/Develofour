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
        iconNameText.text = fileData.windowName;
        iconImage.sprite = newFileData.iconSprite;

        if (isRegisterEvent == false && fileData.windowName == "�Ƿ��� ����")
        {
            isRegisterEvent = true;
            EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoStart, delegate
            {
                if(gameObject != null)
                {
                    if (gameObject.activeSelf) StartCoroutine(YellowSignCor());
                }

                EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoSelect, delegate
                {
                    EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoEnd, new object[0]);

                    EventManager.TriggerEvent(ETutorialEvent.EndTutorial, new object[0]);
                    //EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]); ������ �ؾ��ϴ°�
                });
            });
            
            EventManager.StartListening(ETutorialEvent.LibraryRequesterInfoEnd, delegate { StopCor(); RequesterInfoEventStop(); });
            Debug.Log("�̺�Ʈ ���");
        }

        if (isUSBEvent == false && fileData.windowName == "BestUSB")
        {
            isUSBEvent = true;
            EventManager.StartListening(ETutorialEvent.LibraryUSBStart, delegate
            {
                if (gameObject != null)
                {
                    if (gameObject.activeSelf) StartCoroutine(YellowSignCor());
                }
                EventManager.StartListening(ETutorialEvent.LibraryUSBSelect, delegate
                {
                    EventManager.TriggerEvent(ETutorialEvent.LibraryUSBEnd, new object[0]);

                });
            });
            EventManager.StartListening(ETutorialEvent.LibraryUSBEnd, delegate { StopCor(); USBEventStop(); });
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (clickCount != 0)
            {

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
                if (gameObject.name == "ExplorerIcon")
                {
                    EventManager.TriggerEvent(ETutorialEvent.BackgroundSelect, new object[0]);
                }

                if (fileData.windowName == "�Ƿ��� ����")
                {
                    EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoSelect, new object[0]);
                }
                if (fileData.windowName == "BestUSB")
                {
                    Debug.Log("BestUSB");
                    EventManager.TriggerEvent(ETutorialEvent.LibraryUSBSelect, new object[0]);
                }
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

    void Start()
    {
        if (gameObject.name == "ExplorerIcon")
        {
            EventManager.StartListening(ETutorialEvent.BackgroundSignStart, delegate
            {
                if (gameObject.activeSelf) StartCoroutine(YellowSignCor());
                EventManager.StartListening(ETutorialEvent.BackgroundSelect, delegate
                {
                    EventManager.TriggerEvent(ETutorialEvent.BackgroundSignEnd, new object[0]);
                });
            });

            EventManager.StartListening(ETutorialEvent.BackgroundSignEnd, delegate { StopCor(); BackgroundEventStop(); });
        }
    }

    private bool isSign;
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

    public void StopCor()
    {
        isSign = false;
        yellowUI.gameObject.SetActive(false);
        StopCoroutine(YellowSignCor());
        yellowUI.DOKill();
    }

    private void BackgroundEventStop()
    {
        EventManager.StopListening(ETutorialEvent.BackgroundSignStart, delegate { if (gameObject.activeSelf) StartCoroutine(YellowSignCor()); });
        EventManager.StopListening(ETutorialEvent.BackgroundSignEnd, delegate { StopCor(); });

        EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]);
    }

    private void RequesterInfoEventStop()
    {
        EventManager.StopListening(ETutorialEvent.LibraryRequesterInfoStart, delegate { if (gameObject.activeSelf) StartCoroutine(YellowSignCor()); });
        EventManager.StopListening(ETutorialEvent.LibraryRequesterInfoEnd, delegate { StopCor(); });
    }

    private void USBEventStop()
    {
        EventManager.StopListening(ETutorialEvent.LibraryUSBStart, delegate { if (gameObject.activeSelf) StartCoroutine(YellowSignCor()); });
        EventManager.StopListening(ETutorialEvent.LibraryUSBEnd, delegate { StopCor(); });
        EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck, new object[0]);
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
