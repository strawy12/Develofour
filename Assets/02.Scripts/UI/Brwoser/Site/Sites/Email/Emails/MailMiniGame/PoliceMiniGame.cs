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
    private PoliceGameSendButton sendButton;

    [SerializeField]
    private List<PoliceGameArrow> arrows;
    private Queue<PoliceGameArrow> arrowsPool;
    private bool isStarted = false;
    private bool isCleared = false;
    public bool IsCleared { get { return isCleared; } }
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
        if (isStarted) return;
        answerCount = 0;
        SettingNewGame();
        currentTime = limitTime;
        isStarted = true;

        for (KeyCode key = KeyCode.UpArrow; key <= KeyCode.LeftArrow; key++)
        {
            KeyCode keyCode = key;
            InputManager.Inst.AddKeyInput(key, onKeyDown: () => InputArrowKey(keyCode));
        }

        StartCoroutine(GameTimeCoroutine());
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

    private void InputArrowKey(KeyCode key)
    {
        if (!isStarted) return;
        if (arrows[0].IsInputed) return;

        Debug.Log(key);

        if(key == arrows[0].AnswerKey)
        {
            arrows[0].Succcess();
            arrows.RemoveAt(0);
        }

        else 
        {
            arrows[0].Fail();
        }

        CheckClear();
    }
     
    private void CheckClear()
    {
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
    }

    private void GameFail()
    {
        Debug.Log("Fail");
        foreach (PoliceGameArrow arrow in arrows)
        {
            arrow.Pop();
        }
        arrows.Clear();
        isStarted = false;
    }

    private void GameClear()
    {
        for(KeyCode key = KeyCode.UpArrow; key <= KeyCode.LeftArrow; key++)
        {
            InputManager.Inst.RemoveKeyInput(key, onKeyDown: () => InputArrowKey(key));
        }

        isCleared = true;
        EventManager.TriggerEvent(EGamilSiteEvent.PoliceGameClear);
        isStarted = false;
        sendButton.SuccessEffect();
    }

    private IEnumerator GameTimeCoroutine()
    {
        while (isStarted && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            timerUI.localScale = new Vector3(currentTime / limitTime, 1, 1);

            yield return new WaitForEndOfFrame();
        }

        if (currentTime <= 0f)
        {
            GameFail();
        }
    }
}
