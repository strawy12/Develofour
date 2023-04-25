using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECursorState = CursorChangeSystem.ECursorState;


public class GetProfilerWordSystem : MonoBehaviour 
{
    public static Func<string, object[]> OnGeneratedProfiler; 
    public static Func<string, CursorChangeSystem.ECursorState> OnFindWord;

    [SerializeField]
    private List<ProfileInfoTextDataSO> willGetWordList;

    private Dictionary<string, ProfileInfoTextDataSO> wordListDictionary;

    private void Start()
    {
        wordListDictionary = new Dictionary<string, ProfileInfoTextDataSO>();

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
        foreach (ProfileInfoTextDataSO profiler in willGetWordList)
        {
            wordListDictionary.Add(profiler.key, profiler);
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
        string information = wordListDictionary[word].key;

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

        ProfileInfoTextDataSO data = willGetWordList.Find(x => x.key == word);

        if (!DataManager.Inst.IsProfileInfoData(data.category, data.key))
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
