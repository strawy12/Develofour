using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    private void CreateMonologData()
    {
        saveData.monologData = new List<MonologSaveData>();

        foreach (var element in ResourceManager.Inst.MonologTextDataSOList)
        {
            int type = element.Value.TextDataType;
            saveData.monologData.Add(new MonologSaveData() { monologType = type, isShow = false });
        }

        saveData.monologData.OrderBy(x => x.monologType);
    }

    public bool IsMonologShow(string type)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            Debug.Log("Json에 존재하지않는 텍스트 데이터 입니다.");
            return true;

        }
        return data.isShow;
    }

    public void SetMonologShow(string type, bool value)
    {
        MonologSaveData data = saveData.monologData.Find(x => x.monologType == type);
        if (data == null)
        {
            return;
        }
        data.isShow = value;
    }

}
