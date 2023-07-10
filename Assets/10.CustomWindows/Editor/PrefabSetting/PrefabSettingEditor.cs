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

    private void OutStarPrefabSetting()
    {
        string[] characterGuids = AssetDatabase.FindAssets("t:OutStarCharacterDataSO", null);
        List<OutStarCharacterDataSO> characterList = new List<OutStarCharacterDataSO>();
        foreach(var guid in characterGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            characterList.Add(AssetDatabase.LoadAssetAtPath<OutStarCharacterDataSO>(path));
        }
    }
}
