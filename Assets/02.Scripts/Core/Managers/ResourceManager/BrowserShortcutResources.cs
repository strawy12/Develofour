using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<string, BrowserShortcutDataSO> browserShortcutDataResourcesList;

    public BrowserShortcutDataSO GetBrowserShortcutData(string key)
    {
        if(browserShortcutDataResourcesList.ContainsKey(key))
        {
            return browserShortcutDataResourcesList[key];
        }
        return null;
    }
    public Dictionary<string, BrowserShortcutDataSO> GetBrowserShortcutDataList()
    {
        return browserShortcutDataResourcesList;
    }
    private async void LoadBrowserShortcutDataResourcesAssets(Action callBack)
    {
        browserShortcutDataResourcesList = new Dictionary<string, BrowserShortcutDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("BrowserShortcutData", typeof(BrowserShortcutDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<BrowserShortcutDataSO>(handle.Result[i]).Task;
            await task;

            browserShortcutDataResourcesList.Add(task.Result.fileName, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
