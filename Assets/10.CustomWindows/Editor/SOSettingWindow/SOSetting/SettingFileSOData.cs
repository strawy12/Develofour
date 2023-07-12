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

    public void SettingMonologSO(string dataText, string type)
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

            if (columns[0] == string.Empty || columns[1] == string.Empty) continue;


            string id = columns[0];

            Color characterColor = Color.white;
            Color character2Color = Color.white;
            UnityEngine.ColorUtility.TryParseHtmlString('#' + columns[4], out characterColor);
            UnityEngine.ColorUtility.TryParseHtmlString('#' + columns[5], out character2Color);

            MonologTextDataSO monologData = monologSOList.Find(x => x.ID == id);
            bool isCreate = false;

            if (monologData == null)
            {
                monologData = CreateInstance<MonologTextDataSO>();
                isCreate = true;
            }


            string[] textDataList = columns[3].Split('#');

            monologData.textDataList = new List<TextData>();

            for (int j = 0; j < textDataList.Length; j++)
            {
                TextData data = new TextData() { text = textDataList[j] };

                if (textDataList[j].Contains("-"))
                {
                    data.textColor = characterColor;
                    data.text = data.text.Replace("-", "");
                }
                else
                {
                    data.textColor = character2Color;
                }
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

            string path = AssetDatabase.GetAssetPath(monologData.GetInstanceID());
            string[] pathSplits = path.Split('/');
            pathSplits[pathSplits.Length - 1] = $"{fileName}.asset";
            string newPath = string.Join('/', pathSplits);

            if (path != newPath)
            {
                AssetDatabase.RenameAsset(path, newPath);
            }

            EditorUtility.SetDirty(monologData);
            monologSOList.Remove(monologData);
        }
        monologSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }
}
