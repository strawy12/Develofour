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

    private RectTransform rectTransform;
    private WindowIconDataSO windowPropertyData;
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void CreateMenu(Vector3 menuPos, WindowIconDataSO windowIconData)
    {
        Vector3 defaultPos = new Vector3(menuPos.x + 1.35f, menuPos.y - 1.35f, menuPos.z);

        if (defaultPos.x >= 7.5f)
            defaultPos.x -= 2.4f;

        if (defaultPos.y <= -3f)
            defaultPos.y += 2.8f;

        gameObject.SetActive(true); 
        transform.position = defaultPos;
        windowPropertyData = windowIconData;
    }   

    public void CreateProperty()
    {
       propertyUI.CreatePropertyUI(windowPropertyData);
    }
}
