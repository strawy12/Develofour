using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using static System.Net.Mime.MediaTypeNames;

public class FileManager : MonoSingleton<FileManager>
{
    //private class FileWeight
    //{
    //    public FileSO file;
    //    public float weight;
    //    public bool isCompleteWeightDirectory;
    //    public FileWeight(FileSO file, float weight)
    //    {
    //        this.file = file;
    //        this.weight = weight;
    //        isCompleteWeightDirectory = false;
    //    }
    //}

    [SerializeField]
    private DirectorySO rootDirectory;
    [SerializeField]
    private List<FileSO> additionFileList = new List<FileSO>();
    private List<FileSO> debugAdditionFileList = new List<FileSO>();
    //새롭게 추가된 파일은 fileList에 등록된다.
    [SerializeField]
    private List<FileSO> defaultFileList = new List<FileSO>();
    //[Header("Profiler FileSearch")]
    //private const float findNameScore = 30f;
    //private const float findTagScore = 20f;
    //private bool isSearchTag = false;
    //private bool isSearchByFileName = false;
    private Dictionary<EWindowType, FileSO> defaultFileDictionary = new Dictionary<EWindowType, FileSO>();
    private List<string> currentFileNameWord;
    [SerializeField]
    private BackgroundIcons backgroundIcons;
    //List<FileWeight> foundFileWeights;
    private void Awake()
    {
        //foundFileWeights = new List<FileWeight>();
        currentFileNameWord = new List<string>();
        foreach (FileSO file in defaultFileList)
        {
            defaultFileDictionary.Add(file.windowType, file);
        }
    }

    private void Start()
    {
        GameManager.Inst.OnGameStartCallback += Init;
    }


    private void Init()
    {
        foreach (var fileData in additionFileList)
        {
            if (DataManager.Inst.AdditionalFileContain(fileData))
            {
                string directoryID = DataManager.Inst.GetAdditionFileData(fileData.ID).directoryID;

                AddFile(fileData, directoryID, false);
            }
        }
    }

    public List<FileSO> GetAllAdditionalFile()
    {
        return additionFileList;
    }

    public FileSO GetDefaultFile(EWindowType windowType)
    {
        if (!defaultFileDictionary.ContainsKey(windowType))
        {
            Debug.Log("존재하지 않는 defaultfile 윈도우 입니다.");
            return null;
        }
        FileSO file = defaultFileDictionary[windowType];
        return file;
    }
    public FileSO GetFile(string key)
    {
        List<FileSO> allFileList = GetALLUnLockFileList(rootDirectory, true);
        allFileList.AddRange(defaultFileList);
        FileSO file = allFileList.Find(x => x.ID == key);
        if (file == null)
        {
            Debug.LogError($"{key}를 id로 가지고있는 파일이 존재하지않습니다.");
        }
        return file;
    }
    public FileSO GetAdditionalFile(string key)
    {
        FileSO file = additionFileList.Find((x) => x.ID == key);
        if (file == null)
        {
            Debug.LogError("추가파일이 Null입니다.");
        }
        return file;
    }

    public void AddFile(string file, string directoryID)
    {
        AddFile(GetAdditionalFile(file), directoryID);
    }
    public void AddFile(FileSO file, string directoryID, bool isNotice = true)
    {
        List<FileSO> fileList = GetALLFileList();
        // 이분탐색 만들 때 Define에다 만들든 함수를 따로 빼서 작업해야해
        DirectorySO directory = fileList.Find(x => x.ID == directoryID) as DirectorySO;

        if (!directory.children.Contains(file))
        {
            directory.children.Add(file);
            Debug.Log(file);
            Debug.Log(directory);
            file.parent = directory;
            debugAdditionFileList.Add(file);
        }

        if (!DataManager.Inst.AdditionalFileContain(file))
        {
            DataManager.Inst.AddNewFileData(file, directory);
        }
        EventManager.TriggerEvent(ELibraryEvent.AddFile);
        if (isNotice)
        {
            string str = $"{file.GetFileLocationToSlash().TrimEnd('/')} 폴더를 확인해 주세요";
            NoticeSystem.OnNotice.Invoke(file.fileName + "파일이 다운로드 되었습니다.", str, 0.1f, true, null, Color.white, ENoticeTag.None, file.parent.ID);
        }

    }

    public List<FileSO> GetALLUnLockFileList(DirectorySO currentDirectory = null, bool isAdditional = false)
    {
        //fileList.Clear();
        if (currentDirectory == null)
        {
            currentDirectory = rootDirectory;
        }

        List<FileSO> fileList = new List<FileSO>();

        Queue<DirectorySO> directories = new Queue<DirectorySO>();
        directories.Enqueue(currentDirectory);
        int i = 0;
        while (directories.Count != 0)
        {
            DirectorySO directory = directories.Dequeue();
            i++;
            if (i > 10000)
            {
                Debug.LogWarning("while문이 계속해서 실행중입니다.");
                break;
            }
            foreach (FileSO file in directory.children)
            {
                if (file == null)
                {
                    continue;
                }
                if (DataManager.Inst.IsPinLock(file.ID))
                {
                    continue;
                }
                fileList.Add(file);
                if (file is DirectorySO)
                {
                    directories.Enqueue(file as DirectorySO);
                }
            }
        }

        if (isAdditional)
        {
            fileList.AddRange(additionFileList);
        }

        return fileList;
    }
    public List<FileSO> GetALLFileList(DirectorySO currentDirectory = null, bool isAdditional = false)
    {
        //fileList.Clear();
        if (currentDirectory == null)
        {
            currentDirectory = rootDirectory;
        }

        List<FileSO> fileList = new List<FileSO>();

        Queue<DirectorySO> directories = new Queue<DirectorySO>();
        directories.Enqueue(currentDirectory);
        int i = 0;
        while (directories.Count != 0)
        {
            DirectorySO directory = directories.Dequeue();
            i++;
            if (i > 10000)
            {
                Debug.LogWarning("while문이 계속해서 실행중입니다.");
                break;
            }
            foreach (FileSO file in directory.children)
            {
                if (file == null)
                {
                    continue;
                }
                fileList.Add(file);
                if (file is DirectorySO)
                {
                    directories.Enqueue(file as DirectorySO);
                }
            }
        }

        if (isAdditional)
        {
            fileList.AddRange(additionFileList);
        }

        return fileList;
    }
    public List<FileSO> GetFileIDList(List<string> fileIDList)
    {
        List<FileSO> allFileList = GetALLFileList();
        List<FileSO> fileList = new List<FileSO>();
        foreach (string fileID in fileIDList)
        {
            FileSO file = allFileList.Find(x => x.ID == fileID);

            if (file == null)
            {
                continue;
            }

            fileList.Add(file);
        }
        return fileList;
    }
    public bool IsExistFile(string id)
    {
        List<FileSO> allFileList = GetALLFileList();

        return allFileList.Find(x => x.ID == id) == true;
    }
    public List<FileSO> SearchFile(string text, DirectorySO currentDirectory = null)
    {
        List<FileSO> allFileList = GetALLUnLockFileList(currentDirectory);

        List<FileSO> searchFileList = new List<FileSO>();

        foreach (FileSO file in allFileList)
        {
            if (file == null)
            {
                continue;
            }
            if (file.fileName.Contains(text, StringComparison.OrdinalIgnoreCase))
            {
                searchFileList.Add(file);
            }
        }

        return searchFileList;
    }

    public void ResetAdditionalFile()
    {
        List<FileSO> allFileList = GetALLUnLockFileList(rootDirectory);

        foreach (var file in allFileList)
        {
            if (file is DirectorySO)
            {
                foreach (var addfile in additionFileList)
                {
                    DirectorySO directory = file as DirectorySO;
                    if (directory.children.Contains(addfile))
                    {
                        directory.children.Remove(addfile);
                    }
                }
            }
        }

        backgroundIcons.Init();
    }
    //public List<FileSO> ProfileSearchFile(string text, DirectorySO currentDirectory = null)
    //{
    //    List<FileSO> allFileList = GetALLFileList(currentDirectory);

    //    text = Regex.Replace(text, @"[^0-9a-zA-Z가-힣_\s]", "");

    //    string[] words = text.Split(" ");

    //    currentFileNameWord.Clear();
    //    //foundFileWeights.Clear();

    //    foreach (FileSO file in allFileList)
    //    {
    //        if (file == null)
    //        {
    //            continue;
    //        }

    //        if (file.isCantFind)
    //        {
    //            continue;
    //        }
    //        string fileName = Regex.Replace(file.fileName, @"[^0-9a-zA-Z가-힣\s]", "");
    //        string[] fileNameWords = fileName.Split(" ");
    //        float fileNameWeight = 0;
    //        float tagWeight = 0;

    //        //isSearchByFileName = false;
    //        isSearchTag = false;

    //        foreach (var word in words)
    //        {
    //            fileNameWeight += SearchFileName(fileNameWords, word, fileName);
    //            foreach (var tag in file.tags)
    //            {
    //                tagWeight += SearchTag(tag, word);
    //            }

    //            if (file.windowType == EWindowType.Notepad)
    //            {
    //                tagWeight += SearchNotePad(word, file);
    //            }
    //        }

    //        if (!isSearchByFileName)
    //        {
    //            fileNameWeight = 0;
    //        }
    //        if (!isSearchTag)
    //        {
    //            tagWeight = 0;
    //        }
    //        FileWeight fileWeight = new FileWeight(file, fileNameWeight + tagWeight);

    //        foundFileWeights.Add(fileWeight);

    //    }

    //    foreach (FileSO file in allFileList)
    //    {
    //        if (file is DirectorySO)
    //        {
    //            DirectorySO directory = file as DirectorySO;
    //            CalcDirectoryWeight(directory);
    //        }
    //    }

    //    if (foundFileWeights.Count > 5)
    //    {
    //        ProfileChattingSystem.OnPlayChat?.Invoke($"'{text}'와 관련된 정보가 너무 많습니다.", false, false);
    //    }

    //    List<FileSO> fileList = foundFileWeights.Where(x =>
    //    {
    //        bool result = false;
    //        if (x.file.windowType == EWindowType.SiteShortCut || x.file.windowType == EWindowType.HarmonyShortCut)
    //            result = true;
    //        if (x.file is DirectorySO)
    //            result = true;

    //        if (x.weight <= 0)
    //            result = false;


    //        return result;
    //    }).OrderByDescending((x) => x.weight).Select((x) => x.file).Take(5).ToList();

    //    Debug.Log(fileList.Count);

    //    return fileList;
    //}

    //private float SearchNotePad(string word, FileSO file)
    //{
    //    string infoString = ResourceManager.Inst.GetNotepadData(file.id).scripts;
    //    float weight = 0f;

    //    string[] infoWords = infoString.Split(" ");

    //    foreach (var infoWord in infoWords)
    //    {
    //        if (infoWord == word)
    //        {
    //            isSearchTag = true;
    //            weight += GetWeight(word.Length, infoString.Length, findTagScore);
    //        }
    //    }


    //    return weight;
    //}

    //private void CalcDirectoryWeight(DirectorySO currentFile)
    //{
    //    float totalweigt = 0;

    //    FileWeight currentFileWeight = foundFileWeights.Find(x => x.file == currentFile);
    //    if (currentFileWeight == null || currentFileWeight.isCompleteWeightDirectory)
    //    {
    //        return;
    //    }
    //    int cnt = 0;
    //    foreach (FileSO child in currentFile.children)
    //    {
    //        FileWeight childWeight = foundFileWeights.Find(x => x.file == child);

    //        if (childWeight == null)
    //        {
    //            continue;
    //        }
    //        cnt++;
    //        if (child is DirectorySO && childWeight.isCompleteWeightDirectory == false)
    //        {
    //            CalcDirectoryWeight(child as DirectorySO);
    //        }

    //        totalweigt += childWeight.weight;
    //    }

    //    if(cnt != 0)
    //    {
    //        totalweigt = totalweigt / cnt * 0.75f + currentFileWeight.weight / 2;
    //    }

    //    currentFileWeight.isCompleteWeightDirectory = true;
    //    currentFileWeight.weight = totalweigt;
    //}
    //private float GetWeight(int matchWordCnt, int wordCnt, float maxScore)
    //{
    //    float weight = 0;
    //    float t;
    //    t = (float)matchWordCnt / wordCnt;
    //    weight += maxScore * t;
    //    return weight;

    //}
    //private float SearchFileName(string[] fileNameWords, string word, string fileName)
    //{
    //    float weight = 0;

    //    foreach (var fileNameWord in fileNameWords)
    //    {
    //        if (fileNameWord == word)
    //        {
    //            //isSearchByFileName = true;
    //            weight += GetWeight(word.Length, fileName.Length, findNameScore);
    //        }
    //    }
    //    return weight;
    //}
    //private float SearchTag(string fileTag, string word)
    //{
    //    float weight = 0;
    //    string[] tagWords = fileTag.Split(" ");

    //    foreach (string tagWord in tagWords)
    //    {
    //        if (tagWord == word)
    //        {
    //            isSearchTag = true;
    //            weight += GetWeight(word.Length, fileTag.Length, findTagScore);
    //        }
    //    }
    //    return weight;
    //}
    //private FileSO GetFile(string location)
    //{
    //    FileSO resultFile = null;
    //    resultFile = GetALLFileList().Find((x) => x.GetFileLocation() == location);
    //    return resultFile;
    //}

#if UNITY_EDITOR

    private void OnDestroy()
    {
        foreach (var dd in debugAdditionFileList)
        {
            DirectorySO parent = dd.parent;

            if (parent != null)
            {
                parent.children.Remove(dd);
            }
        }
    }
#endif
}
