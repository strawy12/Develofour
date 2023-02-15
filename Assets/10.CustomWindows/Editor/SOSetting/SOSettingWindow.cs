using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Reflection;

public class SOSettingWindow : EditorWindow
{

    const string URL = "https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv";

    private TextField sheetField;
    private TextField scriptField;
    private Button settingButton;

    [MenuItem("Tools/SOSettingWindow")]
    public static void ShowWindow()
    {
        SOSettingWindow win = GetWindow<SOSettingWindow>();
        win.minSize = new Vector2(350, 240);
        win.maxSize = new Vector2(350, 240);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/SOSetting/SOSetting.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        sheetField = rootVisualElement.Q<TextField>("SheetIDField");
        scriptField = rootVisualElement.Q<TextField>("SOScriptField");
        settingButton = rootVisualElement.Q<Button>("SettingButton");
        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
    }

    private void Setting()
    {
        Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReadSheet(), new object[0]);
    }

    private IEnumerator ReadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();
        string add = www.downloadHandler.text;
        string[] ver = add.Split('\n'); //ї­

        string[] firstLine = ver[0].Split('\t'); //За

        Assembly asm = typeof(SOParent).Assembly;
        Type soType = asm.GetType(firstLine[0]);

        string[] top = ver[0].Split('\t');

        if(!Directory.Exists($"Assets/Resources/{top[0]}"))
        {
            Directory.CreateDirectory($"Assets/Resources/{top[0]}");
        }

        for (int i = 1; i < ver.Length; i++)
        {
            string[] hor = ver[i].Split('\t');

            string SO_PATH = $"Assets/Resources/{top[0]}/{hor[0]}.asset";
            Debug.Log(SO_PATH);

            if (File.Exists(SO_PATH))
            {
                SOParent soObj = Resources.Load($"{top[0]}/{hor[0]}") as SOParent;
                soObj.Setting(hor);
            }
            else
            {
                object obj = CreateInstance(soType);
                SOParent soObj = obj as SOParent;
                soObj.Setting(hor);
                AssetDatabase.CreateAsset(soObj, SO_PATH);
            }
        }

    }
}
