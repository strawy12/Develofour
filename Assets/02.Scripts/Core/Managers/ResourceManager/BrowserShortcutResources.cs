using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<int, BrowserShortcutDataSO> browserShortcutDataResourcesList;

    public BrowserShortcutDataSO GetBrowserShortcutData(int key)
    {
        if(browserShortcutDataResourcesList.ContainsKey(key))
        {
            return browserShortcutDataResourcesList[key];
        }
        return null;
    }
    public Dictionary<int, BrowserShortcutDataSO> GetBrowserShortcutDataList()
    {
        return browserShortcutDataResourcesList;
    }
    private async void LoadBrowserShortcutDataResourcesAssets(Action callBack)
    {
        browserShortcutDataResourcesList = new Dictionary<int, BrowserShortcutDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("BrowserShortcutData", typeof(BrowserShortcutDataSO));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<BrowserShortcutDataSO>(handle.Result[i]).Task;
            await task;

            browserShortcutDataResourcesList.Add(task.Result.fileId, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
