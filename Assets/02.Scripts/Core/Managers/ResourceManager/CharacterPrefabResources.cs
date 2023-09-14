using System;
using System.Collections;
using System.Collections.Generic;
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

        var handle = Addressables.LoadResourceLocationsAsync("CharacterPrefab", typeof(CharacterAnimator));
        await handle.Task;
        for (int i = 0; i < handle.Result.Count; i++)
        {
            var task = Addressables.LoadAssetAsync<CharacterAnimator>(handle.Result[i]).Task;
            await task;
            if (task.Result == null)
            {
                Debug.Log("null");
                continue;
            }
            characterPrefabDictionary.Add(task.Result.Type, task.Result);
        }
        Addressables.Release(handle);

        callBack?.Invoke();
    }
}
