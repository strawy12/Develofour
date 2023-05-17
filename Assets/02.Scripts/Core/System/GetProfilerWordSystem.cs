using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ECursorState = CursorChangeSystem.ECursorState;

public class GetProfilerWordSystem : MonoBehaviour 
{
    [Serializable]
    private class InfoTextData
    {
        public int id;
        public string word;
    }

    public static Func<string, object[]> OnGeneratedProfiler; 
    public static Func<string, CursorChangeSystem.ECursorState> OnFindWord;

    [SerializeField]
    private List<InfoTextData> willGetWordList;

    private Dictionary<string, InfoTextData> wordListDictionary;

    private void Start()
    {
        wordListDictionary = new Dictionary<string, InfoTextData>();

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
        foreach (InfoTextData info in willGetWordList)
        {
            wordListDictionary.Add(info.word, info);
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
        
        int infoID = wordListDictionary[word].id;
        var infoData = ResourceManager.Inst.GetProfileInfoData(infoID);
        object[] value = new object[] { infoID, infoData.category};
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

        InfoTextData data = willGetWordList.Find(x => x.word == word);

        if (!DataManager.Inst.IsProfileInfoData(data.id))
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
