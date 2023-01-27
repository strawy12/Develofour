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
        EventManager.TriggerEvent(ELibraryEvent.AddFile);
    }

    private void OnApplicationQuit()
    {
        Debug.LogError("디버깅을 위해 추가한 파일들을 모두 제거합니다");

        foreach(FileSO file in additionFileList)
        {
            DirectorySO dir = file.parent;
            file.parent = null;
            dir.children.Remove(file);
        }
    }
}
