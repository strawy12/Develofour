using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Dictionary<ETextDataType, TextDataSO> textDataSOList;

    public TextDataSO GetTextDataSO(ETextDataType textType)
    {
        return textDataSOList[textType];
    }

    private async void LoadTextDataSOAssets(Action callBack)
    {
        textDataSOList = new Dictionary<ETextDataType, TextDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("TextData", typeof(TextDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<TextDataSO>(handle.Result[i]).Task;
            await task;

            textDataSOList.Add(task.Result.TextDataType, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
