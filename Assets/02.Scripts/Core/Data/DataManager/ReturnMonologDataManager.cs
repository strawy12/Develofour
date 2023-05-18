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

    public ReturnMonologData GetReturnData(int index)
    {
        return saveData.returnMonologData[index];
    }

    // Character Ÿ���� �ޱ� ������ ����� ������ ���� �����Ͱ� �����ϹǷ� �׳� �ٷ� ���� ����
    public List<ReturnMonologData> GetReturnDataList(ECharacterDataType type = ECharacterDataType.Assistant)
    {
        return saveData.returnMonologData;
        //saveData.returnMonologData.Where(x =>x.CharacterType== type).ToList();
    }
}
