using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProfilerWord
{
    public EProfileCategory category;
    public string information;
}

[System.Serializable]
public class SubstitutionWord
{
    public bool isFinded;
    public string word;

    public ProfilerWord value;
}


public class GetProfilerWordSystem : MonoBehaviour 
{
    public static Action<string> OnGeneratedProfiler; 
    public static Action<string> OnFindWord;

    [SerializeField]
    private List<SubstitutionWord> willGetWordList;

    private Dictionary<string, ProfilerWord> wordListDictionary;

    private void Start()
    {
        wordListDictionary = new Dictionary<string, ProfilerWord>();

        Init();
    }

    private void Init()
    {
        OnGeneratedProfiler += RegistrationProfiler;
        OnFindWord += FindedWordCheck;

        DictionaryInit();
    }

    private void DictionaryInit()
    {
        foreach (SubstitutionWord profiler in willGetWordList)
        {
            wordListDictionary.Add(profiler.word, profiler.value);
        }
    }

    private void RegistrationProfiler(string word)
    {
        if (!wordListDictionary.ContainsKey(word))
        {
            return;
        }

        EProfileCategory category = wordListDictionary[word].category;
        string information = wordListDictionary[word].information;

        willGetWordList.Find(x => x.word == word).isFinded = true;

        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
    }

    private void FindedWordCheck(string word)
    {
        CursorChangeSystem.ECursorState state = CursorChangeSystem.ECursorState.Default;

        if (word == null || !wordListDictionary.ContainsKey(word))
        {
            EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
            return;
        }


        if (!willGetWordList.Find(x => x.word == word).isFinded)
        {
            state = CursorChangeSystem.ECursorState.FindInfo;
        }
        else
        {
            state = CursorChangeSystem.ECursorState.FoundInfo;
        }

        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
    }
}
