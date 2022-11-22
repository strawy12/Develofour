using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowManager : MonoSingleton<WindowManager>
{
    private Dictionary<EWindowType, List<Window>> windowDictionary = new Dictionary<EWindowType, List<Window>>();
    private List<Window> windowPrefab = new List<Window>();

    private void Start()
    {
        EventManager.StartListening(EBrowserEvent.OnOpenSite, CheckBrowserWindow);
    }

    public void CheckBrowserWindow(object[] ps)
    {
        if(windowDictionary.ContainsKey(EWindowType.Browser))
        {
            if(windowDictionary[EWindowType.Browser].Count > 0)
            {
                Browser.currentBrowser?.ChangeSite((ESiteLink)ps[0], (float)ps[1]);
            }
            else
            {
                Window window = CreateWindow(EWindowType.Browser, 0);
                Browser browser = (Browser)window;
                
                browser.ChangeSite((ESiteLink)ps[0], (float)ps[1]);
            }
        }
    }

    public Window GetWindow(System.Enum windowEnum, int titleId)
    {
        return null;
    }

    public bool IsExistWindow(System.Enum windowEnum)
    {
        return false;
    }

    public Window CreateWindow(EWindowType windowEnum, int titleId) 
    {
        return null;
    }

    public Window GetWindowPrefab(System.Enum windowEnum, int titleId)
    {
        return null;
    }

    private ISelectable selectedObject = null;

    public void SelectObject(ISelectable target)
    {
        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = target;
        selectedObject?.OnSelected?.Invoke();
    }

    public void SelectedObjectNull()
    {
        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = null;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PointerEventData data = new PointerEventData(EventSystem.current);
            data.position = Input.mousePosition;
            List<RaycastResult> hits = new List<RaycastResult>();
            EventSystem.current.RaycastAll(data, hits);

            object[] ps = new object[1] { hits };
            EventManager.TriggerEvent(ECoreEvent.LeftButtonClick, ps);

            if (selectedObject == null) return;

            foreach (RaycastResult hit in hits)
            {
                if (hit.gameObject.GetComponent<ISelectable>() == selectedObject)
                {
                    return;
                }
            }

            SelectedObjectNull();
        }
    }
}