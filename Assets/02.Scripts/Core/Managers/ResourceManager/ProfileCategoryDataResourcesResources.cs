using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<EProfileCategory, ProfileCategoryDataSO> profileCategoryDataResourcesList;

    public ProfileCategoryDataSO GetProfileCategoryData(EProfileCategory key)
    {
        return profileCategoryDataResourcesList[key];
    }
    public Dictionary<EProfileCategory, ProfileCategoryDataSO> GetProfileCategoryDataList()
    {
        return profileCategoryDataResourcesList;
    }
    private async void LoadProfileCategoryDataResourcesAssets(Action callBack)
    {
        profileCategoryDataResourcesList = new Dictionary<EProfileCategory, ProfileCategoryDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("ProfileCategory", typeof(ProfileCategoryDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<ProfileCategoryDataSO>(handle.Result[i]).Task;
            await task;

            profileCategoryDataResourcesList.Add(task.Result.category, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
