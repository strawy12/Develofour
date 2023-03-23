using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<EAIChattingTextDataType, AIChattingTextDataSO> aiChattingTextDataSOList;

    public AIChattingTextDataSO GetAIChattingTextDataSO(EAIChattingTextDataType textType)
    {
        return aiChattingTextDataSOList[textType];
    }

    private async void LoadAIChattingTextDataSOAssets(Action callBack)
    {
        aiChattingTextDataSOList = new Dictionary<EAIChattingTextDataType, AIChattingTextDataSO>();
        var handle = Addressables.LoadResourceLocationsAsync("AIChattingTextData", typeof(AIChattingTextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<AIChattingTextDataSO>(handle.Result[i]).Task;
            await task;

            aiChattingTextDataSOList.Add(task.Result.TextDataType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
