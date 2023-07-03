using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, OutStarChatDataSO> outStarChatResourceManagerList;

    public OutStarChatDataSO GetOutStarChatResourceManager(string key)
    {
        return outStarChatResourceManagerList[key];
    }

    private async void LoadOutStarChatResourceManagerAssets(Action callBack)
    {
        outStarChatResourceManagerList = new Dictionary<string, OutStarChatDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("OutStarChat", typeof(OutStarChatDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<OutStarChatDataSO>(handle.Result[i]).Task;
            await task;

            outStarChatResourceManagerList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
