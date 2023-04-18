using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<ECharacterDataType, CharacterInfoDataSO> characterDataSOList;

    public CharacterInfoDataSO GetCharacterDataSO(ECharacterDataType textType)
    {
        return characterDataSOList[textType];
    }
    public CharacterInfoDataSO GetCharacterDataSO(string phoneNumber)
    {
        return characterDataSOList.Values.FirstOrDefault((x) => x.phoneNum == phoneNumber);
    }
    public List< CharacterInfoDataSO> GetCharacterDataSOList()
    {
        List<CharacterInfoDataSO> results = new List<CharacterInfoDataSO>();
        foreach(var temp in characterDataSOList.Values)
        {
            results.Add(temp);
        }
        return results;
    }
    private async void LoadCharacterDataDataSOAssets(Action callBack)
    {
        characterDataSOList = new Dictionary<ECharacterDataType, CharacterInfoDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("CharacterInfoData", typeof(CharacterInfoDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<CharacterInfoDataSO>(handle.Result[i]).Task;
            await task;

            characterDataSOList.Add(task.Result.characterType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
