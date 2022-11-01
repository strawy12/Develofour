using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class WindowIconAttrributeUI : MonoUI
{
    [SerializeField]
    private PropertyUI propertyUI;

    public RectTransform rectTransform;

    private WindowIconDataSO windowPropertyData;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void CreateMenu(Vector3 iconPos, WindowIconDataSO windowIconData)
    {
        float pivotX = 0f;
        float pivotY = 1f;
        
        rectTransform.localPosition = iconPos;

        if (rectTransform.localPosition.x >= 690)
            pivotX = 1f;

        if (rectTransform.localPosition.y <= -240)
            pivotY = 0f;

        
        Vector2 normalPivot = new Vector2(pivotX, pivotY);
        rectTransform.pivot = normalPivot;

        gameObject.SetActive(true); 
        windowPropertyData = windowIconData;
    }   

    public void CreateProperty()
    {
       propertyUI.CreatePropertyUI(windowPropertyData);
    }
}
