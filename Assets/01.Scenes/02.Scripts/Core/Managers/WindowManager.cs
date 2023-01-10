using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowManager : MonoSingleton<WindowManager>
{
    private Dictionary<EWindowType, List<Window>> windowDictionary = new Dictionary<EWindowType, List<Window>>();
    [SerializeField]
    private List<Window> windowPrefab = new List<Window>();

    private void Awake()
    {
        InitDictionary();
    }
    private void Start()
    {
        EventManager.StartListening(EBrowserEvent.OnOpenSite, CheckBrowserWindow);
    }

    private void InitDictionary()
    {
        for (int i = 0; i < (int)EWindowType.End; ++i)
        {
            windowDictionary.Add((EWindowType)i, new List<Window>());
        }
    }

    public void CheckBrowserWindow(object[] ps)
    {
        if (!windowDictionary.ContainsKey(EWindowType.Browser))
        {
            Debug.LogError("Browser Type이 Dictionary에 들어가있지않습니다");
            return;
        }

        if (windowDictionary[EWindowType.Browser].Count > 0)
        {
            ESiteLink link = (ESiteLink)ps[0];
            float delay = (ps[1] is int) ? (int)ps[1] : (float)ps[1];
            if(ps.Length >= 3)
            {
                if (ps[2] is bool) {
                    bool isAddUndo = (bool)ps[2];
                    Browser.currentBrowser?.ChangeSite(link, delay, isAddUndo);
                    return;
                }
            }
            Browser.currentBrowser?.ChangeSite(link, delay);
        }
        else
        {
            Browser browser = CreateWindow(EWindowType.Browser, 0) as Browser; 
            
            ESiteLink link = (ESiteLink)ps[0];
            float delay = (ps[1] is int) ? (int)ps[1] : (float)ps[1];
            browser.ChangeSite(link, delay);

        }
    }
    public Window GetWindow(EWindowType windowType, int titleId)
    {
        return windowDictionary[windowType].Find((x) => x.WindowData.windowTitleID == titleId);
    }

    public bool IsExistWindow(EWindowType windowType)
    {
        return windowDictionary.ContainsKey(windowType);
    }

    public Window CreateWindow(EWindowType windowType, int titleId)
    {
        Window window = GetWindowPrefab(windowType, titleId);
        window.CreatedWindow();
        windowDictionary[windowType].Add(window);
        return window;
    }

    public Window GetWindowPrefab(EWindowType windowType, int titleId)
    {
        Window prefab = windowPrefab.Find((x) => x.WindowData.windowType == windowType 
                                              && x.WindowData.windowTitleID == titleId);

        return Instantiate(prefab, Define.WindowCanvasTrm);
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