using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    private Stack<GameObject> lockImagePool;
    private GameObject lockImagePrefab;


    public GameObject GetLockImage()
    {
        GameObject lockImage;
        if (lockImagePool.Count > 0)
        {
            lockImage = lockImagePool.Pop();
        }

        else
        {
            lockImage = Instantiate(lockImagePrefab);
        }

        lockImage.SetActive(true);
        return lockImage;

    }

    public void PushLockImage(GameObject lockImage)
    {
        lockImage.gameObject.SetActive(false);
        lockImagePool.Push(lockImage);
    }

    private async void LoadLockImage(Action callBack)
    {
        lockImagePool = new Stack<GameObject>();

        var task = Addressables.LoadAssetAsync<GameObject>("LockImage");
        await task.Task;
        lockImagePrefab = task.Result;

        callBack?.Invoke();
    }
}
