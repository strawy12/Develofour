using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreateMonologData()
    {
        saveData.monologData = new List<MonologSaveData>();

        for (int i = 0; i < ResourceManager.Inst.MonologDataListCount; i++)
        {
            int type = ResourceManager.Inst.MonologTextDataSOList[i].TextDataType;
            saveData.monologData.Add(new MonologSaveData() { monologType = type, isShow = false });
        }

        saveData.monologData.OrderBy(x => x.monologType);
    }

    public bool IsMonologShow(int type)
    {
        Debug.Log("�ڽ��� Ÿ�� : " + type);
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type); ;
        if (data == null)
        {
            Debug.Log("Json�� ���������ʴ� �ؽ�Ʈ ������ �Դϴ�.");
            return true;

        }
        return data.isShow;
    }

    public void SetMonologShow(int type, bool value)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            return;
        }
        data.isShow = value;
    }

}
