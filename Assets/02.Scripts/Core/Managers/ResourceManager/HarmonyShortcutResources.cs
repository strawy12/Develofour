using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, HarmonyShortcutDataSO> harmonyShortcutDataResourcesList;

    public HarmonyShortcutDataSO GetHarmonyShortcutData(string key)
    {
        if(harmonyShortcutDataResourcesList.ContainsKey(key))
        {
            return harmonyShortcutDataResourcesList[key];
        }
        return null;
    }
    public Dictionary<string, HarmonyShortcutDataSO> GetHarmonyShortcutDataList()
    {
        return harmonyShortcutDataResourcesList;
    }
    private async void LoadHarmonyShortcutDataResourcesAssets(Action callBack)
    {
        harmonyShortcutDataResourcesList = new Dictionary<string, HarmonyShortcutDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("HarmonyShortcutData", typeof(HarmonyShortcutDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<HarmonyShortcutDataSO>(handle.Result[i]).Task;
            await task;

            harmonyShortcutDataResourcesList.Add(task.Result.fileName, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
