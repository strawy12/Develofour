using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TaskIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected string windowTitle;
    public string Title => windowTitle;

    protected List<Window> targetWindowList;
    protected Image iconImage;
    protected Image activeImage;
    protected Image highlightedImage;

    protected bool isFixed = false;

    public void Init()
    {
        targetWindowList = new List<Window>();
        Bind();
    }

    protected void Bind()
    {
        iconImage = transform.Find("IconImage").GetComponent<Image>();
        activeImage = transform.Find("ActiveImage").GetComponent<Image>();
        highlightedImage = transform.Find("HighlightedImage").GetComponent<Image>();
    }

    public void AddTargetWindow(Window target)
    {
        if (iconImage.sprite != target.WindowData.IconSprite)
        {
            iconImage.sprite = target.WindowData.IconSprite;
        }
        if(string.IsNullOrEmpty(windowTitle))
        {
            windowTitle = target.WindowData.Title;
        }

        target.OnClose += RemoveTargetWindow;
        targetWindowList.Add(target);
        activeImage.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                OpenTargetWindow();
                break;

            //case PointerEventData.InputButton.Middle:

            case PointerEventData.InputButton.Right:
                break;
        }
    }

    protected virtual void OpenTargetWindow()
    {
        if (targetWindowList.Count == 1)
        {
            targetWindowList[0].Open();
        }
    }

    protected void RemoveTargetWindow(string windowID)
    {
        int idx = targetWindowList.FindIndex(x => x.ID.Equals(windowID));
        targetWindowList.RemoveAt(idx);
        activeImage.gameObject.SetActive(false);

        if (!isFixed && targetWindowList.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(false);
    }

}
