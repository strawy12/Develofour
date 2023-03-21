using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GetWordList
{
    public EProfileCategory category;
    public string information;
}

[System.Serializable]
public class SubstitutionWord
{
    public bool isFindWord;
    public string keyWord;

    public GetWordList value;
}


public class GetProfilerWordSystem : MonoBehaviour 
{
    public static Action<string> OnGeneratedProfiler; 
    public static Action<string> OnFindWord;

    [SerializeField]
    private List<SubstitutionWord> willGetWordList;

    private Dictionary<string, GetWordList> wordListDictionary;


    void Start()
    {
        wordListDictionary = new Dictionary<string, GetWordList>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
