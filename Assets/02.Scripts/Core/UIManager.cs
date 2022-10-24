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

        windowPrefabDictionary[window.Info.WindowName] = window;
    }

    public Window CreateWindow(string windowID, Transform parent = null)
    {
        if (!windowPrefabDictionary.ContainsKey(windowID)) return null;

        parent ??= windowCanvasTrm;
        
        Window prefab = windowPrefabDictionary[windowID];

        Window window = Instantiate(prefab, parent);

        return window;
    }

}
