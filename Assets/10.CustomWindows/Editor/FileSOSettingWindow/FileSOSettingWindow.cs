using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class FileSOSettingWindow : EditorWindow
{
    const string URL = "https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv&range=2:1000&gid=1984911729";

    private Button settingButton;

    [MenuItem("Tools/FileSOSettingWindow")]
    public static void ShowWindow()
    {
        FileSOSettingWindow win = GetWindow<FileSOSettingWindow>();
        win.minSize = new Vector2(350, 140);
        win.maxSize = new Vector2(350, 140);
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/FileSOSettingWindow/FileSOSettingWindow.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        settingButton = rootVisualElement.Q<Button>("SettingButton");
        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
    }

    private void Setting()
    {
        Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReadSheet(), new object[0]);
    }

    private IEnumerator ReadSheet()
    {
        UnityWebRequest www;

        www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        string add = www.downloadHandler.text;

        SettingFileSO(add);
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
                isCreate = true;
            }

            file.id = id;
            file.fileName = fileName;
            file.windowType = type;
            file.isFileLock = isFileLock;
            file.windowPin = pin;
            file.windowPinHintGuide = pinHint;

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
                Debug.Log(11);
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
        string temp = "Assets/07.ScriptableObjects/DirectorySO";
        for (int i = 3; i < splitPath.Length - 1; i++)
        {
            temp += '/' + splitPath[i];
            if (!Directory.Exists(temp))
            {
                Debug.Log(temp);
                Directory.CreateDirectory(temp);
            }
        }
        AssetDatabase.Refresh();
    }

}


