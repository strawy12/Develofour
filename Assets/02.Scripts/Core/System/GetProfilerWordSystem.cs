using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

[System.Serializable]
public class ProfilerWord
{
    public EProfileCategory category;
    public string information;
}

[System.Serializable]
public class ProfilerWordElement
{
    public bool isFindWord;
    public string word;
    public ProfilerWord value;
}


public class GetProfilerWordSystem : MonoBehaviour
{
    public static Action<string> OnGeneratedProfiler; // 프로파일러에 정보를 넣어주는 애고
    public static Action<string> OnFindWord; // 프로파일러가 정보를 찾았는지 알려주는 애

    [SerializeField]
    private List<ProfilerWordElement> substitutionList;

    private Dictionary<string, ProfilerWord> substitutionDictionary;

    private void Start()
    {
        substitutionDictionary = new Dictionary<string, ProfilerWord>();

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
        foreach (ProfilerWordElement profiler in substitutionList)
        {
            substitutionDictionary.Add(profiler.word, profiler.value);
        }
    }

    private void RegistrationProfiler(string word)
    {
        if (!substitutionDictionary.ContainsKey(word))
        {
            return;
        }

        substitutionList.Find(x => x.word == word).isFindWord = true;


        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.GetNotePadWordsForProfiler, 0);

        EventManager.TriggerEvent(
            EProfileEvent.FindInfoText,
            new object[2] { substitutionDictionary[word].category, substitutionDictionary[word].information });
    }

    private void FindedWordCheck(string word)
    {
        if (word == null)
        {
            return;
        }

        if(!substitutionDictionary.ContainsKey(word) || !substitutionList.Find(x => x.word == word).isFindWord)
        {
            EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { "FindingWord" });
        }
        else
        {
            EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { "FindedWord" });
        }
    }
}
