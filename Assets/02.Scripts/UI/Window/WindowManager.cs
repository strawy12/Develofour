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
        }

        if (windowDictionary[EWindowType.Browser].Count > 0)
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

    public Window GetWindow(EWindowType windowEnum, int titleId)
    {
        return windowDictionary[windowEnum].Find((x) => x.WindowData.windowTitleID == titleId);
    }

    public bool IsExistWindow(EWindowType windowEnum)
    {
        return windowDictionary.ContainsKey(windowEnum);
    }

    public Window CreateWindow(EWindowType windowEnum, int titleId)
    {
        var prefab = from window in windowPrefab
                     where window.WindowData.windowType == windowEnum
                     && window.WindowData.windowTitleID == titleId
                     select window;

        return prefab.FirstOrDefault();
    }

    public Window GetWindowPrefab(EWindowType windowEnum, int titleId)
    {
        return windowPrefab.Find((x) => x.WindowData.windowTitleID == titleId);
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