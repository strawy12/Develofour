using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<int, ProfileInfoTextDataSO> profileInfoDataList;

    public ProfileInfoTextDataSO GetProfileInfoData(int key)
    {
        return profileInfoDataList[key];
    }

    private async void LoadProfileInfoDataAssets(Action callBack)
    {
        profileInfoDataList = new Dictionary<int, ProfileInfoTextDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("filler text", typeof(ProfileInfoTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfileInfoTextDataSO>(handle.Result[i]).Task;
            await task;

            profileInfoDataList.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
