using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public partial class ResourceManager : MonoSingleton<ResourceManager>
{
    Dictionary<ECharacterType, CharacterAnimator> characterPrefabDictionary;

    public CharacterAnimator GetCharacterPrefab(ECharacterType type)
    {
        CharacterAnimator prefab = null;
        if (characterPrefabDictionary.TryGetValue(type, out prefab))
            return prefab;

        return null;
    }

    private async void LoadCharacterprefabs(Action callBack)
    {
        characterPrefabDictionary = new Dictionary<ECharacterType, CharacterAnimator>();

        var handle = Addressables.LoadResourceLocationsAsync("CharacterPrefab");
        await handle.Task;
        Debug.Log(handle.Result);

        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<GameObject>(handle.Result[i]).Task;
            await task;
            Debug.Log(task.Result);
            if (task.Result == null)
            { 
                continue;
            }
            CharacterAnimator result = task.Result.GetComponent<CharacterAnimator>();
            characterPrefabDictionary.Add(result.Type, result);
        }
        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
