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



    public void AddFile(FileSO file, string location)
    {
        string[] locations = location.Split('/');
        DirectorySO currentDir = rootDirectory;

        if (locations[0] == "User")
        {
            Debug.LogError("Location�� User�� ���Խ�Ű�� ������");
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
                        Debug.LogError($"Location ���� ���� {child.name}�� Directory�� �ƴմϴ�");
                        return;
                    }
                    currentDir = child as DirectorySO;
                    break;
                }
            }

            if(beforeDir == currentDir)
            {
                Debug.LogError($"��� Ž���� �Ͽ����� Directory�� ��������ʾҽ��ϴ�.");
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
                Debug.LogWarning("while���� ����ؼ� �������Դϴ�.");
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

    private void OnApplicationQuit()
    {
        Debug.LogError("������� ���� �߰��� ���ϵ��� ��� �����մϴ�");

        foreach (FileSO file in additionFileList)
        {
            DirectorySO dir = file.parent;
            file.parent = null;
            dir.children.Remove(file);
        }
    }

}
