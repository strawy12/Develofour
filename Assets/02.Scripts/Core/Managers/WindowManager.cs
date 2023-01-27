﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public class WindowPrefabElement
{
    public EWindowType windowType;
    public Window windowPrefab;
}

public class WindowManager : MonoSingleton<WindowManager>
{
    // 이거는 동적으로 생성된 Window 모음
    private Dictionary<EWindowType, List<Window>> windowDictionary = new Dictionary<EWindowType, List<Window>>();

    // 프리팹
    [SerializeField]
    private List<WindowPrefabElement> windowPrefabList = new List<WindowPrefabElement>();

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
        for (EWindowType type = EWindowType.None + 1; type < EWindowType.End; type++)
        {
            windowDictionary.Add(type, new List<Window>());
        }
    }

    // ps[0] = SiteLink
    // ps[1] = Site Open Or Change Delay
    // ps[2] = Undo Flag
    public void CheckBrowserWindow(object[] ps)
    {
        if (!windowDictionary.ContainsKey(EWindowType.Browser))
        {
            Debug.LogError("Browser Type이 Dictionary에 들어가있지않습니다");
            return;
        }

        ESiteLink link = (ESiteLink)ps[0];
        float delay = (ps[1] is int) ? (int)ps[1] : (float)ps[1];

        if (windowDictionary.ContainsKey(EWindowType.Browser))
        {
            if (ps.Length >= 3)
            {
                if (ps[2] is bool)
                {
                    bool isAddUndo = (bool)ps[2];
                    Browser.currentBrowser?.ChangeSite(link, delay, isAddUndo);
                    return;
                }
            }
        }
        else
        {
            // Browser가 존재하지않을 때 하나를 새로 생성시킨다
            // 여기서 생성이 되면 자동으로 Browser.currentBrowser로 지정된다
            CreateWindow(EWindowType.Browser);
        }


        Browser.currentBrowser?.ChangeSite(link, delay);
    }

    // TODO : 같은 이름의 윈도우를 실행 시켰을 때 키 값이 겹칠 수 있음. (나중에 구분 할 수 있는 코드 짜야함)
    // 다른 키값 하나가 더 있으야함
    public Window GetWindow(EWindowType windowType, string windowName)
    {
        return windowDictionary[windowType].Find(x => x.File.name == windowName);
    }

    // 다른 키 값 하나가 더 있어야 구분 가능
    // 메모장1, 메모장2 구별
    public bool IsExistWindow(EWindowType windowType)
    {
        return windowDictionary.ContainsKey(windowType);
    }

    public Window WindowOpen(EWindowType windowType, FileSO file = null)
    {
        Window targetWindow = GetWindow(file.windowType, file.name);

        if (targetWindow == null)
        {
            if(!file.isWindowLockClear)
            {
                EventManager.TriggerEvent(EWindowEvent.OpenWindowPin, new object[1] { file });
                return null;
            }

            targetWindow = CreateWindow(file.windowType, file);
        }

        targetWindow.WindowOpen();
        return targetWindow;
    }

    // 
    public Window CreateWindow(EWindowType windowType, FileSO file = null)
    {
        if(file == null)
        {
            // null이면 기본 FileSO를 리소스로드로 찾아서 넣을거임
        }

        Window window = GetWindowPrefab(windowType);
        window.CreatedWindow(file);
        windowDictionary[windowType].Add(window);
        return window;
    }

    public Window GetWindowPrefab(EWindowType windowType)
    {
        Window prefab = windowPrefabList.Find((x) => x.windowType == windowType).windowPrefab;

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

    void LateUpdate()
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
                if (selectedObject.IsSelected(hit.gameObject))
                {
                    return;
                }
            }

            SelectedObjectNull();
        }
    }
}