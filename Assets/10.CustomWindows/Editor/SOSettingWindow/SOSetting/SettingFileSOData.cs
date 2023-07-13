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
    public void SettingFileSO(string dataText, string typeStr)
    {
        string[] rows = dataText.Split('\n');

        string[] guids = AssetDatabase.FindAssets("t:FileSO", null);
        List<FileSO> fileSOList = new List<FileSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            fileSOList.Add(AssetDatabase.LoadAssetAtPath<FileSO>(path));
        }
        List<FileSO> temp = fileSOList.ToList();

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            if (string.IsNullOrEmpty(columns[0])) continue;
            string id = columns[0];
            string fileName = columns[1];

            EWindowType type = (EWindowType)Enum.Parse(typeof(EWindowType), columns[2]);

            string parentID = columns[3];
            float bytes = 0;
            string madeDate = string.Empty; 
            string lastFixDate = string.Empty;
            string lastAccessDate = string.Empty;
            if (columns.Length > 4)
            {
                float.TryParse(columns[4], out bytes);
                madeDate = columns[5];
                lastFixDate = columns[6];
                lastAccessDate = columns[7];
            }

            FileSO file = fileSOList.Find(x => x.ID == id);
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

            file.ID = id;
            file.fileName = fileName;
            file.windowType = type;
            if(bytes != 0)
            {
                file.propertyData.bytes = bytes;
                file.propertyData.madeDate = madeDate;
                file.propertyData.lastAccessDate = lastAccessDate;
                file.propertyData.lastFixDate = lastFixDate;
            }

            Debug.Log(parentID);

            if(!string.IsNullOrEmpty(parentID))
            {
                Debug.Log(parentID);
                DirectorySO directory = (DirectorySO)(fileSOList.Find(x => x.ID == parentID));
                if (directory == null) { Debug.LogError("디렉토리가 없음"); }
                else { FileSO SO = file; Debug.Log(SO.GetType()); directory.children.Add(SO); }
            }

            string path = file.GetRealFileLocation();

            Debug.Log(path);
            string SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path.Remove(path.Length - 1)}.asset";

            Debug.Log(SO_PATH);
            if (isCreate)
            {
                if (File.Exists(SO_PATH))
                {
                    SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path}_{id}.asset";
                }

                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(file, SO_PATH);
            }
            else
            {
                SO_PATH = SO_PATH.Replace("\\", "/");

                CreateFolder(SO_PATH);
                if(columns.Length > 8)
                {
                    bool flag1 = !File.Exists(SO_PATH);
                    bool flag2 = !(columns[8] == "추가 파일" || columns[8] == "디폴트 파일");

                    if (flag1 && flag2)
                    {
                        string oldPath = AssetDatabase.GetAssetPath(file.GetInstanceID());
                        AssetDatabase.MoveAsset(oldPath, SO_PATH);
                    }
                }
            }

            EditorUtility.SetDirty(file);
            temp.Remove(file);
        }

        temp.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

}
