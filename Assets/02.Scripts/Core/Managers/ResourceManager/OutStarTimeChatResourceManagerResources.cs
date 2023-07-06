using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, OutStarTimeChatDataSO> outStarTimeChatResourceManagerList;

    public OutStarTimeChatDataSO GetOutStarTimeChatResourceManager(string key)
    {
        return outStarTimeChatResourceManagerList[key];
    }

    private async void LoadOutStarTimeChatResourceManagerAssets(Action callBack)
    {
        outStarTimeChatResourceManagerList = new Dictionary<string, OutStarTimeChatDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("OutStarTimeChat", typeof(OutStarTimeChatDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<OutStarTimeChatDataSO>(handle.Result[i]).Task;
            await task;

            outStarTimeChatResourceManagerList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
