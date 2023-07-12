using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public abstract class ResourcesComponent : MonoBehaviour
{
    [SerializeField] private string label;
    protected Dictionary<string, ResourceSO> resourceDictionary;

    public abstract void LoadResources(Action callBack);

    protected async void LoadResourceDataAssets<T>(Action callBack) where T : ResourceSO
    {
        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync(label, typeof(T));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<T>(handle.Result[i]).Task;
            await task;
            Debug.Log(typeof(T) + task.Result.id);
            if (task.Result.id == null)
            {
                Debug.Log("null");
                continue;
            }
            resourceDictionary.Add(task.Result.id, task.Result);
        }
        Addressables.Release(handle);

        ResourceManager.Inst.AddResourcesComponent(typeof(T), this);
        callBack?.Invoke();
    }

    public ResourceSO GetResource(string key)
    {
        if (!resourceDictionary.ContainsKey(key)) return null;

        return resourceDictionary[key];
    }

    public Dictionary<string, ResourceSO> GetRsourceDictionary()
    {
        return resourceDictionary;
    }
}


