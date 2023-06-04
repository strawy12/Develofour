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
using Unity.VisualScripting;

public class SOSettingWindow : EditorWindow
{
    const string URL = @"https://docs.google.com/spreadsheets/d/1yrZPGjn1Vw5-YiqKahh6nIVdxDFNO0lo86dslqTVb6Q/export?format=tsv&range=2:1000&gid={0}";

    private enum ESOType
    {
        None,
        File,
        ProfileInfo,
        ProfileCategory,
        Monolog,
        ProfileGuide,
    }

    private Button settingButton;
    private Button fileSOBtn;
    private Button monologSOBtn;
    private Button profileInfoBtn;
    private Button profileGuideBtn;

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
        profileInfoBtn = rootVisualElement.Q<Button>("ProfileInfoBtn");
        profileGuideBtn = rootVisualElement.Q<Button>("ProfileGuideBtn");
        gidField = rootVisualElement.Q<TextField>("GidField");
        soTypeField = rootVisualElement.Q<TextField>("SOTypeField");
        profileGuideBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfileGuide));
        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
        profileInfoBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfileInfo));
        fileSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.File));
        monologSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.Monolog));
    }



    private void AutoComplete(ESOType type)
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
            case ESOType.ProfileInfo:
                gidField.value = "1539170501";
                soTypeField.value = "ProfileInfoTextDataSO";
                break;
            case ESOType.ProfileCategory:
                gidField.value = "1328616179";
                soTypeField.value = "ProfileCategoryDataSO";
                break;
            case ESOType.ProfileGuide:
                gidField.value = "77751767";
                soTypeField.value = "ProfileGuideDataSO";
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
            case "ProfileCategoryDataSO":
                SettingInfoCategorySO(add);
                break;
            case "ProfileInfoTextDataSO":
                SettingInfoSO(add);
                break;
            case "ProfileGuideDataSO":
                SettingProfileGuideSO(add);
                break;
        }

    }

    public void SettingMonologSO(string dataText)
    {

        List<string> monologKeyList = new List<string>();

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

            if (columns[0] == string.Empty || columns[1] == string.Empty || columns[3] == string.Empty) continue;

            int id = int.Parse(columns[0]);
            string fileName = columns[1];
            string monologName = columns[2];
            Color characterColor = Color.white;
            Color character2Color = Color.white;
            UnityEngine.ColorUtility.TryParseHtmlString('#' + columns[4], out characterColor);
            UnityEngine.ColorUtility.TryParseHtmlString('#' + columns[5], out character2Color);
            Debug.Log(characterColor);
            MonologTextDataSO monologData = monologSOList.Find(x => x.TextDataType == id);
            bool isCreate = false;

            if (monologData == null)
            {
                monologData = CreateInstance<MonologTextDataSO>();
                isCreate = true;
            }

            monologData.TextDataType = id;
            monologData.monologName = monologName;

            string[] textDataList = columns[3].Split('#');

            if (monologData.textDataList == null)
            {
                monologData.textDataList = new List<TextData>();
            }

            for (int j = 0; j < textDataList.Length; j++)
            {
                TextData data = new TextData() { text = textDataList[j] };

                if (textDataList[j].Contains("-"))
                {
                    data.textColor = characterColor;
                    data.text =  data.text.Replace("-", "");
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

            string variableName = fileName.ToUpper();
            Debug.Log(id);
            int monologIdx = variableName.IndexOf("MONOLOG");

            if (monologIdx != -1)
            {
                variableName = variableName.Remove(monologIdx, 7);
            }

            monologKeyList.Add($"        public const int {variableName} = {id};");

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

        //const string scriptPath = "Assets/02.Scripts/Utils/Constant.cs";
        //string temp = "";

        //using (StreamReader sr = new StreamReader(scriptPath))
        //{
        //    string line = "";
        //    while (!line.Contains("public static class MonologKey"))
        //    {
        //        line = sr.ReadLine();
        //        temp += line + '\n';
        //    }

        //    temp += sr.ReadLine() + '\n';

        //    int idx = 0;
        //    while (idx < monologKeyList.Count)
        //    {
        //        line = monologKeyList[idx];
        //        temp += line + '\n';
        //        idx++;
        //    }

        //    temp += "    }\n    #endregion\n}";

        //    sr.Close();
        //}

        //using (StreamWriter writer = new StreamWriter(scriptPath))
        //{
        //    writer.Write(temp);
        //    writer.Flush();
        //}

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public void SettingFileSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        string[] guids = AssetDatabase.FindAssets("t:FileSO", null);
        List<FileSO> fileSOList = new List<FileSO>();
        List<FileSO> temp = fileSOList.ToList();
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
            string pinString = columns[5];
            string pinHint = columns[6];
            float bytes = 0;
            float.TryParse(columns[10], out bytes);
            string madeDate = columns[11];
            string lastFixDate = columns[12];
            string lastAccessDate = columns[13];
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

                isCreate = true;
            }

            string[] pins = pinString.Split(',');
            List<string> pinList = new List<string>();
            foreach (string pin in pins)
            {
                if (string.IsNullOrEmpty(pin)) continue;
                pinList.Add(pin);
            }

            file.id = id;
            file.fileName = fileName;
            file.windowType = type;
            file.isFileLock = isFileLock;
            file.windowPinHintGuide = pinHint;
            file.windowPin = pinList;
            file.name = columns[9];
            file.tags = tags;
            file.propertyData.bytes = bytes;
            file.propertyData.madeDate = madeDate;
            file.propertyData.lastAccessDate = lastAccessDate;
            file.propertyData.lastFixDate = lastFixDate;

            if (file is DirectorySO)
            {
                DirectorySO directory = (DirectorySO)file;
                directory.children.Clear();
                foreach (int childID in childIdList)
                {
                    FileSO child = fileSOList.Find(x => x.id == childID);
                    if (child == null) continue;
                    Debug.Log($"{child}_{childID}");
                    child.parent = directory;
                    directory.children.Add(child);
                }
            }

            string path = file.GetRealFileLocation();
            string SO_PATH = $"Assets/07.ScriptableObjects/DirectorySO/{path.Remove(path.Length - 1)}.asset";

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
                bool flag1 = !File.Exists(SO_PATH);
                bool flag2 = !(columns[10] == "추가 파일" || columns[10] == "디폴트 파일");

                if (flag1 && flag2)
                {
                    string oldPath = AssetDatabase.GetAssetPath(file.GetInstanceID());
                    AssetDatabase.MoveAsset(oldPath, SO_PATH);
                }
            }

            EditorUtility.SetDirty(file);
            temp.Remove(file);
        }

        temp.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();

    }

    private void SettingInfoSO(string dataText)
    {

        string[] rows = dataText.Split('\n');
        string[] guids = AssetDatabase.FindAssets("t:ProfileInfoTextDataSO", null);
        List<ProfileInfoTextDataSO> infoSODatas = new List<ProfileInfoTextDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            infoSODatas.Add(AssetDatabase.LoadAssetAtPath<ProfileInfoTextDataSO>(path));
        }
        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            int id = int.Parse(columns[0]);

            if (columns[0] == string.Empty || columns[1] == string.Empty || columns[2] == string.Empty) continue;

            EProfileCategory category = Enum.Parse<EProfileCategory>(columns[2]);
            string infoText = columns[3];
            string noticeText = columns[4];


            ProfileInfoTextDataSO infoData = infoSODatas.Find(x => x.id == id);

            bool isCreate = false;

            if (infoData == null)
            {
                infoData = CreateInstance<ProfileInfoTextDataSO>();
                isCreate = true;
            }

            infoData.id = id;
            infoData.category = category;
            infoData.infomationText = infoText;
            infoData.noticeText = noticeText;


            string SO_PATH = $"Assets/07.ScriptableObjects/Profile/ProfileInfoData/InfoTextData/{category}/{columns[5].Trim()}.asset";

            if (isCreate)
            {
                Debug.Log(SO_PATH);
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(infoData, SO_PATH);
            }

            EditorUtility.SetDirty(infoData);
            infoSODatas.Remove(infoData);
        }

        infoSODatas.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));
        AssetDatabase.Refresh();

        AutoComplete(ESOType.ProfileCategory);
        Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReadSheet(), new object[0]);
    }

    private void SettingInfoCategorySO(string dataText)
    {
        Debug.Log("start category");

        string[] rows = dataText.Split('\n');
        string[] guids = AssetDatabase.FindAssets("t:ProfileCategoryDataSO", null);
        List<ProfileCategoryDataSO> categorySODatas = new List<ProfileCategoryDataSO>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            categorySODatas.Add(AssetDatabase.LoadAssetAtPath<ProfileCategoryDataSO>(path));
        }
        string[] infoDataGuids = AssetDatabase.FindAssets("t:ProfileInfoTextDataSO", null);
        List<ProfileInfoTextDataSO> infoSODatas = new List<ProfileInfoTextDataSO>();

        foreach (string guid in infoDataGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            infoSODatas.Add(AssetDatabase.LoadAssetAtPath<ProfileInfoTextDataSO>(path));
        }

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            EProfileCategory category = Enum.Parse<EProfileCategory>(columns[0]);
            EProfileCategoryType categoryType = Enum.Parse<EProfileCategoryType>(columns[1]);
            string categoryName = columns[2];
            List<ProfileInfoTextDataSO> infoTextDataList = new List<ProfileInfoTextDataSO>();
            ProfileInfoTextDataSO defaultText = null;

            if (columns[3] != "")
            {
                int[] infoTextIDs = Array.ConvertAll(columns[3].Split(','), x => int.Parse(x));

                foreach (int infoID in infoTextIDs)
                {
                    ProfileInfoTextDataSO infoData = infoSODatas.Find(x => x.id == infoID);
                    if (infoData != null)
                    {
                        infoTextDataList.Add(infoData);
                    }
                }
            }

            columns[4] = Regex.Replace(columns[4], "[^0-9]", "");
            if (columns[4] != "")
            {
                ProfileInfoTextDataSO infoData = infoSODatas.Find(x => x.id == int.Parse(columns[4]));
                defaultText = infoData;
            }

            bool isCreate = false;
            ProfileCategoryDataSO categoryData = categorySODatas.Find(x => x.category == category);

            if (categoryData == null)
            {
                categoryData = CreateInstance<ProfileCategoryDataSO>();
                isCreate = true;
            }

            categoryData.category = category;
            categoryData.categoryType = categoryType;
            categoryData.categoryName = categoryName;
            categoryData.infoTextList = infoTextDataList;
            categoryData.defaultInfoText = defaultText;

            string SO_PATH = $"Assets/07.ScriptableObjects/Profile/ProfileInfoData/InfoCategoryData/{category}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(categoryData, SO_PATH);
            }

            EditorUtility.SetDirty(categoryData);
            categorySODatas.Remove(categoryData);
        }
        categorySODatas.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    private void SettingProfileGuideSO(string dataText)
    {
        string[] rows = dataText.Split('\n');
        string[] guids = AssetDatabase.FindAssets("t:ProfileGuideDataSO", null);

        List<ProfileGuideDataSO> guideSODatas = new List<ProfileGuideDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            guideSODatas.Add(AssetDatabase.LoadAssetAtPath<ProfileGuideDataSO>(path));
        }
        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            string guideName = columns[0];
            string fileName = columns[1];
            bool addTutorial = columns[2] == "TURE";
            List<string> textList = columns[3].Split('#').ToList();


            ProfileGuideDataSO guideData = guideSODatas.Find(x => x.name == fileName);
            bool isCreate = false;

            if (guideData == null)
            {
                Debug.Log("isCreate");
                guideData = CreateInstance<ProfileGuideDataSO>();
                isCreate = true;
            }

            guideData.guideName = guideName;
            guideData.guideTextList = textList;
            guideData.isAddTutorial = addTutorial;
            string SO_PATH = $"Assets/07.ScriptableObjects/Profile/ProfileGuideData/{columns[1]}.asset";

            if(isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(guideData, SO_PATH);
            }

            EditorUtility.SetDirty(guideData);
            guideSODatas.Remove(guideData);

        }
        guideSODatas.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

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


