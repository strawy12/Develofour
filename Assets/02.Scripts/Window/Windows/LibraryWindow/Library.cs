using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField]
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
        GuideUISystem.EndGuide?.Invoke();
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

    private Queue<WindowIcon> poolQueue = new Queue<WindowIcon>();
    private List<WindowIcon> iconList = new List<WindowIcon>();

    protected override void Init()
    {
        base.Init();
        currentDirectory = file as DirectorySO;


        undoStack = new Stack<DirectorySO>();
        redoStack = new Stack<DirectorySO>();
        FileManager.Inst.ALLFileAddList(currentDirectory);

        fileAddressPanel.Init();
        SetHighlightImage();
        SetLibrary();

        EventManager.StartListening(ETutorialEvent.LibraryRootCheck, CheckTutorialRoot);
        
        EventManager.StartListening(ELibraryEvent.IconClickOpenFile, OnClickIcon);
        EventManager.StartListening(ELibraryEvent.ButtonOpenFile, OnFileOpen);
        EventManager.StartListening(ELibraryEvent.SelectIcon, SelectIcon);
        EventManager.StartListening(ELibraryEvent.SelectNull, SelectNull);
        
        searchInputField.onValueChanged.AddListener(CheckSearchInputTextLength);
        
        searchInputField.onSubmit.AddListener(SearchFunction);
        searchBtn.onClick.AddListener(SearchFunction);
        undoBtn.onClick.AddListener(UndoFile);
        redoBtn.onClick.AddListener(RedoFile);

        EventManager.TriggerEvent(EGuideEventType.ClearGuideType, new object[1] { EGuideTopicName.LibraryOpenGuide });
    }

    private void SearchFunction(string text)
    {
        if (text.Length < 2) return;
        List<FileSO> fileList = FileManager.Inst.SearchFile(text);
        ShowFoundFile(fileList);
    }
        
    private void SearchFunction()
    {
        if (searchInputField.text.Length < 2) return;

        List<FileSO> fileList = FileManager.Inst.SearchFile(searchInputField.text);
        ShowFoundFile(fileList);
    }   

    public void SetLibrary()
    {
        windowBar.SetNameText(currentDirectory.fileName);
        fileAddressPanel.SetButtons(currentDirectory);
        CreateChildren();
        searchInputField.text = "";

        if (GameManager.Inst.GameState == EGameState.Tutorial)
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryRootCheck);
        }
    }

    private void CreateChildren()
    {
        PushAll();
        foreach (FileSO file in currentDirectory.children)
        {
            WindowIcon icon = Pop();
            icon.SetFileData(file);
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
        EventManager.TriggerEvent(ELibraryEvent.ButtonOpenFile, new object[1] { data });
    }

    public void RedoFile(object[] emptyParam) => RedoFile();

    public void RedoFile()
    {
        //count가 0이면 알파값 내리는게 맞을듯
        if (redoStack.Count == 0) return;
        DirectorySO data = redoStack.Pop();
        undoStack.Push(currentDirectory);
        EventManager.TriggerEvent(ELibraryEvent.ButtonOpenFile, new object[1] { data });
    }

    private void OnClickIcon(object[] ps)
    {
        if (redoStack.Count != 0)
        {
            redoStack.Pop();
        }

        undoStack.Push(currentDirectory);
        OnFileOpen(ps);
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

    private void CheckTutorialRoot(object[] ps)
    {
        if (currentDirectory.GetFileLocation() == "User\\BestUSB\\")
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryRequesterInfoStart);
        }
        else if (currentDirectory.GetFileLocation() == "User\\")
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryUSBStart);
        }
        else
        {
            EventManager.TriggerEvent(ETutorialEvent.LibraryUserButtonStart);
        }
    }

    public override void WindowOpen()
    {
        base.WindowOpen();
        //if (GameManager.Inst.GameState == EGameState.Tutorial)
        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignEnd);
    }

    private void OnDisable()
    {
        EventManager.StopListening(ELibraryEvent.IconClickOpenFile, OnClickIcon);
        EventManager.StopListening(ELibraryEvent.ButtonOpenFile, OnFileOpen);
        EventManager.StopListening(ELibraryEvent.SelectIcon, SelectIcon);
        EventManager.StopListening(ELibraryEvent.SelectNull, SelectNull);
        
        EventManager.StopListening(ETutorialEvent.LibraryRootCheck, CheckTutorialRoot);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        EventManager.StopListening(ELibraryEvent.IconClickOpenFile, OnClickIcon);
        EventManager.StopListening(ELibraryEvent.ButtonOpenFile, OnFileOpen);
        EventManager.StopListening(ELibraryEvent.SelectIcon, SelectIcon);
        EventManager.StopListening(ELibraryEvent.SelectNull, SelectNull);

        EventManager.StopListening(ETutorialEvent.LibraryRootCheck, CheckTutorialRoot);
    }
}
