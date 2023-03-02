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
public class ProfilerWordElement
{
    public string key;
    public ProfilerWord value;
}


public class GetProfilerWordSystem : MonoBehaviour
{
    public static Action<string> OnGeneratedProfiler;

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

        DictionaryInit();
    }

    private void DictionaryInit()
    {
        Debug.Log(substitutionList.Count);
        foreach(ProfilerWordElement profiler in substitutionList)
        {
            substitutionDictionary.Add(profiler.key, profiler.value);
            Debug.Log(profiler.key);
        }
    }

    private void RegistrationProfiler(string word)
    {
        if(!substitutionDictionary.ContainsKey(word))
        {
            return;
        }

        EventManager.TriggerEvent(
            EProfileEvent.FindInfoText, 
            new object[2] { substitutionDictionary[word].category, substitutionDictionary[word].information });
    }
}
