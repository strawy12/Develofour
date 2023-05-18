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
                saveData.FileLockData.Add(new FileLockData() { id = file.id, isLock = true });
            }
        }

        // �ؽ��� ������ �صּ� ã�� �ӵ��� ���� ���� ��Ŵ
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
