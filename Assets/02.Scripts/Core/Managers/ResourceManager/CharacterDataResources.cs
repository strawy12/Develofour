using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, CharacterInfoDataSO> characterDataSOList;

    public CharacterInfoDataSO GetCharacterDataSO(ECharacterDataType characterType)
    {
        return characterDataSOList.Values.FirstOrDefault(x=>x.characterType == characterType);
    }
    public CharacterInfoDataSO GetCharacterByPhoneNumber(string phoneNumber)
    {
        return characterDataSOList.Values.FirstOrDefault((x) => x.phoneNum == phoneNumber);
    }
    public CharacterInfoDataSO GetCharacterDataSO(string id)
    {
        return characterDataSOList[id];
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
        characterDataSOList = new Dictionary<string, CharacterInfoDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("CharacterInfoData", typeof(CharacterInfoDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<CharacterInfoDataSO>(handle.Result[i]).Task;
            await task;

            characterDataSOList.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
