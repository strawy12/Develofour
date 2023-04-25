using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreateFileLockData()
    {
        saveData.FileLockData = new List<FileLockData>();
        List<FileSO> fileList = FileManager.Inst.ALLFileAddList();

        foreach (FileSO file in fileList)
        {
            if (file.isFileLock == true)
            {
                saveData.FileLockData.Add(new FileLockData() { fileLocation = file.GetFileLocation(), isLock = true });
            }
        }

        // �ؽ��� ������ �صּ� ã�� �ӵ��� ���� ���� ��Ŵ
        //saveData.FileLockData.OrderBy(x => Animator.StringToHash(x.fileLocation));
    }

    public bool IsFileLock(string fileLocation)
    {
        FileLockData data = saveData.FileLockData.Find(x => x.fileLocation == fileLocation);

        if (data == null)
        {
            return true;
        }

        return data.isLock;
    }

    public void SetFileLock(string fileLocation, bool value)
    {
        FileLockData data = saveData.FileLockData.Find(x => x.fileLocation == fileLocation);

        if (data != null)
        {
            data.isLock = value;
        }
    }
}
