using System;
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
    private LinkedList<Window> windowOrderList = new LinkedList<Window>();

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
        bool isAddUndo = false;
        if (ps.Length > 2)
        {
            isAddUndo = (bool)ps[2];
        }

        if (windowDictionary[EWindowType.Browser].Count == 0)
        {
            // Browser가 존재하지않을 때 하나를 새로 생성시킨다
            // 여기서 생성이 되면 자동으로 Browser.currentBrowser로 지정된다
            CreateWindow(EWindowType.Browser);
        }

        Browser.currentBrowser?.ChangeSite(link, delay, isAddUndo);
    }

    //public void CheckHarmonyWindow(object[] ps)
    //{
    //    if (!windowDictionary.ContainsKey(EWindowType.OutStarDM))
    //    {
    //        Debug.LogError("Browser Type이 Dictionary에 들어가있지않습니다");
    //        return;
    //    }

    //    string name = (string)ps[0];

    //    if (windowDictionary[EWindowType.OutStarDM].Count == 0)
    //    {
    //        // Browser가 존재하지않을 때 하나를 새로 생성시킨다
    //        // 여기서 생성이 되면 자동으로 Browser.currentBrowser로 지정된다
    //        CreateWindow(EWindowType.OutStarDM);
    //    }

    //    Discord.currentDiscord.OpenChattingRoom(name);
    //    WindowOpen(EWindowType.OutStarDM);
    //}

    // TODO : 같은 이름의 윈도우를 실행 시켰을 때 키 값이 겹칠 수 있음. (나중에 구분 할 수 있는 코드 짜야함)
    // 다른 키값 하나가 더 있으야함
    public Window GetWindow(EWindowType windowType, string fileId)
    {
        return windowDictionary[windowType].Find(x => x.File.ID == fileId);
    }

    // 현재 윈도우 딕셔너리의 있는 windowType의 개수를 반환
    public int CurrentWindowCount(EWindowType windowType)
    {
        int num = 0;
        num = windowDictionary[windowType].Count;
        return num;
    }

    public void RemoveWindowDictionary(EWindowType windowType, Window window)
    {
        windowDictionary[windowType].Remove(window);
    }

    // 다른 키 값 하나가 더 있어야 구분 가능
    // 메모장1, 메모장2 구별
    public bool IsExistWindow(EWindowType windowType)
    {
        return windowDictionary.ContainsKey(windowType);
    }

    public Window WindowOpen(EWindowType windowType, FileSO file = null)
    {
        if (file == null)
        {
            file = FileManager.Inst.GetDefaultFile(windowType);
        }

        Window targetWindow = null;
        targetWindow = GetWindow(file.windowType, file.ID);
        bool isNewWindow = false;
        if (targetWindow == null)
        {
            isNewWindow = true;
            PinLockDataSO windowLock = ResourceManager.Inst.GetResource<PinLockDataSO>(file.ID);
            bool isLock = false;

            if (windowLock != null)
            {
                isLock = true;
            }

            Debug.Log("방금 연 윈도우의 핀락은 " + windowLock);

            // lock이 설정 되어있는 fileSO가 이미 락이 풀려있는지 체크
            if (isLock && DataManager.Inst.IsPinLock(file.ID))
            {
                //FileSO lockFileWindowData = FileManager.Inst.GetFile(windowLock.lockWindowType);
                Debug.Log("1234");
                targetWindow = CreateWindow(windowLock.lockWindowType, file); // file.ID(핀락, 미니게임, 파일), windowLock.lockWindowType
            }
            else
            {
                Debug.Log("4321");
                targetWindow = CreateWindow(file.windowType, file);
            }
        }
        targetWindow.WindowOpen(isNewWindow);
        return targetWindow;
    }

    public Window CreateWindow(EWindowType windowType, FileSO file = null)
    {
        if (file == null)
        {
            file = FileManager.Inst.GetDefaultFile(windowType);
        }

        if (windowType == EWindowType.Directory)
        {
            if (windowDictionary[windowType].Count == 0)
            {
                Window fileExplore = GetWindowPrefab(windowType);
                fileExplore.CreatedWindow(file);
                windowDictionary[windowType].Add(fileExplore);
                fileExplore.OnClosed += (s) => windowOrderList.Remove(fileExplore);
            }
            Window directory = windowDictionary[windowType][0];
            EventManager.TriggerEvent(ELibraryEvent.IconClickOpenFile, new object[] { file });
            SetWindowOrder(directory);
            return directory;
        }

        Window window = GetWindowPrefab(windowType);
        window.CreatedWindow(file);
        windowDictionary[windowType].Add(window);
        window.OnClosed += (s) => windowOrderList.Remove(window);
        return window;
    }

    public Window OpenIconProperty(FileSO file)
    {
        FileSO propertyFile = FileManager.Inst.GetDefaultFile(EWindowType.IconProperty);

        Window targetWindow = GetWindow(propertyFile.windowType, file.ID);
        bool isNewWindow = false;
        if (targetWindow == null)
        {
            isNewWindow = true;
            targetWindow = CreateWindow(EWindowType.IconProperty, file);
        }

        targetWindow.WindowOpen(isNewWindow);
        DataManager.Inst.SaveData.isOnceOpenWindowProperty = true;

        return targetWindow;
    }
    public Window GetWindowPrefab(EWindowType windowType)
    {
        Window prefab = windowPrefabList.Find((x) => x.windowType == windowType).windowPrefab;
        return Instantiate(prefab, Define.WindowCanvasTrm);
    }

    private ISelectable selectedObject = null;
    public ISelectable SelectedObject => selectedObject;

    public void SelectObject(ISelectable target)
    {
        if (selectedObject == target) return;

        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = target;

        if (target is Window)
        {
            SetWindowOrder(target as Window);
        }

        selectedObject?.OnSelected?.Invoke();
    }

    public void SetWindowOrder(Window targetWindow)
    {
        if (targetWindow == null) return;
        if (windowOrderList.First?.Value == targetWindow) return;

        if (windowOrderList.Contains(targetWindow))
        {
            windowOrderList.Remove(targetWindow);
        }

        windowOrderList.AddFirst(targetWindow);

        int order = windowOrderList.Count;

        foreach (Window window in windowOrderList)
        {
            window.SortingOrder = order--;
        }
    }

    public bool IsTopWindow(EWindowType type)
    {
        Window window = windowOrderList.First.Value;
        Debug.Log(window.File.windowType);

        return window.File.windowType == type ? true : false;
    }
    public void SelectedObjectNull()
    {
        selectedObject?.OnUnSelected?.Invoke();
        selectedObject = null;
    }
    public bool IsOpenWindowType(EWindowType windowType)
    {
        return windowDictionary.ContainsKey(windowType);
    }
    void LateUpdate()
    {
        if (!DataLoadingScreen.completedDataLoad) return;
        if (InputManager.AnyMouseButtonDown())
        {
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.MouseDown);

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

        if (InputManager.AnyMouseButtonUp())
        {
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.MouseUp);
        }
    }

    public void PopupOpen(FileSO file, string text, Action agreeAction, Action degreeAction)
    {
        FileSO popupFile = FileManager.Inst.GetDefaultFile(EWindowType.Popup);

        Window targetWindow = GetWindow(popupFile.windowType, file.ID);
        bool isNewWindow = false;
        if (targetWindow == null)
        {
            isNewWindow = true;
            targetWindow = CreateWindow(EWindowType.Popup, file);
        }
        PopupWindow popupWindow = targetWindow as PopupWindow;

        popupWindow.Setting(text, agreeAction, degreeAction);

        targetWindow.WindowOpen(isNewWindow);
    }
    
}