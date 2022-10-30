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

    private bool isSelected = false;

    private WindowIconData data;

    private Window targetWindow = null;


    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;

    [SerializeField]
    private TMP_Text iconNameText;

    [SerializeField]
    private RightButtonClick rightButtonMenu;

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

        pointerStayImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);

        OnSelected += () => SelectedIcon(true);
        OnUnSelected += () => SelectedIcon(false);
    }

   
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (isSelected == false)
            {
                WindowManager.Inst.SelectObject(this);
            }
            else // task�ٷ� �̺�Ʈ �����
            {
                if (targetWindow == null)
                {

                }
                else
                {
                    targetWindow.Open();
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            MakingRightClickMenu();
        }
    }


    private void CreateWindow()
    {
        targetWindow.CreateWindow();
        targetWindow.OnClose += (id) => targetWindow = null;
    }

    private void SelectedIcon(bool isSelected)
    {
        this.isSelected = isSelected;
        selectedImage.gameObject.SetActive(isSelected);
    }
    void MakingRightClickMenu()
    {
        Vector3 mousePos
            = Camera.main.ScreenToWorldPoint((new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z)));
        // ���콺 Ŀ�� ���� �����ϴϱ� ��ǥ �޾ƿ�

        rightButtonMenu.CreateMenu(mousePos);
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
