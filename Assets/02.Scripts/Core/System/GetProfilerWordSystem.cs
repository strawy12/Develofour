using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECursorState = CursorChangeSystem.ECursorState;

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
    public static Func<string, object[]> OnGeneratedProfiler; 
    public static Func<string, CursorChangeSystem.ECursorState> OnFindWord;

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

    private object[] RegistrationProfiler(string word)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return null;
        }

        if (!wordListDictionary.ContainsKey(word))
        {
            return null;
        }
        
        EProfileCategory category = wordListDictionary[word].category;
        string information = wordListDictionary[word].information;

        object[] value = new object[] { category, information };
        return value;
    }

    private ECursorState FindedWordCheck(string word)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return ECursorState.Default;
        }

        ECursorState state = ECursorState.Default;

        if (word == null || !wordListDictionary.ContainsKey(word))
        {
            EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
            return state;
        }

        if (!willGetWordList.Find(x => x.word == word).isFinded)
        {
            state = ECursorState.FindInfo;
        }
        else
        {
            state = ECursorState.FoundInfo;
        }

        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
        return state;
    }
}
