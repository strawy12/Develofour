using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 파일탐색기 역할을 하는 window class이다.
/// Library에서 fileSO는 rootDirectory의 fileSO을 가지고 있는다.
/// </summary>
public class Library : Window
{
    [Header("UI")]
    #region poolParam
    [SerializeField]
    private WindowIcon iconPrefab;
    [SerializeField]
    private Transform poolParent;
    [SerializeField]
    private Transform iconParent;

    [Header("Directory")]
    #endregion
    private DirectorySO currentDirectory;

    #region Redo & Undo
    [Header("Redo & Undo")]
    private Stack<DirectorySO> undoStack;

    private Stack<DirectorySO> redoStack;
    #endregion
    [SerializeField]
    private FileAddressPanel fileAddressPanel;

    [Header("SearchUI")]
    [SerializeField]
    private TMP_InputField searchInputField;
    [SerializeField]
    private Button searchBtn;

    [Header("FuctionBar")]
    #region UI
    [SerializeField]
    private Button undoBtn;
    [SerializeField]
    private TextMeshProUGUI undoText;
    [SerializeField]
    private Button redoBtn;
    [SerializeField]
    private TextMeshProUGUI redoText;
    #endregion

    private WindowIcon selectIcon;
    #region pooling
    private void CreatePool()
    {
        for (int i = 0; i < 50; i++)
        {
            WindowIcon icon = Instantiate(iconPrefab, poolParent);

            icon.Bind();
            icon.Init();

            poolQueue.Enqueue(icon);
            icon.gameObject.SetActive(false);
        }
    }
    private void PushAll()
    {
        while (iconList.Count > 0)
        {
            Push(iconList[0]);
        }
    }
    private void Push(WindowIcon icon)
    {
        if (iconList.Contains(icon))
        {
            iconList.Remove(icon);
        }
        if (!poolQueue.Contains(icon))
        {
            poolQueue.Enqueue(icon);
        }
        GuideUISystem.EndGuide?.Invoke(icon.rectTranstform);
        icon.transform.SetParent(poolParent);
        icon.gameObject.SetActive(false);
    }
    private WindowIcon Pop()
    {
        if (poolQueue.Count <= 0)
        {
            CreatePool();
        }
        WindowIcon icon = poolQueue.Dequeue();
        iconList.Add(icon);
        icon.transform.SetParent(iconParent);
        icon.gameObject.SetActive(true);
        return icon;
    }
    #endregion

    private bool isSetLibrary = false;
    private bool isFirstOpen = false;

    private Queue<WindowIcon> poolQueue = new Queue<WindowIcon>();
    private List<WindowIcon> iconList = new List<WindowIcon>();

    public override void WindowOpen()
    {
        base.WindowOpen();
        if (DataManager.Inst.IsProfilerTutorial())
        {
            TutorialEvent();
        }

        if(!DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
        {
            OnSelected -= TutorialLibraryClick;
            OnSelected += TutorialLibraryClick;
        }
    }

    private void TutorialLibraryClick()
    {
        EventManager.TriggerEvent(ETutorialEvent.SelectLibrary, new object[] { this });
    }

    public void TutorialLibraryClickRemoveEvent()
    {
        OnSelected -= TutorialLibraryClick;
    }
    protected override void Init()
    {
        base.Init();
        currentDirectory = file as DirectorySO;

        undoStack = new Stack<DirectorySO>();
        redoStack = new Stack<DirectorySO>();
        FileManager.Inst.GetALLFileList(currentDirectory);

        fileAddressPanel.Init();
        SetHighlightImage();
        SetLibrary();

        
        EventManager.StartListening(ELibraryEvent.IconClickOpenFile, OnClickIcon);
        EventManager.StartListening(ELibraryEvent.ButtonOpenFile, OnFileOpen);
        EventManager.StartListening(ELibraryEvent.SelectIcon, SelectIcon);
        EventManager.StartListening(ELibraryEvent.SelectNull, SelectNull);
        EventManager.StartListening(ELibraryEvent.AddUndoStack, UndoStackPush);
        EventManager.StartListening(ELibraryEvent.ResetRedoStack, RedoStackReset);
        EventManager.StartListening(ETutorialEvent.LibraryEventTrigger, SetLibraryEvent);
        
        searchInputField.onValueChanged.AddListener(CheckSearchInputTextLength);
        
        searchInputField.onSubmit.AddListener(SearchFunction);
        searchBtn.onClick.AddListener(SearchFunction);
        undoBtn.onClick.AddListener(UndoFile);
        redoBtn.onClick.AddListener(RedoFile);

        EventManager.TriggerEvent(EGuideEventType.ClearGuideType, new object[1] { EGuideTopicName.LibraryOpenGuide });
    }

    private void SetLibraryEvent(object[] obj)
    {
        if (DataManager.Inst.IsProfilerTutorial())
        {
            Debug.Log("Asdf");
            TutorialEvent();
        }
    }

    private void SearchFunction(string text)
    {
        if (text.Length < 2) return;
        List<FileSO> fileList = FileManager.Inst.SearchFile(text, currentDirectory);
        ShowFoundFile(fileList);
    }
        
    private void SearchFunction()
    {
        if (searchInputField.text.Length < 2) return;

        List<FileSO> fileList = FileManager.Inst.SearchFile(searchInputField.text, currentDirectory);
        ShowFoundFile(fileList);
    }

    public void SetLibrary()
    {
        WindowManager.Inst.SetWindowOrder(this);
        windowBar.SetNameText(currentDirectory.fileName);
        fileAddressPanel.SetButtons(currentDirectory);
        CreateChildren();
        EventManager.TriggerEvent(EMonologEvent.MonologException, new object[1] { currentDirectory });
        searchInputField.text = "";

        if (DataManager.Inst.IsProfilerTutorial())
        {
            TutorialEvent();
        }
    }

    public void TutorialEvent()
    {
        Debug.Log(currentDirectory.id);
        if (currentDirectory.id == 7 && DataManager.Inst.IsProfilerTutorial())
        {
            EventManager.TriggerEvent(ETutorialEvent.USBTutorial);
        }
        else if (currentDirectory.id == 6 && DataManager.Inst.IsProfilerTutorial())
        {
            EventManager.TriggerEvent(ETutorialEvent.ReportTutorial);
        }
        else if (DataManager.Inst.IsProfilerTutorial() )
        {
            TopFileButton button = fileAddressPanel.TopFileButtons.Find((x) => x.CurrentDirectory.id == 7);

            GuideUISystem.EndAllGuide?.Invoke();
            GuideUISystem.OnGuide(button.tutorialSelectImage.transform as RectTransform);
            GuideUISystem.FullSizeGuide?.Invoke(button.tutorialSelectImage.transform as RectTransform);
        }

    }

    private void CreateChildren()
    {
        PushAll();
        foreach (FileSO file in currentDirectory.children)
        {
            if (file.isHide) { continue; }
            WindowIcon icon = Pop();
            icon.PointerStayImage.gameObject.SetActive(false);
            icon.SetFileData(file); // icon마다의 startTrigger를 이 함수에 넣어야함
        }
        isSetLibrary = false;
    }
    
    private void CheckSearchInputTextLength(string text)
    {
        if (isSetLibrary) return;

        if (text.Length == 0)
        {
            SetLibrary();
        }
    }

    private void ShowFoundFile(List<FileSO> fileList)
    {
        PushAll();
        foreach (FileSO file in fileList)
        {
            WindowIcon icon = Pop();
            icon.SetFileData(file);
        }
        fileAddressPanel.SetEmptyBtn();
    }


    public void UndoFile(object[] emptyParam) => UndoFile();
    public void UndoFile()
    {
        //count가 0이면 알파값 내리는게 맞을듯
        if (undoStack.Count == 0) return;
        DirectorySO data = undoStack.Pop();
        redoStack.Push(currentDirectory);
        OnFileOpen(new object[1] { data });
    }

    public void RedoFile(object[] emptyParam) => RedoFile();
    public void RedoFile()
    {
        //count가 0이면 알파값 내리는게 맞을듯
        if (redoStack.Count == 0) return;
        DirectorySO data = redoStack.Pop();
        undoStack.Push(currentDirectory);
        OnFileOpen(new object[1] { data });
    }
    private void OnClickIcon(object[] ps)
    {

        if (redoStack.Count != 0)
        {
            redoStack.Pop();
        }

        if(!isFirstOpen)
        {
            isFirstOpen = true;
        }
        else
        {
            undoStack.Push(currentDirectory);
        }

        RedoStackReset(ps);
        OnFileOpen(ps);
    }

    public void UndoStackPush(object[] ps)
    {
        undoStack.Push(currentDirectory);
    }

    public void RedoStackReset(object[] ps)
    {
        redoStack.Clear();
    }

    private void OnFileOpen(object[] ps)
    {
        if (ps[0] is DirectorySO)
        {
            SetHighlightImage();
            currentDirectory = ps[0] as DirectorySO;
            SetLibrary();
        }
    }

    private void SetHighlightImage()
    {
        if (undoStack.Count == 0)
            undoText.color = new Color32(50, 50, 50, 120);
        else
            undoText.color = new Color32(50, 50, 50, 255);

        if (redoStack.Count == 0)
            redoText.color = new Color32(50, 50, 50, 120);
        else
            redoText.color = new Color32(50, 50, 50, 255);
    }

    public void SelectIcon(object[] ps)
    {
        if (ps[0] == null || !(ps[0] is WindowIcon)) return;

        WindowIcon icon = ps[0] as WindowIcon;

        if (selectIcon == icon) return;

        selectIcon?.SelectedIcon(false);
        selectIcon = icon;

        selectIcon?.SelectedIcon(true);
    }

    public void SelectNull(object[] dummy)
    {
        selectIcon?.SelectedIcon(false);
        selectIcon = null;
    }

    //여기에 라이브러리 위치를 확인해서 튜토리얼에서 어떤 이벤트가 실행될지 관리해주는 함수가 있음   

    protected override void OnDestroyWindow()
    {

        base.OnDestroyWindow();
        isFirstOpen = false;
        GuideUISystem.EndAllGuide?.Invoke();
        OnSelected -= TutorialLibraryClick;
        EventManager.StopListening(ELibraryEvent.IconClickOpenFile, OnClickIcon);
        EventManager.StopListening(ELibraryEvent.ButtonOpenFile, OnFileOpen);
        EventManager.StopListening(ELibraryEvent.SelectIcon, SelectIcon);
        EventManager.StopListening(ELibraryEvent.SelectNull, SelectNull);
        EventManager.StopListening(ELibraryEvent.AddUndoStack, UndoStackPush);
        EventManager.StopListening(ELibraryEvent.ResetRedoStack, RedoStackReset);
    }
}
