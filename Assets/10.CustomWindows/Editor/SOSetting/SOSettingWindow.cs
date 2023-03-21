using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEngine.Networking;
using System;
using System.IO;
using System.Collections;
using System.Threading;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using Object = System.Object;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class SOSettingWindow : EditorWindow
{
    const string URL = "https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv&range=2:1000";
    //https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv&gid=1984911729

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
        Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReadSheet(sheetField.text), new object[0]);
    }

    private IEnumerator ReadSheet(string id)
    {
        UnityWebRequest www;


        if (id == "")
        {
            www = UnityWebRequest.Get($"{URL}");
        }
        else
        {
            www = UnityWebRequest.Get($"{URL}&gid={id}");
        }
        yield return www.SendWebRequest();
        string add = www.downloadHandler.text;

        switch (scriptField.text)
        {
            case "FileSO":
                {
                    SettingFileSO(add);
                }
                break;
        }

    }


    public SOParent ByteToObject(byte[] buffer)
    {
        try
        {
            using (MemoryStream stream = new MemoryStream(buffer))
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                stream.Position = 0;
                SOParent soparent = binaryFormatter.Deserialize(stream) as SOParent;
                return soparent;
            }
        }
        catch (Exception exception)
        {
            Debug.Log(exception.ToString());
        }
        return null;
    }

    public static Object binaryDeserialize(FileStream stream)
    {
        //MemoryStream stream = new MemoryStream(bytes);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.AssemblyFormat
        = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
        Object obj = (Object)formatter.Deserialize(stream);
        return obj;
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
            bool isMultiple = columns[4] == "TRUE";
            bool isFileLock = columns[5] == "TRUE";
             
            string pin = columns[6];
            string pinHint = columns[7];

            bool isAlram = columns[8] == "TRUE";

            List<int> childIdList = new List<int>();

            string[] children = columns[11].Split(',');
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
                file = CreateInstance<FileSO>();
                isCreate = true;
            }

            file.fileName = fileName;
            file.windowType = type;
            file.isMultiple = isMultiple;
            file.isFileLock = isFileLock;
            file.windowPin = pin;
            file.windowPinHintGuide = pinHint;
            file.isAlarm = isAlram;

            if (file is DirectorySO)
            {
                (file as DirectorySO).children.Clear();
                foreach (int childID in childIdList)
                {
                    FileSO child = fileSOList.Find(x => x.id == childID);
                    Debug.Log($"{child}_{childID}");
                    child.parent = file as DirectorySO;
                    (file as DirectorySO).children.Add(child);
                }
            }

            if (isCreate)
            {
                string SO_PATH = $"Assets/{file.fileName}.asset";
                AssetDatabase.CreateAsset(file, SO_PATH);
                AssetDatabase.Refresh();
            }
        }
    }

    public void CreateRoot(string[] root)
    {
        string current = $"Assets/07.ScriptableObjects/DirectorySO";

        for (int i = 0; i < root.Length; i++)
        {
            current = current + "/" + root[i];
            if (!Directory.Exists(current))
            {
                Directory.CreateDirectory(current);
            }
        }
    }

    public void SetChildren(string[] hor, object getObj)
    {
        string path = $"Assets/07.ScriptableObjects/DirectorySO";
        string[] root = hor[0].Split('/');
        for (int i = 0; i < root.Length - 1; i++)
        {
            path += '/';
            path += root[i];
        }
        path += "/" + root[root.Length - 2];
        path += ".asset";
        if (File.Exists(path))
        {
            object obj = AssetDatabase.LoadAssetAtPath(path, typeof(DirectorySO));
            if (obj != null)
            {
                DirectorySO directory = obj as DirectorySO;
                FileSO soObj = getObj as FileSO;

                if (directory.children == null)
                {
                    directory.children = new System.Collections.Generic.List<FileSO>();
                }
                directory.children.Add(soObj);
                soObj.parent = directory;
            }
        }
    }
}


