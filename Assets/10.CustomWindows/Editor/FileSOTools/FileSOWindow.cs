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
using UnityEditor.UIElements;
using System.Data;

public class FileSOWindow : EditorWindow
{
    const string URL = "https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv&range=2:1000&gid=1984911729";
    //https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv&gid=1984911729

    private IntegerField idField;
    private TextField nameField;
    private Label resultLabel;
    private Button translateBtn;

    [MenuItem("Tools/FileSOWindow")]
    public static void ShowWindow()
    {
        FileSOWindow win = GetWindow<FileSOWindow>();
    }

    private void OnEnable()
    {
        VisualTreeAsset xml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/10.CustomWindows/Editor/FileSOTools/FileSOTools.uxml");

        TemplateContainer tree = xml.CloneTree();
        rootVisualElement.Add(tree);

        idField = rootVisualElement.Q<IntegerField>("IDField");
        nameField = rootVisualElement.Q<TextField>("TextField");
        resultLabel = rootVisualElement.Q<Label>("FileNameText");
        translateBtn = rootVisualElement.Q<Button>("TranslateBtn");
        translateBtn.RegisterCallback<MouseUpEvent>(x => Click());
    }

    private void Click()
    {
        Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReadSheet(), new object[0]);

    }

    private IEnumerator ReadSheet()
    {
        UnityWebRequest www;


        www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();
        string add = www.downloadHandler.text;
        if (idField.value != 0)
        {
            SearchFileSO(idField.value, add);
        }

        else if (string.IsNullOrEmpty(nameField.value))
        {
            SearchFileSO(nameField.value, add);
        }
    }

    public void SearchFileSO(string name, string dataText)
    {
        string[] rows = dataText.Split('\n');

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            if (columns[1] == name)
            {
                Debug.Log(int.Parse(columns[0]));
                break;
            }
        }
    }

    public void SearchFileWithID(int id, string dataText)
    {
        string[] rows = dataText.Split('\n');

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            if (int.Parse(columns[0]) == id)
            {
                Debug.Log(columns[1]);
            }
        }
    }

    public void SearchFileSO(int id, string dataText)
    {
        string[] rows = dataText.Split('\n');
        for (int i = 0; i < rows.Length; i++)
        {
            if(i == 49)
            {
                int a = 0;
                a = 1;
                if(a == 0)
                {

                }
            }
            string[] columns = rows[i].Split('\t');
             
            List<int> childIdList = new List<int>();
            string[] children = columns[11].Split(',');
            foreach (string child in children)
            {
                string newChild = Regex.Replace(child, "[^0-9]", "");

                if (string.IsNullOrEmpty(newChild)) continue;

                if (int.Parse(child) == id)
                {
                    Debug.Log($"{columns[0]}_{columns[1]}");
                }
            }
        }

        Debug.Log("³¡");
    }
   
}


