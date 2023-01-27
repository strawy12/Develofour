using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FileManager : MonoSingleton<FileManager>
{
    [SerializeField]
    private DirectorySO rootDirectory;

    private List<FileSO> additionFileList = new List<FileSO>();

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
        EventManager.TriggerEvent(ELibraryEvent.AddFile);
    }

    private void OnApplicationQuit()
    {
        Debug.LogError("������� ���� �߰��� ���ϵ��� ��� �����մϴ�");

        foreach(FileSO file in additionFileList)
        {
            DirectorySO dir = file.parent;
            file.parent = null;
            dir.children.Remove(file);
        }
    }
}
