using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class WindowIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectable
{
    private bool isSelected = false;
    private bool isDragIcon = false;

    private WindowIconData data;

    private Window targetWindow = null;

    private RectTransform rectTranstform;

    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;


    [SerializeField]
    private TMP_Text iconNameText;
    
    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }

    public void Create(WindowIconData windowIconData)
    {
        data = windowIconData;

        Bind();
        Init();
    }

    private void Bind()
    {
        rectTranstform = GetComponent<RectTransform>();
    }

    private void Init()
    {
        iconImage.sprite = data.windowPrefab.WindowData.IconSprite;
        iconNameText.SetText(data.windowPrefab.WindowData.WindowName);

        pointerStayImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);

        float x = (data.cellPoint.x * Constant.WINDOWICONSIZE.x) + Constant.WINDOWDEFAULTPOS.x;
        float y = (data.cellPoint.y * Constant.WINDOWICONSIZE.y) - Constant.WINDOWDEFAULTPOS.y;
        rectTranstform.localPosition = new Vector3(x, y, rectTranstform.localPosition.z);

        OnSelected += () => SelectedIcon(true);
        OnUnSelected += () => SelectedIcon(false);
    }

   
    public void OnPointerClick(PointerEventData eventData)
    {
        if(isSelected == false)
        {
            WindowManager.Inst.SelectObject(this);
        }
        else // task바로 이벤트 쏠거임
        {
            if(targetWindow == null)
            {
                if (isDragIcon)
                {
                    isDragIcon = false;
                    return;
                }

            }
            else
            {
                targetWindow.Open();
            }
        }
    }

    private void CreateWindow()
    {
        targetWindow = UIManager.Inst.CreateWindow(data.windowPrefab.gameObject.name);
        targetWindow.CreateWindow();
        targetWindow.OnClose += (id) => targetWindow = null;
    }

    private void SelectedIcon(bool isSelected)
    {
        this.isSelected = isSelected;
        selectedImage.gameObject.SetActive(isSelected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(false);
    }
}
