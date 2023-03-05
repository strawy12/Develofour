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

    public List<FileSO> fileList = new List<FileSO>();

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
        string[] locations = location.Split('/');
        DirectorySO currentDir = rootDirectory;

        if (locations[0] == "User")
        {
            Debug.LogError("Location에 User을 포함시키지 마세요");
            return;
        }

        for (int i = 0; i < locations.Length; i++)
        {
            DirectorySO beforeDir = currentDir;
            foreach (FileSO child in currentDir.children)
            {
                if (child.name == locations[i])
                {
                    if((child is DirectorySO) == false)
                    {
                        Debug.LogError($"Location 도중 파일 {child.name}이 Directory가 아닙니다");
                        return;
                    }
                    currentDir = child as DirectorySO;
                    break;
                }
            }

            if(beforeDir == currentDir)
            {
                Debug.LogError($"모든 탐색을 하였으나 Directory가 변경되지않았습니다.");
                return;
            }
        }

        currentDir.children.Add(file);
        file.parent = currentDir;

        additionFileList.Add(file);
        fileList.Add(file);
        EventManager.TriggerEvent(ELibraryEvent.AddFile);
    }

    public void ALLFileAddList(DirectorySO currentDirectory)
    {
        fileList.Clear();
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
    }

    public List<FileSO> SearchFile(string text)
    {
        List<FileSO> searchFileList = new List<FileSO>();
      
        foreach (FileSO file in fileList)
        {
            if (file == null)
            {
                continue;
            }
            if (file.windowName.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                searchFileList.Add(file);
            }
        }

        return searchFileList;
    }

    public List<FileSO> ProfileSearchFile(string text)
    {
        List<FileSO> searchFileList = new List<FileSO>();

        if(text.Length < 2)
        {
            return null;
        }


        foreach (FileSO file in fileList)
        {
            if (file == null)
            {
                continue;
            }

            if (file.windowName.Contains(text, StringComparison.OrdinalIgnoreCase))
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
                    Debug.Log("NotePadAdd");
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
