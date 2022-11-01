using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class WindowIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectable
{
    public RectTransform rectTranstform;

    private int cilckCount = 0;

    //private WindowIconData data;

    //private Window targetWindow = null;

    [SerializeField]
    private GameObject windowPanel;

    private Sprite sprite;

    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;

    [SerializeField]
    private TMP_Text iconNameText;

    [SerializeField]
    private WindowIconAttrributeUI rightButtonMenu;
    [SerializeField]
    private WindowIconDataSO windowIconData;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }

    public void Awake()
    {
        Bind();
        Init();
    }

    private void Bind()
    {
        rectTranstform = GetComponent<RectTransform>();
    }

    private void Init()
    {
        pointerStayImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);

        OnSelected += () => SelectedIcon(true);
        OnUnSelected += () => SelectedIcon(false);
    }
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (cilckCount != 0)
            {
                // 여기에서 이벤트 쏨
                cilckCount = 0;
                Instantiate(windowPanel, windowPanel.transform.position, windowPanel.transform.rotation, gameObject.transform);
                WindowManager.Inst.SelectedObjectNull();
            }

            else 
            {
                WindowManager.Inst.SelectObject(this);
                cilckCount++;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            WindowManager.Inst.SelectObject(this);
            MakingRightClickMenu(eventData);
        }
    }

    private void CreateWindow()
    {
        //targetWindow.CreateWindow();
        //targetWindow.OnClose += (id) => targetWindow = null;
    }

    private void SelectedIcon(bool isSelected)
    {
        selectedImage.gameObject.SetActive(isSelected);
    }

    void MakingRightClickMenu(PointerEventData eventData)
    {
        //Vector3 inputPos = Input.mousePosition;
        //Vector3 wolrdPos = Camera.main.ScreenToWorldPoint(inputPos);

        //Vector3 defaultPos = new Vector3(wolrdPos.x, wolrdPos.y, 0);
        //Debug.Log(defaultPos);

        rightButtonMenu.CreateMenu(rectTranstform.localPosition, windowIconData);
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
