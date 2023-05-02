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
        public bool isCompleteWeightDirectory;
        public FileWeight(FileSO file, float weight)
        {
            this.file = file;
            this.weight = weight;
            isCompleteWeightDirectory = false;
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
    private const float findNameScore = 30f;
    private const float findTagScore = 20f;
    private bool isSearchTag = false;
    private bool isSearchByFileName = false;
    private Dictionary<EWindowType, FileSO> defaultFileDictionary = new Dictionary<EWindowType, FileSO>();
    private List<string> currentFileNameWord;
    List<FileWeight> foundFileWeights;
    private void Awake()
    {
        foundFileWeights = new List<FileWeight>();
        currentFileNameWord = new List<string>();
        foreach (FileSO file in defaultFileList)
        {
            defaultFileDictionary.Add(file.windowType, file);
        }
    }

    public void Init()
    {
        foreach (var fileData in additionFileList)
        {
            if (DataManager.Inst.AdditionalFileContain(fileData))
            {
                string location = DataManager.Inst.GetAdditionalFileLocation(fileData);
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

    public bool IsExist(string location)
    {
        List<FileSO> fileList = ALLFileAddList();

        FileSO file = fileList.Find((x) => x.GetFileLocation() == location);

        if (file != null)
            return true;
        return false;
    }

    public FileSO GetAdditionalFile(string fileName)
    {
        FileSO folder = additionFileList.Find((x) => x.fileName == fileName);
        return folder;
    }

    public FileSO GetAdditionalFile(int key)
    {
        foreach(var temp in additionFileList)
        {
            Debug.Log(temp.id);
        }
        FileSO folder = additionFileList.Find((x) => x.id == key);

        return folder;
    }

    public void AddFile(FileSO file, string location)
    {
        List<FileSO> fileList = ALLFileAddList();
        DirectorySO currentDir = rootDirectory;

        currentDir = fileList.Find((x) => x.GetFileLocation() == location) as DirectorySO;
        
        if (currentDir == null)
        {
            //디렉토리 생성

        }


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
                if(file == null)
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

        text = Regex.Replace(text, @"[^0-9a-zA-Z가-힣_\s]", "");

        string[] words = text.Split(" ");

        currentFileNameWord.Clear();
        foundFileWeights.Clear();

        foreach (FileSO file in allFileList)
        {
            if (file == null)
            {
                continue;
            }

            string fileName = Regex.Replace(file.fileName, @"[^0-9a-zA-Z가-힣\s]", "");
            string[] fileNameWords = fileName.Split(" ");
            float fileNameWeight = 0;
            float tagWeight = 0;
            isSearchByFileName = false;
            isSearchTag = false;

            foreach (var word in words)
            {
                fileNameWeight += SearchFileName(fileNameWords, word, fileName);

                foreach (var tag in file.tags)
                {
                    tagWeight += SearchTag(tag, word);
                }
            }

            if(isSearchByFileName || isSearchTag)
            {
                if (!isSearchByFileName)
                {
                    fileNameWeight = 0;
                }
                if (!isSearchTag)
                {
                    tagWeight = 0;
                }
                FileWeight fileWeight = new FileWeight(file, fileNameWeight + tagWeight);

                foundFileWeights.Add(fileWeight);
            }
        }

        foreach (FileSO file in allFileList)
        {
            if (file is DirectorySO)
            {
                DirectorySO directory = file as DirectorySO;
                CalcDirectoryWeight(directory);
            }
        }

        if (foundFileWeights.Count > 5)
        {
            TextData textData = new TextData() { color = Color.black, text = $"'{text}'와 관련된 정보가 너무 많습니다." };
            ProfileChattingSystem.OnPlayChat?.Invoke(textData, false, false);
        }

        List<FileSO> fileList = foundFileWeights.Where(x =>
        {
            bool result = false;
            if (x.file.windowType == EWindowType.SiteShortCut || x.file.windowType == EWindowType.HarmonyShortCut)
                result = true;
            if (x.file is DirectorySO)
                result = true;
            if (x.weight == 0)
                result = false;
            return result;
        }).OrderByDescending((x) => x.weight).Select((x) => x.file).Take(5).ToList();

        return fileList;
    }
    private void CalcDirectoryWeight(DirectorySO currentFile)
    {
        float totalweigt = 0;

        FileWeight currentFileWeight = foundFileWeights.Find(x => x.file == currentFile);
        if (currentFileWeight == null || currentFileWeight.isCompleteWeightDirectory)
        {
            return;
        }
        int cnt = 0;
        foreach (FileSO child in currentFile.children)
        {
            FileWeight childWeight = foundFileWeights.Find(x => x.file == child);

            if (childWeight == null)
            {
                continue;
            }
            cnt++;
            if (child is DirectorySO && childWeight.isCompleteWeightDirectory == false)
            {
                CalcDirectoryWeight(child as DirectorySO);
            }

            totalweigt += childWeight.weight;
        }
        totalweigt = totalweigt / cnt * 0.75f + currentFileWeight.weight / 2;
        currentFileWeight.isCompleteWeightDirectory = true;
        currentFileWeight.weight = totalweigt;
    }
    private float GetWeight(int matchWordCnt, int wordCnt, float maxScore)
    {
        float weight = 0;
        float t;
        t = (float)matchWordCnt / wordCnt;
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
                isSearchByFileName = true;
                weight += GetWeight(word.Length, fileName.Length, findNameScore);
            }
        }
        return weight;
    }
    private float SearchTag(string fileTag, string word)
    {
        float weigth = 0;
        string[] tagWords = fileTag.Split(" ");

        foreach (string tagWord in tagWords)
        {
            if (tagWord == word)
            {
                isSearchTag = true;
                weigth += GetWeight(word.Length, fileTag.Length, findTagScore);
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
