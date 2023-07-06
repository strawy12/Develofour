using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, CallProfileDataSO> callProfileDataList;

    public CallProfileDataSO GetCallProfileData(string key)
    {
        if(callProfileDataList.ContainsKey(key))
        {
            return callProfileDataList[key];
        }
        return null;
    }
    private async void LoadCallProfileDataResourcesAssets(Action callBack)
    {
        callProfileDataList = new Dictionary<string, CallProfileDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("CallProfileData", typeof(CallProfileDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<CallProfileDataSO>(handle.Result[i]).Task;
            await task;

            callProfileDataList.Add(task.Result.CharacterID, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
