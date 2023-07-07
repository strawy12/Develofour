using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public bool IsMonologShow(string type)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            Debug.Log("Json�� ���������ʴ� �ؽ�Ʈ ������ �Դϴ�.");
            return true;

        }
        return data.isShow;
    }

    public void SetMonologShow(string type)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            data = new MonologSaveData();
            data.monologType = type;
            saveData.monologData.Add(data);
        }
        data.isShow = true;
    }

}
