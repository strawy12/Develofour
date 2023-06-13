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
using static MonologLockDecision;

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
        Mail,
        TriggerPrefab,
        WindowLockData,
    }
    private class BodyPrefabData
    {
        public GameObject bodyObject;
        public string prefabPath;
    }
    private Button settingButton;
    private Button fileSOBtn;
    private Button monologSOBtn;
    private Button profilerInfoBtn;
    private Button profilerGuideBtn;
    private Button mailBtn;
    private Button triggerBtn;
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
        mailBtn = rootVisualElement.Q<Button>("MailBtn");
        rootVisualElement.Q<Button>("FileLockBtn").RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.WindowLockData));
        gidField = rootVisualElement.Q<TextField>("GidField");
        soTypeField = rootVisualElement.Q<TextField>("SOTypeField");
        triggerBtn = rootVisualElement.Q<Button>("TriggerPrfBtn");

        settingButton.RegisterCallback<MouseUpEvent>(x => Setting());
        profilerGuideBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerGuide));
        profilerInfoBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.ProfilerInfo));
        fileSOBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.File));
        mailBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.Mail));
        triggerBtn.RegisterCallback<MouseUpEvent>(x => AutoComplete(ESOType.TriggerPrefab));
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
            case ESOType.TriggerPrefab:
                gidField.value = "1321675607";
                soTypeField.value = "Trigger";
                break;
            case ESOType.WindowLockData:
                gidField.value = "33006268";
                soTypeField.value = "WindowLockDataSO";
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
        string add = www.downloadHandler.text.Replace("\r","");

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
            case "Trigger":
                SettingInfoTrigger(add);
                break;
            case "WindowLockDataSO":
                SettingFileLockSO(add);
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
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            fileSOList.Add(AssetDatabase.LoadAssetAtPath<FileSO>(path));
        }
        List<FileSO> temp = fileSOList.ToList();

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

    private void SettingInfoTrigger(string dataText)
    {
        string[] rows = dataText.Split('\n');

        string[] notepadGuids = AssetDatabase.FindAssets("t:NotepadDataSO", null);

        Dictionary<int, ClickInfoTrigger> infoTriggerList = new Dictionary<int, ClickInfoTrigger>();
        Dictionary<int, BodyPrefabData> notePadBodys = new Dictionary<int, BodyPrefabData>();
        List<ClickInfoTrigger> removeTriggerList = new List<ClickInfoTrigger>();
        foreach (var guid in notepadGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            NotepadDataSO notepadDataSO = AssetDatabase.LoadAssetAtPath<NotepadDataSO>(path);
            if (notepadDataSO.notepadBody != null)
                notepadDataSO.notepadBody.ClearTextTrigger();
            if (notepadDataSO.notepadBody == null) continue;

            GameObject soBodyPrefab = Instantiate(notepadDataSO.notepadBody.gameObject);
            List<ClickInfoTrigger> bodyTriggerList = soBodyPrefab.GetComponentsInChildren<ClickInfoTrigger>().ToList();

            foreach (var trigger in bodyTriggerList)
            {
                if (trigger.triggerID == 0)
                {
                    removeTriggerList.Add(trigger);
                }
                else
                {
                    infoTriggerList.Add(trigger.triggerID, trigger);
                }
            }

            BodyPrefabData bodyData = new BodyPrefabData() { bodyObject = soBodyPrefab, prefabPath = AssetDatabase.GetAssetPath(notepadDataSO.notepadBody) };
            notePadBodys.Add(notepadDataSO.fileId, bodyData);
        }

        string[] mediaplayerGuids = AssetDatabase.FindAssets("t:MediaPlayerDataSO", null);
        Dictionary<int, BodyPrefabData> mediaplayerBodys = new Dictionary<int, BodyPrefabData>();

        foreach (var guid in mediaplayerGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            MediaPlayerDataSO mediaPlayerDataSO = AssetDatabase.LoadAssetAtPath<MediaPlayerDataSO>(path);
            if (mediaPlayerDataSO.body != null)
                mediaPlayerDataSO.body.ClearTextTrigger();
            if (mediaPlayerDataSO.body == null) continue;

            GameObject soBodyPrefab = Instantiate(mediaPlayerDataSO.body.gameObject);

            List<ClickInfoTrigger> bodyTriggerList = soBodyPrefab.GetComponentsInChildren<ClickInfoTrigger>().ToList();
            foreach (var trigger in bodyTriggerList)
            {
                if (trigger.triggerID == 0)
                {
                    removeTriggerList.Add(trigger);
                }
                else
                {
                    infoTriggerList.Add(trigger.triggerID, trigger);
                }
            }
            BodyPrefabData bodyData = new BodyPrefabData() { bodyObject = soBodyPrefab, prefabPath = AssetDatabase.GetAssetPath(mediaPlayerDataSO.body) };
            mediaplayerBodys.Add(mediaPlayerDataSO.fileId, bodyData);
        }
        string[] imageviewerGuids = AssetDatabase.FindAssets("t:ImageViewerDataSO", null);
        Dictionary<int, BodyPrefabData> imageviewerBodys = new Dictionary<int, BodyPrefabData>();

        foreach (var guid in imageviewerGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            ImageViewerDataSO imageViewerDataSO = AssetDatabase.LoadAssetAtPath<ImageViewerDataSO>(path);
            if (imageViewerDataSO.imageBody == null) continue;
            GameObject soBodyPrefab = Instantiate(imageViewerDataSO.imageBody.gameObject);
            List<ClickInfoTrigger> bodyTriggerList = soBodyPrefab.GetComponentsInChildren<ClickInfoTrigger>().ToList();
            foreach (var trigger in bodyTriggerList)
            {
                if (trigger.triggerID == 0)
                {
                    removeTriggerList.Add(trigger);
                }
                else
                {
                    infoTriggerList.Add(trigger.triggerID, trigger);
                }
            }
            BodyPrefabData bodyData = new BodyPrefabData() { bodyObject = soBodyPrefab, prefabPath = AssetDatabase.GetAssetPath(imageViewerDataSO.imageBody) };
            imageviewerBodys.Add(imageViewerDataSO.fileId, bodyData);
        }

        while (removeTriggerList.Count != 0)
        {
            ClickInfoTrigger temp = removeTriggerList[0];
            removeTriggerList.RemoveAt(0);
            DestroyImmediate(temp.gameObject, true);
        }

        ClickInfoTrigger infoTriggerPrefab = AssetDatabase.LoadAssetAtPath<ClickInfoTrigger>("Assets/03.Prefabs/InfoTrigger/InfoDefault.prefab");

        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            int triggerID = int.Parse(columns[0]);
            int fileID = int.Parse(columns[1]);
            EWindowType fileType = Enum.Parse<EWindowType>(columns[2]);

            BodyPrefabData bodyPrefabData = null;
            switch (fileType)
            {
                case EWindowType.Notepad:
                    bodyPrefabData = notePadBodys[fileID];
                    break;
                case EWindowType.MediaPlayer:
                    bodyPrefabData = mediaplayerBodys[fileID];
                    break;
                case EWindowType.ImageViewer:
                    bodyPrefabData = imageviewerBodys[fileID];
                    break;
            }

            List<int> infoList = new List<int>();
            List<string> infoStringList = columns[3].Trim().Split(',').ToList();
            foreach (var infoString in infoStringList)
            {
                int infoId = 0;
                if (!int.TryParse(infoString, out infoId))
                {
                    continue;
                }
                infoList.Add(infoId);
            }
            int monologID = int.Parse(columns[4]);
            List<NeedInfoData> needInfoDataList = new List<NeedInfoData>();
            string[] needInfoDataStrings = columns[5].Split(',');
            foreach (var needInfo in needInfoDataStrings)
            {
                string[] division = needInfo.Trim().Split('/');
                int infoID = 0;
                if (!int.TryParse(division[0], out infoID))
                {
                    continue;
                }
                int needMonologID = int.Parse(division[1]);
                bool getInfo = division[2] == "TRUE";

                NeedInfoData needInfoData = new NeedInfoData() { needInfoID = infoID, getInfo = getInfo, monologID = needMonologID };

                needInfoDataList.Add(needInfoData);
            }
            int completeMonologID = int.Parse(columns[6]);
            bool isFakeInfo = columns[7] == "TRUE";
            float delay = float.Parse(columns[8].Trim());
            ClickInfoTrigger infoTrigger = null;
            if (infoTriggerList.ContainsKey(triggerID))
            {
                infoTrigger = infoTriggerList[triggerID];
            }
            else
            {
                if(fileType == EWindowType.Notepad || fileType == EWindowType.MediaPlayer)
                {
                    infoTrigger = Instantiate(infoTriggerPrefab, bodyPrefabData.bodyObject.GetComponentInChildren<TMPro.TMP_Text>().transform);
                }
                else
                {
                    infoTrigger = Instantiate(infoTriggerPrefab, bodyPrefabData.bodyObject.transform);
                }
            }
            infoTrigger.triggerID = triggerID;
            infoTrigger.fileID = fileID;
            infoTrigger.MonologID = monologID;
            infoTrigger.infoDataIDList = infoList;
            infoTrigger.completeMonologType = completeMonologID;
            infoTrigger.delay = delay;
            infoTrigger.needInfoList = needInfoDataList;
            infoTrigger.isFakeInfo = isFakeInfo;

            string infoName = "";
            for (int j = 0; j < infoList.Count; j++)
            {
                if (j != 0)
                    infoName += ',';
                infoName += infoList[j];
            }
            infoTrigger.name = $"info{infoName}_{monologID}_{completeMonologID}";

            if (fileType == EWindowType.Notepad)
            {
                NotepadBody notepadBody = bodyPrefabData.bodyObject.GetComponent<NotepadBody>();

                string text = columns[9];
                int textIdx = 0;
                if (int.TryParse(text, out textIdx))
                {
                    notepadBody.AddTextTriggerData(new TextTriggerData() { id = textIdx, trigger = infoTrigger });
                }
                else
                {
                    notepadBody.AddTextTriggerData(new TextTriggerData() { text = text, trigger = infoTrigger });
                    notepadBody.SetTriggerText();
                }

            }

            if (fileType == EWindowType.MediaPlayer)
            {
                MediaPlayerBody mediaPlayerBody = bodyPrefabData.bodyObject.GetComponent<MediaPlayerBody>();
                string text = columns[9];
                int textIdx = 0;
                if (int.TryParse(text, out textIdx))
                {
                    mediaPlayerBody.AddTextTriggerData(new TextTriggerData() { id = textIdx, trigger = infoTrigger });
                }
                else
                {
                    mediaPlayerBody.AddTextTriggerData(new TextTriggerData() { text = text, trigger = infoTrigger });
                    mediaPlayerBody.SetTriggerText();
                }
            }

            PrefabUtility.SaveAsPrefabAsset(bodyPrefabData.bodyObject, bodyPrefabData.prefabPath);
        }

        AssetDatabase.Refresh();
        AssetDatabase.SaveAssets();
    }

    public void SettingFileLockSO(string dataText)
    {
        string[] rows = dataText.Split('\n');

        string[] guids = AssetDatabase.FindAssets("t:WindowLockDataSO", null);
        List<WindowLockDataSO> fileLockSOList = new List<WindowLockDataSO>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            fileLockSOList.Add(AssetDatabase.LoadAssetAtPath<WindowLockDataSO>(path));
        }
        for (int i = 0; i < rows.Length; i++)
        {
            string[] columns = rows[i].Split('\t');

            int id = int.Parse(columns[0]);
            string windowPin = columns[1];
            string windowPinHint = columns[2];

            AutoAnswerData data = new AutoAnswerData();

            data.answer = columns[1].Split(',')[0];
            data.infoData = new List<MonologLockDecision>();

            for(int j = 4; j < columns.Length; j++)
            {
                if (string.IsNullOrEmpty(columns[j])) continue;
                
                string[] decesions = columns[j].Split("_");
                EDecisionType type = (EDecisionType)Enum.Parse(typeof(EDecisionType), decesions[0]);
                int key = int.Parse(decesions[1]);
                data.infoData.Add(new MonologLockDecision { decisionType=type, key=key });
            }


            bool isCreate = false;

            WindowLockDataSO lockdata = fileLockSOList.Find(x => x.fileId == id);

            if (lockdata == null)
            {
                lockdata = CreateInstance<WindowLockDataSO>();
                isCreate = true;
            }

            lockdata.fileId = id;
            lockdata.windowPin = windowPin;
            lockdata.windowPinHintGuide = windowPinHint;
            lockdata.answerData = data;

            string SO_PATH = $"Assets/07.ScriptableObjects/WindowLockData/{columns[3]}.asset";

            if (isCreate)
            {
                CreateFolder(SO_PATH);
                AssetDatabase.CreateAsset(lockdata, SO_PATH);
            }

            EditorUtility.SetDirty(lockdata);
            fileLockSOList.Remove(lockdata);
        }

        fileLockSOList.ForEach(x => AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(x.GetInstanceID())));

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


