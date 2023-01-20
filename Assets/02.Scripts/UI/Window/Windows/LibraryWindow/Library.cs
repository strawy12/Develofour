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
    #region poolParam
    [SerializeField]
    private WindowIcon iconPrefab;
    [SerializeField]
    private Transform poolParent;
    [SerializeField]
    private Transform iconParent;

    private Queue<WindowIcon> poolQueue = new Queue<WindowIcon>();
    private List<WindowIcon> iconList = new List<WindowIcon>();

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

    #region UI
    [SerializeField]
    private Button undoBtn;
    [SerializeField]
    private Button redoBtn;
    #endregion

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
        foreach (WindowIcon icon in iconList)
        {
            Push(icon);
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
        icon.gameObject.SetActive(true);
        return icon;
    }
    #endregion


    private void Awake()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        currentDirectory = file as DirectorySO;
        undoStack = new Stack<DirectorySO>();
        redoStack = new Stack<DirectorySO>();
        
        SetLibrary();
        EventManager.StartListening(ELibraryEvent.OpenFile, OnFileOpen);
    }

    private void SetLibrary()
    {
        windowBar.SetNameText(currentDirectory.windowName);
        CreateChildren();
    }

    private void CreateChildren()
    {
        PushAll();
        foreach (FileSO file in currentDirectory.children)
        {
            WindowIcon icon = Pop();
            icon.SetFileData(file);
        }
    }


    public void UndoSite(object[] emptyParam) => UndoSite();
    public void UndoSite()
    {
        //count가 0이면 알파값 내리는게 맞을듯
        if (undoStack.Count == 0) return;
        DirectorySO data = undoStack.Pop();
        redoStack.Push(currentDirectory);
        ChangeDirectory(data);
    }

    public void RedoSite(object[] emptyParam) => RedoSite();
    public void RedoSite()
    {
        //count가 0이면 알파값 내리는게 맞을듯
        if (redoStack.Count == 0) return;
        DirectorySO data = redoStack.Pop();
        undoStack.Push(currentDirectory);
        ChangeDirectory(data);
    }

    public void ChangeDirectory(DirectorySO SO)
    {
        //현재 디렉토리를 SO디렉토리로 바꾸고 다시 View든 Show든 Create해주면 됨
        //맨 위에있는 사진과 이름 바꿔야함
        //ㅁㅁㅁㅁ 검색 이름 바꾸기
        //주소 바꾸기
    }
    private void OnFileOpen(object[] ps)
    {
        if (ps[0] is DirectorySO)
        {
            currentDirectory = ps[0] as DirectorySO;
            SetLibrary();
        }
    }


}
