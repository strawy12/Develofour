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
        Character,
        CallProfile,
    }
    #region UIBuilderParam
    private Button settingButton;
    private Button fileSOBtn;
    private Button monologSOBtn;
    private Button aiChattingBtn;
    private Button profilerCategoryBtn;
    private Button profilerInfoBtn;
    private Button profilerGuideBtn;
    private Button characterDataBtn;
    private Button CallProfileDataBtn;

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
        characterDataBtn = rootVisualElement.Q<Button>("CharacterBtn");
        CallProfileDataBtn = rootVisualElement.Q<Button>("CallProfileBtn");
        gidText = rootVisualElement.Q<Label>("GidText");
        soTypeText = rootVisualElement.Q<Label>("SOTypeText");

        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
        fileSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.File));
        monologSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.Monolog));
        aiChattingBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.AIChatting));
        profilerCategoryBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerCategory));
        profilerInfoBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerInfo));
        profilerGuideBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerGuide));
        characterDataBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.Character));
        CallProfileDataBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.CallProfile));
    }

    private void AutoComplete(ESOType type)
    {
        switch (type)
        {
            case ESOType.File:
                gidText.text = "0";
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
            case ESOType.Character:
                gidText.text = "1030073895";
                soTypeText.text = "CharacterDataSO";
                break;
            case ESOType.CallProfile:
                gidText.text = "2090943632";
                soTypeText.text = "CallProfileDataSO";
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
                SettingFileSO(add);
                break;
            case "TextDataSO":
                SettingMonologSO(add);
                break;
            case "AIChattingDataSO":
                SettingAIChattingSO(add);
                break;
            case "ProfilerCategoryDataSO":
                SettingCategorySO(add);
                break;
            case "ProfilerInfoDataSO":
                SettingProfilerInfoSO(add);
                break;
            case "ProfilerGuideBtn":
                SettingProfilerGuideSO(add);
                break;
            case "CharacterDataSO":
                SettingCharacterData(add);
                break;
            case "CallProfileDataSO":
                CallProfileSOSetting(add);
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