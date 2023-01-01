using System.Collections;
using System.Collections.Generic;
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
    private HorizontalLayoutGroup arrowParent;

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
    private bool isCleared
    {
        get
        {
            return DataManager.Inst.CurrentPlayer.questClearData.isPoliceMiniGameClear;
        }
        set
        {
            DataManager.Inst.CurrentPlayer.questClearData.isPoliceMiniGameClear = value;
        }
    }

    public bool IsCleared { get { return isCleared; } }
    public bool IsStarted { get { return isStarted; } }


    private int gameCount;
    private int answerCount = 0;
    private float currentTime = 0f;

    private Queue<string> currentMsgWordQueue;

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

    private void PushAll()
    {
        foreach (PoliceGameArrow arrow in arrows)
        {
            arrow.Push();
        }
    }

    private PoliceGameArrow GetArrow()
    {
        if (arrowsPool.Count != 0)
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

        InputManager.Inst.AddKeyInput(KeyCode.RightArrow, onKeyDown: InputRightArrowKey);
        InputManager.Inst.AddKeyInput(KeyCode.LeftArrow, onKeyDown: InputLeftArrowKey);
        InputManager.Inst.AddKeyInput(KeyCode.UpArrow, onKeyDown: InputUpArrowKey);
        InputManager.Inst.AddKeyInput(KeyCode.DownArrow, onKeyDown: InputDownArrowKey);
    }

    public IEnumerator SettingNewGame(float delay)
    {
        isDelay = true;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
        }

        yield return new WaitForSeconds(delay);

        sendText.SetText("");
        currentMsgWordQueue = new Queue<string>(sendTextMessageList[answerCount].Split(' '));
        currentTime = characterTime * currentMsgWordQueue.Count;
        arrowParent.enabled = true;

        for (int i = 0; i < currentMsgWordQueue.Count; i++)
        {
            PoliceGameArrow arrow = GetArrow();
            arrow.ResetObject();
            arrow.transform.SetParent(arrowParent.transform);
            arrow.Init();
            arrows.Enqueue(arrow);
            arrow.gameObject.SetActive(true);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(arrowParent.transform as RectTransform);
        arrowParent.enabled = false;
        
        isDelay = false;

        timerCoroutine = StartCoroutine(GameTimeCoroutine());
    }

    private void InputRightArrowKey()
    {
        if (!isStarted) return;
        if (isDelay) return;
        if (arrows.Count == 0 || arrows.Peek().IsInputed) return;

        if (KeyCode.RightArrow == arrows.Peek().AnswerKey)
        {
            arrows.Dequeue().Succcess();

            sendText.SetText(string.Format("{0} {1}", sendText.text, currentMsgWordQueue.Dequeue()));
        }
        else
        {
            arrows.Peek().Fail();
        }

        CheckClear();
    }
    private void InputDownArrowKey()
    {
        if (!isStarted) return;
        if (isDelay) return;
        if (arrows.Count == 0 || arrows.Peek().IsInputed) return;

        if (KeyCode.DownArrow == arrows.Peek().AnswerKey)
        {
            arrows.Dequeue().Succcess();

            sendText.SetText(string.Format("{0} {1}", sendText.text, currentMsgWordQueue.Dequeue()));
        }
        else
        {
            arrows.Peek().Fail();
        }

        CheckClear();
    }
    private void InputUpArrowKey()
    {
        if (!isStarted) return;
        if (isDelay) return;
        if (arrows.Count == 0 || arrows.Peek().IsInputed) return;

        if (KeyCode.UpArrow == arrows.Peek().AnswerKey)
        {
            arrows.Dequeue().Succcess();

            sendText.SetText(string.Format("{0} {1}", sendText.text, currentMsgWordQueue.Dequeue()));
        }
        else
        {
            arrows.Peek().Fail();
        }

        CheckClear();
    }
    private void InputLeftArrowKey()
    {
        if (!isStarted) return;
        if (isDelay) return;
        if (arrows.Count == 0 || arrows.Peek().IsInputed) return;

        if (KeyCode.LeftArrow == arrows.Peek().AnswerKey)
        {
            arrows.Dequeue().Succcess();

            sendText.SetText(string.Format("{0} {1}", sendText.text, currentMsgWordQueue.Dequeue()));
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
                return;
            }
            else
            {
                StartCoroutine(SettingNewGame(0.3f));
            }
        }
    }

    private void GameFail()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.D, onKeyDown: GameClear);

        InputManager.Inst.RemoveKeyInput(KeyCode.RightArrow, onKeyDown: InputRightArrowKey);
        InputManager.Inst.RemoveKeyInput(KeyCode.LeftArrow, onKeyDown: InputLeftArrowKey);
        InputManager.Inst.RemoveKeyInput(KeyCode.UpArrow, onKeyDown: InputUpArrowKey);
        InputManager.Inst.RemoveKeyInput(KeyCode.DownArrow, onKeyDown: InputDownArrowKey);

        PushAll();
        arrows.Clear();
        isStarted = false;
    }

    private void GameClear()
    {
        InputManager.Inst.RemoveKeyInput(KeyCode.D, onKeyDown: GameClear);

        InputManager.Inst.RemoveKeyInput(KeyCode.RightArrow, onKeyDown: InputRightArrowKey);
        InputManager.Inst.RemoveKeyInput(KeyCode.LeftArrow, onKeyDown: InputLeftArrowKey);
        InputManager.Inst.RemoveKeyInput(KeyCode.UpArrow, onKeyDown: InputUpArrowKey);
        InputManager.Inst.RemoveKeyInput(KeyCode.DownArrow, onKeyDown: InputDownArrowKey);
        PushAll();
        arrows.Clear();
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

    public void InitializationGame()
    {
        isStarted = false;
        currentTime = 0f;
        answerCount = 0;

        sendText.SetText("");

        GameFail();

        gameObject.SetActive(false);
    }
}
