using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Text.RegularExpressions;

public class FileManager : MonoSingleton<FileManager>
{
    private class FileWeight
    {
        public FileSO file;
        public float weight;

        public FileWeight(FileSO file, float weight)
        {
            this.file = file;
            this.weight = weight;
        }
    }

    [SerializeField]
    private DirectorySO rootDirectory;
    [SerializeField]
    private List<FileSO> additionFileList = new List<FileSO>();
    private List<FileSO> debugAdditionFileList = new List<FileSO>();
    //새롭게 추가된 파일은 fileList에 등록된다.
    [SerializeField]
    private List<FileSO> defaultFileList = new List<FileSO>();
    [Header("Profile FileSearch")]
    [SerializeField]
    private float findNameScore = 30f;
    [SerializeField]
    private float findTagScore = 20f;
    private bool isSearchTag = false;
    private bool isSearchByFileName = false;
    private Dictionary<EWindowType, FileSO> defaultFileDictionary = new Dictionary<EWindowType, FileSO>();
    private List<string> currentFileNameWord;
    private void Awake()
    {
        currentFileNameWord = new List<string>();
        foreach (FileSO file in defaultFileList)
        {
            defaultFileDictionary.Add(file.windowType, file);
        }
    }
    private void Start()
    {
        foreach (var fileData in additionFileList)
        {
            if (DataManager.Inst.AdditionalFileContain(fileData))
            {
                string location = DataManager.Inst.GetAdditionalFileName(fileData);
                Debug.Log(location);
                AddFile(fileData, location);
            }
        }
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

    public void AddFile(FileSO file, string location)
    {
        List<FileSO> fileList = ALLFileAddList();

        DirectorySO currentDir = rootDirectory;

        currentDir = fileList.Find((x) => x.GetFileLocation() == location) as DirectorySO;

        if (!currentDir.children.Contains(file))
        {
            currentDir.children.Add(file);
            file.parent = currentDir;
            debugAdditionFileList.Add(file);
        }

        if (!DataManager.Inst.AdditionalFileContain(file))
        {
            DataManager.Inst.AddNewFileData(file, location + file.fileName + "\\");
        }
        EventManager.TriggerEvent(ELibraryEvent.AddFile);
    }

    public List<FileSO> ALLFileAddList(DirectorySO currentDirectory = null, bool isAdditional = false)
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

    public List<FileSO> SearchFile(string text, DirectorySO currentDirectory = null)
    {
        List<FileSO> allFileList = ALLFileAddList(currentDirectory);

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

    public List<FileSO> ProfileSearchFile(string text, DirectorySO currentDirectory = null)
    {
        List<FileSO> allFileList = ALLFileAddList(currentDirectory);
        List<FileWeight> searchFileList = new List<FileWeight>();

        text = Regex.Replace(text, @"[^0-9a-zA-Z가-힣_\s]", "");
        
        string[] words = text.Split(" ");

        currentFileNameWord.Clear();

        foreach (FileSO file in allFileList)
        {
            if (file == null)
            {
                continue;
            }

            //파일 이름도 특수문자 제거해서 단어로 나눌게.
            string fileName = Regex.Replace(file.fileName, @"[^0-9a-zA-Z가-힣\s]", "");
            string[] fileNameWords = fileName.Split(" ");
            float fileNameWeight = 0;
            float tagWeight = 0;
            isSearchByFileName = false;
            bool isSearch = true;
            foreach (var word in words)
            {
                Debug.Log(word);
                SearchFileName(fileNameWords, word, fileName);

                foreach (var tag in file.tags)
                {
                    tagWeight += SearchTag(tag, word);
                }

                if(!isSearchTag && !isSearchByFileName)
                {
                    Debug.Log($"isSearchTag{isSearchTag}, isSearchByFileName{isSearchByFileName} , FileName{fileName}");
                    isSearch = false;
                    break;
                }
            }
            if(isSearch)
            {
                Debug.Log(isSearch);
                if (!isSearchByFileName) fileNameWeight = 0;
                if (!isSearchTag) tagWeight = 0;
                Debug.Log("FileName:" + file.fileName);
                FileWeight fileWeight = new FileWeight(file, fileNameWeight + tagWeight);
                searchFileList.Add(fileWeight);
            }

        }
        List<FileSO> fileList = searchFileList.OrderByDescending((x) => x.weight).Select((x) => x.file).Take(5).ToList();

        return fileList;
    }

    private float GetWeight(int matchWordCnt, int wordCnt, float maxScore)
    {
        float weight = 0;
        float t = 0;
        t = matchWordCnt / wordCnt;

        weight += maxScore * t;
        return weight;

    }
    private float SearchFileName(string[] fileNameWords, string word, string fileName)
    {
        float weight = 0;
        foreach (var fileNameWord in fileNameWords)
        {
            if (fileNameWord == word)
            {
                Debug.Log("fileName Match" + fileNameWord);
                isSearchByFileName = true;
                weight += GetWeight(word.Length, fileName.Length, findNameScore);
            }
        }
        return weight;
    }
    private float SearchTag(string fileTag, string word)
    {
        float weigth = 0;
        isSearchTag = false;

        string[] tagWords = fileTag.Split(" ");

        foreach (string tagWord in tagWords)
        {
            if (tagWord == word)
            {
                Debug.Log("Tag Match " + tagWord);
                isSearchTag = true;
                weigth += GetWeight(word.Length, fileTag.Length, findTagScore);
            }
            else
            {
                weigth = 0;
                break;
            }
        }
        return weigth;
    }
    private FileSO GetFile(string location)
    {
        FileSO resultFile = null;
        resultFile = ALLFileAddList().Find((x) => x.GetFileLocation() == location);
        return resultFile;
    }

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
