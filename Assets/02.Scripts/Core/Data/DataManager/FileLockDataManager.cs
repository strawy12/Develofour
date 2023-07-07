using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreatePinLockData()
    {
        saveData.PinLockData = new List<PinLockData>();
        List<FileSO> fileList = FileManager.Inst.GetALLFileList();

        for(int i = 0; i < fileList.Count; i++)
        {
            PinLockDataSO lockData = ResourceManager.Inst.GetPinLockData(fileList[i].ID);

            if(lockData == null) 
            {
                continue;
            }
            saveData.PinLockData.Add(new PinLockData() { id = lockData.fileId, isLock = true });
        }

        // 해쉬로 정렬을 해둬서 찾는 속도를 더욱 증가 시킴
        //saveData.FileLockData.OrderBy(x => Animator.StringToHash(x.fileLocation));
    }

    public bool IsPinLock(string id)
    {
        PinLockData data = saveData.PinLockData.Find(x => x.id == id);

        if (data == null)
        {
            return false;
        }

        return data.isLock;
    }

    public void SetPinLock(string id, bool value)
    {
        PinLockData data = saveData.PinLockData.Find(x => x.id == id);

        if (data != null)
        {
            data.isLock = value;
        }
    }
}
