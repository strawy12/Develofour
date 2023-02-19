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

public class SOSettingWindow : EditorWindow
{
    const string URL = "https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv";
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

        string[] ver = add.Split('\n'); //��

        string[] firstLine = ver[0].Split('\t'); //��
        Assembly asm = typeof(SOParent).Assembly;
        Type soType = asm.GetType(firstLine[0]);

        string[] top = ver[0].Split('\t');

        if(soType == typeof(FileSO))
        {
            StartFileSOCreate(ver, soType);
            yield break;
        }

        if (!Directory.Exists($"Assets/07.ScriptableObjects/{firstLine[0]}"))
        {
            Directory.CreateDirectory($"Assets/07.ScriptableObjects/{firstLine[0]}");
        }

        for (int i = 1; i < ver.Length; i++)
        {
            string[] hor = ver[i].Split('\t');

            string SO_PATH = $"Assets/07.ScriptableObjects/{firstLine[0]}/{hor[0]}.asset";

            if (File.Exists(SO_PATH))
            {
                object obj = AssetDatabase.LoadAssetAtPath(SO_PATH, typeof(SOParent));
                SOParent soObj = obj as SOParent;
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

    public void StartFileSOCreate(string[] ver, Type soType)
    {
        string[] top = ver[0].Split('\t');

        for (int i = 1; i < ver.Length; i++)
        {
            string[] hor = ver[i].Split('\t');
            string[] root = hor[0].Split('/');
            CreateRoot(root);
            string SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{hor[0]}/{hor[1]}.asset";

            if (File.Exists(SO_PATH))
            {
                object obj = AssetDatabase.LoadAssetAtPath(SO_PATH, typeof(FileSO));
                FileSO soObj = obj as FileSO;
                soObj.Setting(hor);
                SetChildren(hor, soObj);
            }
            else
            {
                if(hor[3] == "Directory")
                {
                    object obj = CreateInstance(typeof(DirectorySO));
                    DirectorySO soObj = obj as DirectorySO;
                    soObj.Setting(hor);
                    AssetDatabase.CreateAsset(soObj, SO_PATH);
                    SetChildren(hor, soObj);
                }
                else
                {
                    object obj = CreateInstance(soType);
                    FileSO soObj = obj as FileSO;
                    soObj.Setting(hor);
                    AssetDatabase.CreateAsset(soObj, $"Assets/07.ScriptableObjects/DirectorySO/{hor[0]}/{hor[1]}.asset");
                    SetChildren(hor, soObj);
                }
            }


        }
    }

    public void CreateRoot(string[] root)
    {
        string current = $"Assets/07.ScriptableObjects/DirectorySO";

        for(int i = 0; i < root.Length; i++)
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
        for(int i = 0; i < root.Length - 1; i++)
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
                Debug.Log(directory);
                Debug.Log(soObj);
                Debug.Log(directory.children);
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


