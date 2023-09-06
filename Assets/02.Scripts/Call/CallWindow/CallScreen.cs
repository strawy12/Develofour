using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
[Serializable]
public class CallWindowProfile
{
    public Image profileImage;
    public TMP_Text nameText;
}

public class CallScreen : MonoBehaviour
{
    protected bool isPlaying;

    protected EGameState saveState;

    [SerializeField]
    protected List<CallWindowProfile> userImageList;
    [SerializeField]
    protected CallSelectButton selectButton;

    protected RectTransform selectBtnsTrm;

    protected List<CallSelectButton> buttonList = new List<CallSelectButton>();


    public virtual void Init(RectTransform selectBtnsTrm)
    {
        this.selectBtnsTrm = selectBtnsTrm;
    }

    protected void ProfileSetting(int number, string nameText, Sprite iconSprite = null)
    {
        userImageList[number].nameText.SetText(nameText);
        if (iconSprite != null)
        {
            userImageList[number].profileImage.sprite = iconSprite;
        }
    }

    public virtual void StartCall()
    {
        if (isPlaying) return;

        saveState = GameManager.Inst.GameState;
        isPlaying = true;
    }

    protected void StartMonolog(string monologID)
    {
        MonologSystem.OnStartMonolog?.Invoke(monologID, true);
    }
    public virtual void StopCall(bool isClose)
    {
        GameManager.Inst.ChangeGameState(saveState);
        isPlaying = false;
        StopAllCoroutines();
        if (isClose)
        {
            EventManager.TriggerEvent(ECallEvent.EndCall);
        }
    }

    protected void MakeSelectBtn(CallDataSO callData, string btnText, Action action)
    {
        if (callData == null) return;

        CallSelectButton instance = Instantiate(selectButton, selectBtnsTrm);
        instance.btnText.text = btnText;
        buttonList.Add(instance);

        instance.btn.onClick.AddListener(() => { action?.Invoke(); });
        instance.gameObject.SetActive(true);
    }

    protected void HideSelectBtns()
    {
        if (buttonList == null) return;

        int cnt = 0;
        while (buttonList.Count != 0)
        {
            cnt++;
            if(cnt > 100)
            {
                Debug.Log("while");
                break;
            }
            var btn = buttonList[0];
            buttonList.RemoveAt(0);
            Destroy(btn.gameObject);
        }
        buttonList.Clear();
    }

    protected void SelectBtnCallAction(CallDataSO callData)
    {
        HideSelectBtns();
        StopCall(false);
        EventManager.TriggerEvent(ECallEvent.ClickSelectBtn, new object[] { callData });
    }

    public virtual void OnDestroy()
    {

    }
}
