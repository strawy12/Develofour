using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class CharacterDataResources : ResourcesComponent
{
    public CharacterInfoDataSO GetCharacterByPhoneNumber(string phoneNumber)
    {
        foreach(var data in resourceDictionary.Values)
        {
            CharacterInfoDataSO infoData = data as CharacterInfoDataSO;
            if(infoData.phoneNum == phoneNumber)
            {
                return infoData;
            }
        }

        return null;
    }
    public List<CharacterInfoDataSO> GetCharacterDataList()
    {
        List<CharacterInfoDataSO> list = new List<CharacterInfoDataSO>();
        
        foreach(var data in resourceDictionary.Values)
        {
            list.Add(data as CharacterInfoDataSO);
        }

        return list;
    }

    public override void LoadResources(Action callBack)
    {
        LoadResourceDataAssets<CharacterInfoDataSO>(callBack);
    }
}
