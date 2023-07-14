using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections.Generic;

public class PrefabSettingWindow : EditorWindow
{
    private Button outStarPrefabBtn;


    [MenuItem("Tools/PrefabSetting")]
    public static void ShowWindow()
    {
        PrefabSettingWindow win = GetWindow<PrefabSettingWindow>();

    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/PrefabSetting/PrefabSettingWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        outStarPrefabBtn = rootVisualElement.Q<Button>("OutStarPrefabBtn");

        //outStarPrefabBtn.RegisterCallback(x=>)
    }

  
}
