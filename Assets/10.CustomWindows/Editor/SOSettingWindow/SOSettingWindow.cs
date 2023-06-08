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
        ProfilerInfo,
        ProfilerCategory,
        Monolog,
        ProfilerGuide,
        Mail
    }

    private Button settingButton;
    private Button fileSOBtn;
    private Button monologSOBtn;
    private Button profilerInfoBtn;
    private Button profilerGuideBtn;
    private Button mailBtn;
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
        profilerInfoBtn = rootVisualElement.Q<Button>("ProfilerInfoBtn");
        profilerGuideBtn = rootVisualElement.Q<Button>("ProfilerGuideBtn");
        gidField = rootVisualElement.Q<TextField>("GidField");
        mailBtn = rootVisualElement.Q<Button>("MailBtn");
        soTypeField = rootVisualElement.Q<TextField>("SOTypeField");

        profilerGuideBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerGuide));
        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
        profilerInfoBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerInfo));
        fileSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.File));
        mailBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.Mail));

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
            case ESOType.ProfilerInfo:
                gidField.value = "1539170501";
                soTypeField.value = "ProfilerInfoTextDataSO";
                break;
            case ESOType.ProfilerCategory:
                gidField.value = "1328616179";
                soTypeField.value = "ProfilerCategoryDataSO";
                break;
            case ESOType.ProfilerGuide:
                gidField.value = "77751767";
                soTypeField.value = "ProfilerGuideDataSO";
                break;
            case ESOType.Mail:
                gidField.value = "2109502413";
                soTypeField.value = "MailDataSO";
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
            case "ProfilerCategoryDataSO":
                SettingInfoCategorySO(add);
                break;
            case "ProfilerInfoTextDataSO":
                SettingInfoSO(add);
                break;
            case "ProfilerGuideDataSO":
                SettingProfilerGuideSO(add);
                break;
            case "MailDataSO":
                SettingMailSO(add);
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
        string[] guids = AssetDatabase.FindAssets("t:ProfilerInfoTextDataSO", null);
        List<ProfilerInfoTextDataSO> infoSODatas = new List<ProfilerInfoTextDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            infoSODatas.Add(AssetDatabase.LoadAssetAtPath<ProfilerInfoTextDataSO>(path));
        }
        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            int id = int.Parse(columns[0]);

            if (columns[0] == string.Empty || columns[1] == string.Empty || columns[2] == string.Empty) continue;

            EProfilerCategory category = Enum.Parse<EProfilerCategory>(columns[2]);
            string infoText = columns[3];
            string noticeText = columns[4];


            ProfilerInfoTextDataSO infoData = infoSODatas.Find(x => x.id == id);

            bool isCreate = false;

            if (infoData == null)
            {
                infoData = CreateInstance<ProfilerInfoTextDataSO>();
                isCreate = true;
            }

            infoData.id = id;
            infoData.category = category;
            infoData.infomationText = infoText;
            infoData.noticeText = noticeText;


            string SO_PATH = $"Assets/07.ScriptableObjects/Profiler/ProfilerInfoData/InfoTextData/{category}/{columns[5].Trim()}.asset";

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

        AutoComplete(ESOType.ProfilerCategory);
        Unity.EditorCoroutines.Editor.EditorCoroutineUtility.StartCoroutine(ReadSheet(), new object[0]);
    }

    private void SettingInfoCategorySO(string dataText)
    {
        Debug.Log("start category");

        string[] rows = dataText.Split('\n');
        string[] guids = AssetDatabase.FindAssets("t:ProfilerCategoryDataSO", null);
        List<ProfilerCategoryDataSO> categorySODatas = new List<ProfilerCategoryDataSO>();

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            categorySODatas.Add(AssetDatabase.LoadAssetAtPath<ProfilerCategoryDataSO>(path));
        }
        string[] infoDataGuids = AssetDatabase.FindAssets("t:ProfilerInfoTextDataSO", null);
        List<ProfilerInfoTextDataSO> infoSODatas = new List<ProfilerInfoTextDataSO>();

        foreach (string guid in infoDataGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            infoSODatas.Add(AssetDatabase.LoadAssetAtPath<ProfilerInfoTextDataSO>(path));
        }

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            EProfilerCategory category = Enum.Parse<EProfilerCategory>(columns[0]);
            EProfilerCategoryType categoryType = Enum.Parse<EProfilerCategoryType>(columns[1]);
            string categoryName = columns[2];
            List<ProfilerInfoTextDataSO> infoTextDataList = new List<ProfilerInfoTextDataSO>();
            ProfilerInfoTextDataSO defaultText = null;

            if (columns[3] != "")
            {
                int[] infoTextIDs = Array.ConvertAll(columns[3].Split(','), x => int.Parse(x));

                foreach (int infoID in infoTextIDs)
                {
                    ProfilerInfoTextDataSO infoData = infoSODatas.Find(x => x.id == infoID);
                    if (infoData != null)
                    {
                        infoTextDataList.Add(infoData);
                    }
                }
            }

            columns[4] = Regex.Replace(columns[4], "[^0-9]", "");
            if (columns[4] != "")
            {
                ProfilerInfoTextDataSO infoData = infoSODatas.Find(x => x.id == int.Parse(columns[4]));
                defaultText = infoData;
            }

            bool isCreate = false;
            ProfilerCategoryDataSO categoryData = categorySODatas.Find(x => x.category == category);

            if (categoryData == null)
            {
                categoryData = CreateInstance<ProfilerCategoryDataSO>();
                isCreate = true;
            }

            categoryData.category = category;
            categoryData.categoryType = categoryType;
            categoryData.categoryName = categoryName;
            categoryData.infoTextList = infoTextDataList;
            categoryData.defaultInfoText = defaultText;

            string SO_PATH = $"Assets/07.ScriptableObjects/Profiler/ProfilerInfoData/InfoCategoryData/{category}.asset";

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

    private void SettingProfilerGuideSO(string dataText)
    {
        string[] rows = dataText.Split('\n');
        string[] guids = AssetDatabase.FindAssets("t:ProfilerGuideDataSO", null);

        List<ProfilerGuideDataSO> guideSODatas = new List<ProfilerGuideDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            guideSODatas.Add(AssetDatabase.LoadAssetAtPath<ProfilerGuideDataSO>(path));
        }
        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            string guideName = columns[0];
            string fileName = columns[1];
            bool addTutorial = columns[2] == "TURE";
            List<string> textList = columns[3].Split('#').ToList();


            ProfilerGuideDataSO guideData = guideSODatas.Find(x => x.name == fileName);
            bool isCreate = false;

            if (guideData == null)
            {
                Debug.Log("isCreate");
                guideData = CreateInstance<ProfilerGuideDataSO>();
                isCreate = true;
            }

            guideData.guideName = guideName;
            guideData.guideTextList = textList;
            guideData.isAddTutorial = addTutorial;
            string SO_PATH = $"Assets/07.ScriptableObjects/Profiler/ProfilerGuideData/{columns[1]}.asset";

            if (isCreate)
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

    private void SettingMailSO(string dataText)
    {
        string[] rows = dataText.Split('\n');
        string[] guids = AssetDatabase.FindAssets("t:MailDataSO", null);

        List<MailDataSO> mailSODatas = new List<MailDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            mailSODatas.Add(AssetDatabase.LoadAssetAtPath<MailDataSO>(path));
        }

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');
            int mailID = int.Parse(columns[0]);
            int categoryInt = 0;
            string mailTitle = columns[1];
            switch (columns[2])
            {
                default:
                case "Receive":
                    categoryInt = 0x01;
                    break;
                case "Send":
                    categoryInt = 0x04;
                    break;
            }
            string sendName = columns[3];
            string[] sendDate = columns[4].Trim().Split(',');

            int year = int.Parse(sendDate[0]);
            int month = int.Parse(sendDate[1]);
            int day = int.Parse(sendDate[2]);
            int hour = int.Parse(sendDate[3]);
            int minute = int.Parse(sendDate[4]);

            string receiveName = columns[5];
            string informationText = columns[6];
            if (columns[7] == "TRUE")
            {
                categoryInt += 0x02;
            }
            if (columns[8] == "TRUE")
            {
                categoryInt += 0x08;
            }
            if (columns[9] == "FALSE")
            {
                categoryInt += 0x10;
            }

            MailDataSO mailData = mailSODatas.Find(x => x.mailID == mailID);
            bool isCreate = false;
            if (mailData == null)
            {
                mailData = CreateInstance<MailDataSO>();
                isCreate = true;
            }
            mailData.mailID = mailID;
            mailData.mailCategory = categoryInt;
            mailData.titleText = mailTitle;
            mailData.sendName = sendName;
            mailData.receiveName = receiveName;
            mailData.dateData = new Vector3Int(year, month, day);
            mailData.timeData = new Vector2Int(hour, minute);
            mailData.informationText = informationText;

            string SO_PATH = $"Assets/07.ScriptableObjects/MailData/{columns[10]}.asset";
            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(mailData, SO_PATH);
            }
            EditorUtility.SetDirty(mailData);
            mailSODatas.Remove(mailData);
        }
        mailSODatas.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

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


