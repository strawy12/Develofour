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
    [SerializeField]
    private RectTransform timerUI;
    [SerializeField]
    private ScrollRect scrollview;
    private List<PoliceGameArrow> arrows;
    private Queue<PoliceGameArrow> arrowsPool;
    private bool isStarted = false;
    private int answerCount = 0;
    private float currentTime = 0f;

    private void Awake()
    {
        arrows = new List<PoliceGameArrow>();
        arrowsPool = new Queue<PoliceGameArrow>();
        CreatePool();
        startBtn.onClick.AddListener(StartGame);
    }

    

    private void CreatePool()
    {
        for (int i = 0; i < 25; i++)
        {
            PoliceGameArrow arrow = Instantiate(arrowPrefab, arrowPoolParent);
            arrow.OnPush += PushArrow;
            arrow.gameObject.SetActive(false);
            arrowsPool.Enqueue(arrow);
        }
    }

    private void PushArrow(PoliceGameArrow arrow)
    {
        arrowsPool.Enqueue(arrow);
        arrow.transform.SetParent(arrowPoolParent);
    }

    public void StartGame()
    {
        SettingNewGame();
        isStarted = true;
        currentTime = limitTime;
    }

    public void SettingNewGame()
    {
        for (int i = 0; i < arrowCount; i++)
        {
            PoliceGameArrow arrow = arrowsPool.Dequeue();
            arrow.ResetObject();
            arrow.transform.SetParent(arrowParent);
            arrow.Init();
            arrows.Add(arrow);
            arrow.gameObject.SetActive(true);
        }
    }



    private void Update()
    {
        if (isStarted == false)
        {
            return;
        }
        currentTime -= Time.deltaTime;
        timerUI.localScale = new Vector3(currentTime / limitTime, 1, 1);
        if (currentTime < 0)
        { 
            GameFail();
        }

        if (arrows.Count <= 0)
        {
            answerCount++;
            if (answerCount >= gameCount)
            {
                GameClear();
                return;
            }
            else
            {
                SettingNewGame();
            }
        }
        if (Input.GetKeyDown(arrows[0].AnswerKey) && !arrows[0].IsInputed)
        {
            arrows[0].Succcess();
            arrows.RemoveAt(0);
        }
        else if (Input.anyKeyDown && !arrows[0].IsInputed)
        {
            arrows[0].Fail();
        }
    }

    private void GameFail()
    {
        Debug.Log("Fail");
        isStarted = false;
    }

    private void GameClear()
    {
        Debug.Log("GameClear");

        isStarted = false;
    }
}
