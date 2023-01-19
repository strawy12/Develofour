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

    private Queue<WindowIcon> poolQueue;
    private List<WindowIcon> iconList = new List<WindowIcon>();

    #endregion
    [SerializeField]
    private DirectorySO currentDirectory;

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

    private void OnFileOpen(object[] ps)
    {
        if (ps[0] is DirectorySO)
        {
            currentDirectory = ps[0] as DirectorySO;
            SetLibrary();
        }
    }


}
