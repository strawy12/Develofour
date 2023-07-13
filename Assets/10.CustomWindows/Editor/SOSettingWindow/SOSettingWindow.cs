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
    const string URL = @"https://docs.google.com/spreadsheets/d/1AYfKB3JgfR8zXXDccow0jJCuqa-CJJP9cBjdOlUIusw/export?format=tsv&range=2:1000&edit#gid={0}";

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
    }

    private Button settingButton;
    private Button fileSOBtn;
    private Button monologSOBtn;

    private Label gidText;
    private Label soTypeField;


    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/SOSettingWindow/SOSettingWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        settingButton = rootVisualElement.Q<Button>("SOSettingBtn");
        fileSOBtn = rootVisualElement.Q<Button>("FileSOBtn");
        monologSOBtn = rootVisualElement.Q<Button>("MonologBtn");

        gidText = rootVisualElement.Q<Label>("GidText");
        soTypeField = rootVisualElement.Q<Label>("SOTypeText");

        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
        fileSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.File));
        monologSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.Monolog));
    }

    private void AutoComplete(ESOType type)
    {
        switch (type)
        {
            case ESOType.File:
                gidText.text = "2075656520";
                soTypeField.text = "FileSO";
                break;

            case ESOType.Monolog:
                gidText.text = "764708499";
                soTypeField.text = "MonologTextDataSO";
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
        yield return www.SendWebRequest();
        string add = www.downloadHandler.text.Replace("\r", "");

        switch (soTypeField.text)
        {
            case "FileSO":
                //SettingFileSO(add, soTypeField.text);
                break;

            case "MonologTextDataSO":
                SettingMonologSO(add, soTypeField.text);
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

}