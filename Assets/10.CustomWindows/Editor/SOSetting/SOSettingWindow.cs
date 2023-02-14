using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.IO;

public class SOSettingWindow : EditorWindow
{
    const string URL = "https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q";

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
        ReadSheet();
    }

    private void ReadSheet()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        string add = www.downloadHandler.text;

        string[] ver = add.Split('\n'); //열

        string[] firstLine = ver[0].Split('\n'); //행

        Type soType = Type.GetType(ver[0]); 

        Debug.Log(soType);

        for (int i = 1; i < ver.Length; i++)
        {
            string[] hor = ver[i].Split('\t');

            string SO_PATH = $"Assets/Resources/{ver[0]}/{ver[0]}_{ver[i]}.asset";

            if(File.Exists(SO_PATH))
            {
                //파일 잇음
                SOParent soObj = Resources.Load($"{ver[0]}/{ver[0]}_{ver[i]}.asset") as SOParent;
                soObj.Setting(hor);
                AssetDatabase.CreateAsset(soObj, SO_PATH);
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
