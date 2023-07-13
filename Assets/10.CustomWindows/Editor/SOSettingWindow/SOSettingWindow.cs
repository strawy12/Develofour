﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
public partial class SOSettingWindow : EditorWindow
{
    const string URL = @"https://docs.google.com/spreadsheets/d/1AYfKB3JgfR8zXXDccow0jJCuqa-CJJP9cBjdOlUIusw/export?format=tsv&range=2:1000&gid={0}";

    private string gid;
    [MenuItem("Tools/SOSettingWindow")]
    public static void ShowWindow()
    {
        SOSettingWindow win = GetWindow<SOSettingWindow>();
        win.minSize = new Vector2(350, 250);
        win.maxSize = new Vector2(350, 250);
    }

    private enum ESOType
    {
        None,
        File,
        Monolog,
        AIChatting,
        ProfilerCategory,
        ProfilerInfo,
        ProfilerGuide,
    }
    #region UIBuilderParam
    private Button settingButton;
    private Button fileSOBtn;
    private Button monologSOBtn;
    private Button aiChattingBtn;
    private Button profilerCategoryBtn;
    private Button profilerInfoBtn;
    private Button profilerGuideBtn;

    private Label gidText;
    private Label soTypeText;
    #endregion

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/SOSettingWindow/SOSettingWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        settingButton = rootVisualElement.Q<Button>("SOSettingBtn");
        fileSOBtn = rootVisualElement.Q<Button>("FileSOBtn");
        monologSOBtn = rootVisualElement.Q<Button>("TextSOBtn");
        aiChattingBtn = rootVisualElement.Q<Button>("AIChattingBtn");
        profilerCategoryBtn = rootVisualElement.Q<Button>("ProfilerCategoryBtn");
        profilerInfoBtn = rootVisualElement.Q<Button>("ProfilerInfoBtn");
        profilerGuideBtn = rootVisualElement.Q<Button>("ProfilerGuideBtn");

        gidText = rootVisualElement.Q<Label>("GidText");
        soTypeText = rootVisualElement.Q<Label>("SOTypeText");

        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
        fileSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.File));
        monologSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.Monolog));
        aiChattingBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.AIChatting));
        profilerCategoryBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerCategory));
        profilerInfoBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerInfo));
        profilerGuideBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerGuide));

    }

    private void AutoComplete(ESOType type)
    {
        switch (type)
        {
            case ESOType.File:
                gidText.text = "2075656520";
                soTypeText.text = "FileSO";
                break;

            case ESOType.Monolog:
                gidText.text = "764708499";
                soTypeText.text = "TextDataSO";
                break;
            case ESOType.AIChatting:
                gidText.text = "36720751";
                soTypeText.text = "AIChattingDataSO";
                break;

            case ESOType.ProfilerCategory:
                gidText.text = "82908635";
                soTypeText.text = "ProfilerCategoryDataSO";
                break;

            case ESOType.ProfilerInfo:
                gidText.text = "1620940611";
                soTypeText.text = "ProfilerInfoDataSO";
                break;

            case ESOType.ProfilerGuide:
                gidText.text = "1489579503";
                soTypeText.text = "ProfilerGuideBtn";
                break;
        }
    }

    private void Setting()
    {
        Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReadSheet(), new object[0]);
    }

    private IEnumerator ReadSheet()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get(string.Format(URL, gidText.text));
        Debug.Log(string.Format(URL, gidText.text));
        yield return www.SendWebRequest();
        string add = www.downloadHandler.text.Replace("\r", "");

        switch (soTypeText.text)
        {
            case "FileSO":
                //SettingFileSO(add, soTypeField.text);
                break;

            case "TextDataSO":
                SettingMonologSO(add, soTypeText.text);
                break;

            case "AIChattingDataSO":
                SettingAIChattingSO(add, soTypeText.text);
                break;

            case "ProfilerCategoryDataSO":
                SettingCategorySO(add, soTypeText.text);
                break;

            case "ProfilerInfoDataSO":
                SettingProfilerInfoSO(add, soTypeText.text);
                break;

            case "ProfilerGuideBtn":
                SettingProfilerGuideSO(add, soTypeText.text);
                break;
        }
    }

    private void CreateFolder(string path)
    {
        string[] splitPath = path.Split('/');
        string temp = "Assets";
        for (int i = 1; i < splitPath.Length - 1; i++)
        {
            temp += '/' + splitPath[i];
            if (!Directory.Exists(temp))
            {
                Directory.CreateDirectory(temp);
            }
        }
        AssetDatabase.Refresh();

    }

    private List<T> GuidsToSOList<T>(string filtter) where T : UnityEngine.Object
    {
        string[] guids = AssetDatabase.FindAssets(filtter, null);
        List<T> SOList = new List<T>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SOList.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }

        return SOList;
    } 
}