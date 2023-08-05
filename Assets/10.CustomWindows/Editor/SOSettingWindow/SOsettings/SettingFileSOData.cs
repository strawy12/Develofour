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
    public void SettingFileSO(string dataText)
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
            Sprite iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath.Forder);
            string spritePath = columns[8];
            bool isAlarm;
            bool.TryParse(columns[10], out isAlarm);
            switch (type)
            {
                case EWindowType.ProfilerWindow:
                    iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath.Profiler);
                    break;
                case EWindowType.VideoPlayer:
                    iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath.VideoPlayer);
                    break;
                case EWindowType.Notepad:
                    iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath.NotePad);
                    break;
                case EWindowType.Browser:
                    iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath.Zoogle);
                    break;
                case EWindowType.MediaPlayer:
                    iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath.MediaPlayer);
                    break;
                case EWindowType.Installer:
                    iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(SpritePath.Installer);
                    break;
                default:
                    if (spritePath != "")
                        iconSprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);
                    break;
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
            file.parentID = parentID;
            file.iconSprite = iconSprite;
            if (bytes != 0)
            {
                file.propertyData.bytes = bytes;
                file.propertyData.madeDate = madeDate;
                file.propertyData.lastAccessDate = lastAccessDate;
                file.propertyData.lastFixDate = lastFixDate;
            }
            if (!(file.parentID == "" || string.IsNullOrEmpty(file.parentID)))
            {
                DirectorySO directory = (DirectorySO)(fileSOList.Find(x => x.ID == file.parentID));
                if (directory != null)
                {
                    fileSOList[i].parent = directory;
                    if (!directory.children.Contains(file))
                        directory.children.Add(file);
                }
                else
                {
                    parentNullFileList.Add(file);
                }
            }
            file.isAlarm = isAlarm;
            path = file.GetRealFileLocation();

            SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path.Remove(path.Length - 1)}_{fileName}.asset";
            SO_PATH = SO_PATH.Replace("\\", "/");


            if (columns.Length > 9)
            {
                if (columns[9] == "추가 파일")
                {
                    SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/AdditionalFile/{id}_{fileName}.asset";
                }
                else if (columns[9] == "디폴트 파일")
                {
                    SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/DefaultFile/{id}_{fileName}.asset";
                }
            }
            if(columns.Length > 11)
            {
                if(columns[11] == "WHITE")
                {
                    file.iconColor = Color.white;
                }
            }
            if (isCreate)
            {
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
            Debug.Log(parentNullFileList[i].parentID);
        }
        //이제 부모없는 파일들 폴더 위치 생성해주고 이동시켜주기
        for (int i = 0; i < parentNullFileList.Count; i++)
        {
            DirectorySO directory = (DirectorySO)(fileSOList.Find(x => x.ID == parentNullFileList[i].parentID));
            if (directory != null)
            {
                parentNullFileList[i].parent = directory;
                if (!directory.children.Contains(parentNullFileList[i]))
                    directory.children.Add(parentNullFileList[i]);
            }
            path = parentNullFileList[i].GetRealFileLocation();

            SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path.Remove(path.Length - 1)}_{parentNullFileList[i].fileName}.asset";
            SO_PATH = SO_PATH.Replace("\\", "/");
            Debug.Log(SO_PATH);
            CreateFolder(SO_PATH);
            AssetDatabase.MoveAsset(AssetDatabase.GetAssetPath(parentNullFileList[i]), SO_PATH);
            EditorUtility.SetDirty(parentNullFileList[i]);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

}
