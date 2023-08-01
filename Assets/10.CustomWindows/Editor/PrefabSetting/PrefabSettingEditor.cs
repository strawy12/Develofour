using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class PrefabSettingWindow : EditorWindow
{
    private Button outStarPrefabBtn;


    [MenuItem("Tools/PrefabSetting")]
    public static void ShowWindow()
    {
        PrefabSettingWindow win = GetWindow<PrefabSettingWindow>();
        win.minSize = new Vector2(350, 250);
        win.maxSize = new Vector2(350, 250);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/PrefabSetting/PrefabSettingWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        outStarPrefabBtn = rootVisualElement.Q<Button>("OutStarPrefabBtn");

        outStarPrefabBtn.RegisterCallback<MouseUpEvent>(x => OutStarGramSetting());
    }


}
