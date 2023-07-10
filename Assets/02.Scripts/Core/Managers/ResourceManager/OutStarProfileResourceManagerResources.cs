using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, OutStarCharacterDataSO> outStarProfileResourceManagerList;

    public List<OutStarCharacterDataSO> GetOutStarCharacterProfileToList()
    {
        return outStarProfileResourceManagerList.Select(x => x.Value).ToList();
    }

    public OutStarCharacterDataSO GetOutStarProfileResourceManager(string key)
    {
        return outStarProfileResourceManagerList[key];
    }

    private async void LoadOutStarProfileResourceManagerAssets(Action callBack)
    {
        outStarProfileResourceManagerList = new Dictionary<string, OutStarCharacterDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("OutStarProfile", typeof(OutStarCharacterDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<OutStarCharacterDataSO>(handle.Result[i]).Task;
            await task;

            outStarProfileResourceManagerList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
