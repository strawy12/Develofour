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

public class SOSettingWindow : EditorWindow
{
    const string URL = @"https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv&range=2:1000&gid={0}";

    private enum ESOType
    {
        None,
        File,
        Monolog
    }

    private Button settingButton;
    private Button fileSOBtn;
    private Button monologSOBtn;

    private TextField gidField;
    private TextField soTypeField;


    [MenuItem("Tools/SOSettingWindow")]
    public static void ShowWindow()
    {
        SOSettingWindow win = GetWindow<SOSettingWindow>();
        win.minSize = new Vector2(350, 200);
        win.maxSize = new Vector2(350, 200);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/SOSettingWindow/SOSettingWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        settingButton = rootVisualElement.Q<Button>("SettingButton");
        fileSOBtn = rootVisualElement.Q<Button>("FileSOBtn");
        monologSOBtn = rootVisualElement.Q<Button>("MonologBtn");
        gidField = rootVisualElement.Q<TextField>("GidField");
        soTypeField = rootVisualElement.Q<TextField>("SOTypeField");
        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
        fileSOBtn.RegisterCallback<MouseUpEvent>(x => Autocomplete(ESOType.File));
        monologSOBtn.RegisterCallback<MouseUpEvent>(x => Autocomplete(ESOType.Monolog));
    }



    private void Autocomplete(ESOType type)
    {

        switch (type)
        {
            case ESOType.File:
                gidField.value = "2075656520";
                soTypeField.value = "FileSO";
                break;

            case ESOType.Monolog:
                gidField.value = "441334984";
                soTypeField.value = "MonologTextDataSO";
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

        www = UnityWebRequest.Get(string.Format(URL, gidField.value));
        yield return www.SendWebRequest();
        string add = www.downloadHandler.text;

        switch (soTypeField.value)
        {
            case "FileSO":
                SettingFileSO(add);
                break;

            case "MonologTextDataSO":
                SettingMonologSO(add);
                break;
        }

    }

    public void SettingMonologSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        string[] guids = AssetDatabase.FindAssets("t:MonologTextDataSO", null);
        List<MonologTextDataSO> monologSOList = new List<MonologTextDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            monologSOList.Add(AssetDatabase.LoadAssetAtPath<MonologTextDataSO>(path));
        }

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            int id = int.Parse(columns[0]);
            string fileName = columns[1];
            string monologName = columns[2];


            MonologTextDataSO monologData = monologSOList.Find(x => x.TextDataType == id);
            bool isCreate = false;

            if (monologData == null)
            {
                monologData = CreateInstance<MonologTextDataSO>();
                isCreate = true;
            }

            monologData.TextDataType = id;
            monologData.name = fileName;
            monologData.monologName = monologName;

            string[] textDataList = columns[3].Split('#');

            if (monologData.textDataList == null)
            {
                monologData.textDataList = new List<TextData>();
            }

            for (int j = 0; j < textDataList.Length; j++)
            {
                TextData data = new TextData { text = textDataList[j] };
                if (monologData.Count <= j)
                {
                    monologData.textDataList.Add(data);
                }
                else
                {
                monologData[j] = data;
                }
            }

            string SO_PATH = $"Assets/07.ScriptableObjects/TextDataSO/CreateMonolog/{fileName}.asset";
            SO_PATH = SO_PATH.Replace("\\", "/");


            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(monologData, SO_PATH);
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public void SettingFileSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        string[] guids = AssetDatabase.FindAssets("t:FileSO", null);
        List<FileSO> fileSOList = new List<FileSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            fileSOList.Add(AssetDatabase.LoadAssetAtPath<FileSO>(path));
        }
        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            int id = int.Parse(columns[0]);
            string fileName = columns[1];

            EWindowType type = (EWindowType)Enum.Parse(typeof(EWindowType), columns[2]);

            bool isFileLock = columns[4] == "TRUE";
            string pin = columns[5];
            string pinHint = columns[6];

            List<int> childIdList = new List<int>();
            string tagString = columns[7];
            List<string> tags = new List<string>();
            if (tagString != "")
            {
               tags = tagString.Split(',').ToList();
            }
            string[] children = columns[8].Split(',');
            foreach (string child in children)
            {
                string newChild = Regex.Replace(child, "[^0-9]", "");

                if (string.IsNullOrEmpty(newChild)) continue;

                childIdList.Add(int.Parse(newChild));
            }

            FileSO file = fileSOList.Find(x => x.id == id);
            bool isCreate = false;

            if (file == null)
            {
                if (type == EWindowType.Directory)
                {
                    file = CreateInstance<DirectorySO>();
                    (file as DirectorySO).children = new List<FileSO>();

                }
                else
                {
                    file = CreateInstance<FileSO>();
                }

                file.name = columns[9];
                isCreate = true;
            }
            
            file.id = id;
            file.fileName = fileName;
            file.windowType = type;
            file.isFileLock = isFileLock;
            file.windowPin = pin;
            file.windowPinHintGuide = pinHint;
            file.tags = tags;

            if (file is DirectorySO)
            {
                DirectorySO directory = (DirectorySO)file;
                directory.children.Clear();
                foreach (int childID in childIdList)
                {
                    FileSO child = fileSOList.Find(x => x.id == childID);
                    if (child == null) continue;
                    //Debug.Log($"{child}_{childID}");
                    child.parent = directory;
                    directory.children.Add(child);
                }
            }

            string path = file.GetRealFileLocation();
            string SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path.Remove(path.Length - 1)}.asset";
            SO_PATH = SO_PATH.Replace("\\", "/");

            CreateFolder(SO_PATH);

            if (!File.Exists(SO_PATH) && string.IsNullOrEmpty(columns[9]))
            {
                string oldPath = AssetDatabase.GetAssetPath(file.GetInstanceID());
                AssetDatabase.MoveAsset(oldPath, SO_PATH);
            }
            EditorUtility.SetDirty(file);

            if (isCreate)
            {
                if (File.Exists(SO_PATH))
                {
                    SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path}_{id}.asset";
                }

                CreateFolder(SO_PATH);

                AssetDatabase.CreateAsset(file, SO_PATH);
            }
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

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


