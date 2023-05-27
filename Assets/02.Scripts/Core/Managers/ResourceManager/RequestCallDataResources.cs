using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<ECharacterDataType, RequestCallDataSO> requestCallDataList;

    [SerializeField]
    private Dictionary<ECharacterDataType, IncomingCallDataSO> incomingCallDataList;
    public Dictionary<ECharacterDataType, IncomingCallDataSO> IncomingCallDataList => incomingCallDataList;

    public IncomingCallDataSO GetIncomingCallData(ECharacterDataType key)
    {
        return incomingCallDataList[key];
    }
    public RequestCallDataSO GetRequestCallData(ECharacterDataType key)
    {
        return requestCallDataList[key];
    }

    private async void LoadRequestCallDataAssets(Action callBack)
    {
        requestCallDataList = new Dictionary<ECharacterDataType, RequestCallDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("RequestData", typeof(RequestCallDataSO));
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

    private async void LoadIncomingCallDataAssets(Action callBack)
    {
        incomingCallDataList = new Dictionary<ECharacterDataType, IncomingCallDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("IncomingData", typeof(IncomingCallDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<IncomingCallDataSO>(handle.Result[i]).Task;
            await task;

            incomingCallDataList.Add(task.Result.characterType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
