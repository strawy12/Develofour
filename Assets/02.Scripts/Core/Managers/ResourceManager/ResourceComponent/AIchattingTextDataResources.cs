using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class AIchattingTextDataResources : ResourcesComponent
{

    //public AIChattingTextDataSO GetAIChattingTextDataSO(EAIChattingTextDataType textType)
    //{
    //    return [textType];
    //}

    public override async void LoadResourceDataAssets(Action callBack)
    {
        soType = typeof(AIChattingTextDataSO);

        resourceDictionary = new Dictionary<string, ResourceSO>();

        var handle = Addressables.LoadResourceLocationsAsync("AIChattingTextData", typeof(AIChattingTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<AIChattingTextDataSO>(handle.Result[i]).Task;
            await task;

            resourceDictionary.Add(task.Result.id, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
