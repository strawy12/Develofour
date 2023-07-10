using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<string, AIChattingDataSO> aIChattingDataSOList;

    public Dictionary<string, AIChattingDataSO> AIChattingDataSOList
    {
        get => AIChattingDataSOList;
    }
    public AIChattingDataSO GetAIChattingDataSO(string key)
    {
        return aIChattingDataSOList[key];
    }

    private async void LoadAIChattingDataSOAssets(Action callBack)
    {
        aIChattingDataSOList = new Dictionary<string, AIChattingDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("AIChattingDataSO", typeof(AIChattingDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<AIChattingDataSO>(handle.Result[i]).Task;
            await task;

            aIChattingDataSOList.Add(task.Result.ID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
