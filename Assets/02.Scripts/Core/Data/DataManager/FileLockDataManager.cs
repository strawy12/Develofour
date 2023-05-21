using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreateFileLockData()
    {
        saveData.FileLockData = new List<FileLockData>();
        List<FileSO> fileList = FileManager.Inst.GetALLFileList();

        foreach (FileSO file in fileList)
        {
            if (file.isFileLock == true)
            {
                saveData.FileLockData.Add(new FileLockData() { id = file.id, isLock = true });
            }
        }

        // 해쉬로 정렬을 해둬서 찾는 속도를 더욱 증가 시킴
        //saveData.FileLockData.OrderBy(x => Animator.StringToHash(x.fileLocation));
    }

    public bool IsFileLock(int id)
    {
        FileLockData data = saveData.FileLockData.Find(x => x.id == id);

        if (data == null)
        {
            return true;
        }

        return data.isLock;
    }

    public void SetFileLock(int id, bool value)
    {
        FileLockData data = saveData.FileLockData.Find(x => x.id == id);

        if (data != null)
        {
            data.isLock = value;
        }
    }
}
