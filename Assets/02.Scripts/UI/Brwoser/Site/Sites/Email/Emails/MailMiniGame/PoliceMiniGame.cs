using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PoliceMiniGame : MonoBehaviour
{
    [SerializeField]
    private Button startBtn;

    [SerializeField]
    private PoliceGameArrow arrowPrefab;

    [SerializeField]
    private Transform arrowPoolParent;
    [SerializeField]
    private Transform arrowParent;
    [SerializeField]
    private float limitTime = 25.0f;

    [SerializeField]
    private int gameCount;
    [SerializeField]
    private int arrowCount;
    private List<PoliceGameArrow> arrows;  
    private Queue<PoliceGameArrow> arrowsPool;
    private int answerCount = 0;
    private bool isStarted = false;

    private void Awake()
    {
        CreatePool();
    }

    private void CreatePool()
    {
        for (int i = 0; i < 25; i++)
        {
            PoliceGameArrow arrow = Instantiate(arrowPrefab, arrowPoolParent);
            arrow.gameObject.SetActive(false);
            arrowsPool.Enqueue(arrow);
        }
    }

    public void StartGame()
    {
        SettingNewGame();
        isStarted = true;
    }

    public void SettingNewGame()
    {
        for(int i = 0; i < arrowCount; i++)
        {
            PoliceGameArrow arrow = arrowsPool.Dequeue();
            arrow.transform.SetParent(arrowParent);
            arrow.Init();
            arrows.Add(arrow);
            arrow.gameObject.SetActive(true);
        }
    }

    

    private void Update()
    {
        if(isStarted == false)
        {
            return;
        }   

        if(arrows.Count <= 0)
        {
            if(answerCount >= gameCount)
            {
                GameClear();
            } 
            else
            {
                answerCount++;
                SettingNewGame();
            }
        } 
        if(Input.GetKeyDown(arrows[0].AnswerKey))
        {
            arrows[0].Succcess();
            arrowsPool.Enqueue(arrows[0]);
            arrows.RemoveAt(0);
        }
        else if(Input.anyKeyDown)
        {
            arrows[0].Fail();
        }
    }

    private void GameClear()
    {
        Debug.Log("GameClear");
    }
}
