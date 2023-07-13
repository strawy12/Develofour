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
        string path;
        string SO_PATH;
        string[] rows = dataText.Split('\n');

        List<FileSO> fileSOList = GuidsToSOList<FileSO>("t:FileSO");
        List<FileSO> temp = fileSOList.ToList();
        List<FileSO> parentNullFileList = new List<FileSO>();
        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            if (string.IsNullOrEmpty(columns[0])) continue;
            string id = columns[0].Trim();
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
            file.name = id;
            file.fileName = fileName;
            file.windowType = type;
            file.parentName = parentID;

            if (bytes != 0)
            {
                file.propertyData.bytes = bytes;
                file.propertyData.madeDate = madeDate;
                file.propertyData.lastAccessDate = lastAccessDate;
                file.propertyData.lastFixDate = lastFixDate;
            }
            if (!(file.parentName == "" || string.IsNullOrEmpty(file.parentName)))
            {
                DirectorySO directory = (DirectorySO)(fileSOList.Find(x => x.ID == file.parentName));
                if (directory != null)
                {
                    fileSOList[i].parent = directory;
                    directory.children.Add(file);
                }
                else
                {
                    parentNullFileList.Add(file);
                }
            }
            path = file.GetRealFileLocation();

            SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path.Remove(path.Length - 1)}_{fileName}.asset";
            SO_PATH = SO_PATH.Replace("\\", "/");

            if (columns.Length > 8)
            {
                if (columns[8] == "추가 파일")
                {
                    SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/AdditionalFile/{id}_{fileName}.asset";
                }
                else if (columns[8] == "디폴트 파일")
                {
                    SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/DefaultFile/{id}_{fileName}.asset";
                }
            }

            if (isCreate)
            {
                if (File.Exists(SO_PATH))
                {
                    SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path}_{id}_{fileName}.asset";
                }

                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(file, SO_PATH);
            }
            else
            {
                CreateFolder(SO_PATH);
                string oldPath = AssetDatabase.GetAssetPath(file.GetInstanceID());
                AssetDatabase.MoveAsset(oldPath, SO_PATH);
            }

            AssetDatabase.Refresh();
            EditorUtility.SetDirty(file);
            temp.Remove(file);
        }
        //이제 parent 넣어주자
        temp.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        fileSOList = GuidsToSOList<FileSO>("t:FileSO");

        for (int i = 0; i < parentNullFileList.Count; i++)
        {
            Debug.Log(parentNullFileList[i].parentName);
        }
        //이제 부모없는 파일들 폴더 위치 생성해주고 이동시켜주기
        for (int i = 0; i < parentNullFileList.Count; i++)
        {
            DirectorySO directory = (DirectorySO)(fileSOList.Find(x => x.ID == parentNullFileList[i].parentName));
            if (directory != null)
            {
                parentNullFileList[i].parent = directory;
                directory.children.Add(parentNullFileList[i]);
            }
            path = parentNullFileList[i].GetRealFileLocation();

            SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path.Remove(path.Length - 1)}.asset";
            SO_PATH = SO_PATH.Replace("\\", "/");
            Debug.Log(SO_PATH);
            CreateFolder(SO_PATH);
            AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(parentNullFileList[i]), SO_PATH);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

}
