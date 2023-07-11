using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class CharacterDataResources : ResourcesComponent
{
    //public CharacterInfoDataSO GetCharacterByPhoneNumber(string phoneNumber)
    //{
    //    return characterDataSOList.Values.FirstOrDefault((x) => x.phoneNum == phoneNumber);
    //}
    //public List< CharacterInfoDataSO> GetCharacterDataSOList()
    //{
    //    List<CharacterInfoDataSO> results = new List<CharacterInfoDataSO>();
    //    foreach(var temp in characterDataSOList.Values)
    //    {
    //        results.Add(temp);
    //    }
    //    return results;
    //}
    public async override void LoadResourceDataAssets(Action callBack)
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();
        var handle = Addressables.LoadResourceLocationsAsync("CharacterInfoData", typeof(CharacterInfoDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<CharacterInfoDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
