using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class WindowIcon : MonoBehaviour, IDragHandler, IDropHandler, IPointerClickHandler
{
    private WindowIconData data;

    private Window targetWindow = null;
    private bool isSelected = false;

    private RectTransform rectTranstform;
    private Image iconImage;
    private TMP_Text iconNameText;
    private Image selectedImage;

    public void Create(WindowIconData windowIconData)
    {
        data = windowIconData;

        Bind();
        Init();
    }

    private void Bind()
    {
        rectTranstform = GetComponent<RectTransform>();

        iconImage = transform.Find("Icon/IconImage").GetComponent<Image>();
        iconNameText = transform.Find("Icon/IconNameText").GetComponent<TMP_Text>();
        selectedImage = transform.Find("SelectedImage").GetComponent<Image>();
    }

    private void Init()
    {
        iconImage.sprite = data.windowPrefab.WindowData.IconSprite;
        iconNameText.SetText(data.windowPrefab.WindowData.WindowName);
        selectedImage.gameObject.SetActive(false);

        float x = (data.cellPoint.x * Constant.WINDOWICONSIZE.x) + Constant.WINDOWDEFAULTPOS.x;
        float y = (data.cellPoint.y * Constant.WINDOWICONSIZE.y) - Constant.WINDOWDEFAULTPOS.y;
        rectTranstform.localPosition = new Vector3(x, y, rectTranstform.localPosition.z);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SelectedIcon(true);
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isSelected == false)
        {
            SelectedIcon(true);
        }
        else
        {
            if(targetWindow == null)
            {
                CreateWindow();
            }
            else
            {
                targetWindow.Open();
            }

           SelectedIcon(false);
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

}
