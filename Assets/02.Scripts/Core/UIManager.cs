using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField]
    private WindowPrefabListSO windowPrefabListSO;

    [SerializeField]
    private Transform windowCanvasTrm;

    private Dictionary<string, Window> windowPrefabDictionary;

    private void Awake()
    {
        windowPrefabDictionary = new Dictionary<string, Window>();

        windowPrefabListSO.windowList.ForEach(window => AddWindowPrefab(window));
    }

    public void AddWindowPrefab(Window window)
    {
        if (windowPrefabDictionary.ContainsKey(window.name)) return;

        windowPrefabDictionary[window.gameObject.name] = window;
    }

    public Window CreateWindow(Window prefab, Transform parent = null)
    {
        return CreateWindow(prefab.gameObject.name, parent);
    }

    public Window CreateWindow(string prefabName, Transform parent = null)
    {
        if (!windowPrefabDictionary.ContainsKey(prefabName)) return null;

        parent ??= windowCanvasTrm;
        
        Window prefab = windowPrefabDictionary[prefabName];

        Window window = Instantiate(prefab, parent);
        window.gameObject.name = prefabName;
        return window;
    }

}
