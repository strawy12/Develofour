using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LibraryLeftPanel : MonoBehaviour
{
    [SerializeField]
    private WindowIcon iconPrefab;

    [SerializeField]
    private Transform leftIconParent;

    [SerializeField]
    private int iconMaxNum = 20;

    private List<WindowIcon> windowIconList;


    public void Init()
    {
        EventManager.StartListening(ELibraryEvent.CreateLeftPanel, AddIcon);
        windowIconList = new List<WindowIcon>();
        List<string> fileIDList = DataManager.Inst.SaveData.libraryData;
        List<FileSO> fileList = FileManager.Inst.GetFileIDList(fileIDList);
        foreach (var file in fileList)
        {
            AddIcon(file);
        }
    }

    public void AddIcon(object[] ps)
    {
        if (ps[0] == null)
        {
            return;
        }
        FileSO file = ps[0] as FileSO;

        if (ps[0] is string)
        {
            string fileId = ps[0] as string;
            file = FileManager.Inst.GetFile(fileId);
            DataManager.Inst.AddLibraryDataNewFile(fileId);
        }
        else if (ps[0] is FileSO)
        {
           DataManager.Inst.AddLibraryDataNewFile(file.ID);
        }
        else
        {
            return;
        }

        var removeIcon = windowIconList.Find(x => x.File.ID == file.ID);
        if(removeIcon != null)
        {
            windowIconList.Remove(removeIcon);
            Debug.Log($"{file.fileName}아이콘 제거");
            Destroy(removeIcon.gameObject);
        }

        AddIcon(file);
    }

    private void AddIcon(FileSO file)
    {
        if (windowIconList.Count >= iconMaxNum)
        {
            DataManager.Inst.SaveData.libraryData.RemoveAt(0);
            var removeIcon = windowIconList[0];
            Destroy(removeIcon.gameObject);
            windowIconList.RemoveAt(0);
        }

        var windowIcon = Instantiate(iconPrefab, leftIconParent);
        windowIcon.Init();
        windowIcon.SetFileData(file, 30f);
         
        windowIcon.gameObject.SetActive(true);
        windowIconList.Add(windowIcon);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(ELibraryEvent.CreateLeftPanel, AddIcon);

    }
}
