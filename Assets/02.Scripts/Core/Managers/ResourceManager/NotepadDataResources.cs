using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    [SerializeField]
    private Dictionary<int, NotepadDataSO> notepadDataList;

    public NotepadDataSO GetNotepadData(int key)
    {
        return notepadDataList[key];
    }

    private async void LoadNotepadDataAssets(Action callBack)
    {
        notepadDataList = new Dictionary<int, NotepadDataSO>();

        var handle = Addressables.LoadResourceLocationsAsync("NotepadData", typeof(NotepadDataSO));
        await handle.Task;

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<NotepadDataSO>(handle.Result[i]).Task;
            await task;
            Debug.Log(task.Result.name);
            notepadDataList.Add(task.Result.fileId, task.Result);
        }

        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
