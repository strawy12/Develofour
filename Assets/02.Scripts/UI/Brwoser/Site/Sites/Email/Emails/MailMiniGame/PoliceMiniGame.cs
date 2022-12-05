using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    private float characterTime = 0.1f;


    [SerializeField]
    private RectTransform timerUI;

    [SerializeField]
    private PoliceGameSendButton sendButton;

    [SerializeField]
    private TMP_Text sendText;

    [SerializeField]
    [TextArea(3, 5)]
    private List<string> sendTextMessageList;

    private Queue<PoliceGameArrow> arrows;

    private Queue<PoliceGameArrow> arrowsPool;

    private bool isStarted = false;
    private bool isCleared = false;

    public bool IsCleared { get { return isCleared; } }

    private int gameCount;
    private int answerCount = 0;
    private float currentTime = 0f;

    private string currentMsg = "";

    private bool isDelay = false;

    private Coroutine timerCoroutine = null;

    public void Init()
    {
        arrows = new Queue<PoliceGameArrow>();
        arrowsPool = new Queue<PoliceGameArrow>();
        CreatePool();
        startBtn.onClick.AddListener(StartGame);
    }

    private void CreatePool()
    {
        for (int i = 0; i < 25; i++)
        {
            arrowsPool.Enqueue(CreateArrow());
        }
    }

    private PoliceGameArrow CreateArrow()
    {
        PoliceGameArrow arrow = Instantiate(arrowPrefab, arrowPoolParent);
        arrow.OnPush += PushArrow;
        arrow.gameObject.SetActive(false);

        return arrow;
    }

    private void PushArrow(PoliceGameArrow arrow)
    {
        arrowsPool.Enqueue(arrow);
        arrow.transform.SetParent(arrowPoolParent);
    }

    private PoliceGameArrow GetArrow()
    {
        if(arrowsPool.Count != 0)
        {
            return arrowsPool.Dequeue();
        }

        else
        {
            return CreateArrow();
        }
    }

    public void StartGame()
    {
        if (isStarted) return;
        answerCount = 0;
        gameCount = sendTextMessageList.Count;
        StartCoroutine(SettingNewGame(0f));
        isStarted = true;

        InputManager.Inst.AddKeyInput(KeyCode.D, onKeyDown: GameClear);

        for (KeyCode key = KeyCode.UpArrow; key <= KeyCode.LeftArrow; key++)
        {
            KeyCode keyCode = key;
            InputManager.Inst.AddKeyInput(key, onKeyDown: () => InputArrowKey(keyCode));
        }
    }

    public IEnumerator SettingNewGame(float delay)
    {
        isDelay = true;

        if(timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        yield return new WaitForSeconds(delay);

        sendText.SetText("");
        currentMsg = sendTextMessageList[answerCount];
        currentTime = characterTime * currentMsg.Length;

        for (int i = 0; i < currentMsg.Length; i++)
        {
            PoliceGameArrow arrow = GetArrow();
            arrow.ResetObject();
            arrow.transform.SetParent(arrowParent);
            arrow.Init();
            arrows.Enqueue(arrow);
            arrow.gameObject.SetActive(true);
        }

        isDelay = false;

        timerCoroutine = StartCoroutine(GameTimeCoroutine());
    }



    private void InputArrowKey(KeyCode key)
    {
        if (!isStarted) return;
        if (isDelay) return;
        if (arrows.Count == 0 || arrows.Peek().IsInputed) return;

        if (key == arrows.Peek().AnswerKey)
        {
            arrows.Dequeue().Succcess();

            sendText.SetText(string.Format("{0}{1}", sendText.text, currentMsg[sendText.text.Length]));
        }

        else
        {
            arrows.Peek().Fail();
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
                return ;
            }
            else
            {
                StartCoroutine(SettingNewGame(0.3f));
            }
        }
    }

    private void GameFail()
    {
        foreach (PoliceGameArrow arrow in arrows)
        {
            arrow.Pop();
        }
        arrows.Clear();
        isStarted = false;
    }

    private void GameClear()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.D, onKeyDown: GameClear);

        for (KeyCode key = KeyCode.UpArrow; key <= KeyCode.LeftArrow; key++)
        {
            InputManager.Inst.RemoveKeyInput(key, onKeyDown: () => InputArrowKey(key));
        }

        isCleared = true;

        EventManager.TriggerEvent(EMailSiteEvent.PoliceGameClear);
        isStarted = false;

        sendButton.SuccessEffect();
    }

    private IEnumerator GameTimeCoroutine()
    {
        float maxTime = currentTime;
        while (isStarted && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            timerUI.localScale = new Vector3(currentTime / maxTime, 1, 1);

            yield return new WaitForEndOfFrame();
        }

        if (currentTime <= 0f)
        {
            GameFail();
        }
    }
}
