using Coffee.UIEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class DataManager : MonoSingleton<DataManager>
{
    public void AddReturnData(ReturnMonologData data)
    {
        data.SetEndDelayTime();

        saveData.returnMonologData.Add(data);
    }

    public bool IsExistReturnData(ECharacterDataType type)
    {
        return saveData.returnMonologData.Find(x => x.characterType == type) != null;
    }

    public void RemoveReturnData(ReturnMonologData data)
    {
        saveData.returnMonologData.Remove(data);
    }

    public ReturnMonologData GetReturnData(int index)
    {
        return saveData.returnMonologData[index];
    }

    // Character 타입을 받긴 하지만 현재는 조수만 리턴 데이터가 존재하므로 그냥 바로 리턴 박음
    public List<ReturnMonologData> GetReturnDataList(ECharacterDataType type = ECharacterDataType.Assistant)
    {
        return saveData.returnMonologData;
        //saveData.returnMonologData.Where(x =>x.CharacterType== type).ToList();
    }
}
