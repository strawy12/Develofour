using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
public class FileManager : MonoSingleton<FileManager>
{
    [SerializeField]
    public DirectorySO rootDirectory;

    public List<FileSO> additionFileList = new List<FileSO>();
    //새롭게 추가된 파일은 fileList에 등록된다.

    public List<FileSO> defaultFileList = new List<FileSO>();

    private Dictionary<EWindowType, FileSO> defaultFileDictionary = new Dictionary<EWindowType, FileSO>();

    private void Awake()
    {
        foreach(FileSO file in defaultFileList)
        {
            defaultFileDictionary.Add(file.windowType, file);
        }
    }

    public FileSO GetDefaultFile(EWindowType windowType)
    {
        if(!defaultFileDictionary.ContainsKey(windowType))
        {
            Debug.Log("존재하지 않는 defaultfile 윈도우 입니다.");
            return null;
        }
        FileSO file = defaultFileDictionary[windowType];
        return file;
    }

    public void AddFile(FileSO file, string location)
    {

        location += "\\";
        List<FileSO> fileList = ALLFileAddList();

        DirectorySO currentDir = rootDirectory;

        currentDir = fileList.Find((x) => x.GetFileLocation() == location) as DirectorySO; 

        if(!currentDir.children.Contains(file))
        {
            currentDir.children.Add(file);
            file.parent = currentDir;
            additionFileList.Add(file);
        }

        if (!DataManager.Inst.AdditionalFileContain(location + "\\" + file.fileName))
        {
            DataManager.Inst.AddNewFileData(location +"\\" +file.fileName);
        }
        EventManager.TriggerEvent(ELibraryEvent.AddFile);
    }

    public List<FileSO> ALLFileAddList(DirectorySO currentDirectory = null, bool isAdditional = false)
    {
        //fileList.Clear();
        if(currentDirectory == null)
        {
            currentDirectory = rootDirectory;
        }

        List<FileSO> fileList = new List<FileSO>();

        Queue<DirectorySO> directories = new Queue<DirectorySO>();
        directories.Enqueue(currentDirectory);
        int i = 0;
        while (directories.Count != 0)
        {
            DirectorySO directory = directories.Dequeue();
            i++;
            if (i > 10000)
            {
                Debug.LogWarning("while문이 계속해서 실행중입니다.");
                break;
            }
            foreach (FileSO file in directory.children)
            {
                fileList.Add(file);
                if (file is DirectorySO)
                {
                    directories.Enqueue(file as DirectorySO);
                }
            }
        }

        if(isAdditional)
        {
            fileList.AddRange(additionFileList);
        }

        return fileList;
    }

    public List<FileSO> SearchFile(string text,DirectorySO currentDirectory = null)
    {
        List<FileSO> allFileList = ALLFileAddList(currentDirectory);

        List<FileSO> searchFileList = new List<FileSO>();
      
        foreach (FileSO file in allFileList)
        {
            if (file == null)
            {
                continue;
            }
            if (file.fileName.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                searchFileList.Add(file);
            }
        }

        return searchFileList;
    }

    public List<FileSO> ProfileSearchFile(string text, DirectorySO currentDirectory = null)
    {
        List<FileSO> allFileList = ALLFileAddList(currentDirectory);
        List<FileSO> searchFileList = new List<FileSO>();

        if(text.Length < 2)
        {
            return null;
        }


        foreach (FileSO file in allFileList)
        {
            if (file == null)
            {
                continue;
            }

            if (file.fileName.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                searchFileList.Add(file);
            }
            else if (file.SearchTag(text))
            {
                searchFileList.Add(file);
            }
            else if(file.windowType == EWindowType.Notepad)
            {
                if(NotePadFileLoad(text, file))
                {
                    searchFileList.Add(file);
                }
            }
        }

        return searchFileList;
    }

    private bool NotePadFileLoad(string text, FileSO file)
    {
        try
        {
            NotepadDataSO notePadData = ResourceManager.Inst.GetNotepadData(file.name);

            if (notePadData == null)
            {
                return false;
            }

            if (notePadData.scripts.Contains(text))
            {
                return true;
            }
        }
        catch
        {
            Debug.Log($"Don't have this key {text} in NotePad");
        }
        return false;
    }


    private FileSO GetFile(string location)
    {
        FileSO resultFile = null;
        resultFile = ALLFileAddList().Find((x) => x.GetFileLocation() == location);
        return resultFile;
    }
    private void OnApplicationQuit()
    {
        Debug.LogError("디버깅을 위해 추가한 파일들을 모두 제거합니다");

        foreach (FileSO file in additionFileList)
        {
            DirectorySO dir = file.parent;
            file.parent = null;
            dir.children.Remove(file);
        }
    }

}
