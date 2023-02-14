using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Threading;
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
        //함수넣기
        Debug.Log("세팅이요");
        EditorCoroutine.StartCoroutine(ReadSheet());
    }

    private IEnumerator ReadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);

        yield return www.SendWebRequest();
        string data = www.downloadHandler.text;
        Debug.Log(data);
        string[] ver = data.Split('\n');

        string[] firstLine = ver[0].Split('\n');

        Type soType = Type.GetType(ver[0]);
        Debug.Log(ver[0]);
        Debug.Log(soType);

        for (int i = 1; i < ver.Length; i++)
        {
            string[] hor = ver[i].Split('\t');

            object obj = CreateInstance(soType);
            SOParent soObj = obj as SOParent;
            soObj.Setting(hor);
        }
    }
}
