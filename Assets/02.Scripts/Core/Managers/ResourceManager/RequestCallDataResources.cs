using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<ECharacterDataType, RequestCallDataSO> requestCallDataList;

    public RequestCallDataSO GetRequestCallData(ECharacterDataType key)
    {
        return requestCallDataList[key];
    }

    private async void LoadRequestCallDataAssets(Action callBack)
    {
        requestCallDataList = new Dictionary<ECharacterDataType, RequestCallDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("filler text", typeof(RequestCallDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<RequestCallDataSO>(handle.Result[i]).Task;
            await task;

            requestCallDataList.Add(task.Result.characterType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
