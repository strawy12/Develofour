using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting;

public enum EProfileChatting
{
    Email,
    Password,
}

[Serializable]
public struct ChatData
{
    public EAiChatData eChat;
    [TextArea]
    public string script;
};

public class ProfilerChatting : MonoBehaviour
{
    protected float currentValue;

    [SerializeField]
    protected TMP_Text textPrefab;
    [SerializeField]
    protected GameObject imagePrefab;
    [SerializeField]
    protected NewAIChattingImage newImagePrefab;

    [SerializeField]
    protected Transform textParent;

    public int ChattingCount
    {
        get
        {
            if (textParent.childCount <= 3)
                return 0;
            return textParent.childCount - 3;
        }
    }

    public int saveChatCount;

    [SerializeField]
    protected ScrollRect scroll;
    [SerializeField]
    protected RectTransform scrollrectTransform;
    [SerializeField]
    protected ContentSizeFitter contentSizeFitter;
    public void Init()
    {
        saveChatCount = DataManager.Inst.AIChattingListCount();

        Hide();
    }

    public void Show()
    {
        EventManager.StartListening(EProfilerEvent.ProfilerSendMessage, PrintChat);
        AddSaveTexts();
        SetScrollView();//스크롤뷰 가장 밑으로 내리기;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        EventManager.StopListening(EProfilerEvent.ProfilerSendMessage, PrintChat);
        ActiveNewImageUI(false);

        gameObject.SetActive(false);
    }

    private void PrintChat(object[] ps)
    {
        if (ps[0] == null)
        {
            return;
        }

        Debug.Log("saveChatCount = " + saveChatCount + '\n' + "ChattingCount = " + ChattingCount);

        if(ps[0] is string)
        {
            string msg = (ps[0] as string);
            CreateTextUI(msg);
        }

        if(ps[0] is Sprite)
        {
            Sprite sprite = ps[0] as Sprite;
            float sizeY = 0;
            if (ps.Length == 2)
            {
                sizeY = (float)ps[1];
            }

            if (sizeY == 0)
                CreateImageUI(sprite);
            else
                CreateImageUI(sprite, sizeY);
        }

    }

    private void AddSaveTexts()
    {
        List<AIChat> list = DataManager.Inst.SaveData.aiChattingList;

        foreach (AIChat data in list)
        {
            if(data.sprite == null && data.text != null)
            {
                CreateTextUI(data.text);
            }
            else if(data.sprite != null)
            {
                CreateImageUI(data.sprite);
            }
        }
    }

    private void CheckNewChatting()
    {
        if (saveChatCount == ChattingCount) //새로온 채팅인데
        {
            if (!newImagePrefab.gameObject.activeSelf)
            {
                ActiveNewImageUI(true);
            }
        }
    }

    private TMP_Text CreateTextUI(string msg)
    {
        CheckNewChatting();

        TMP_Text textUI = Instantiate(textPrefab, textParent);
        textUI.text = msg;

        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(textUI.rectTransform);
        SetScrollView();
        textUI.gameObject.SetActive(true);
        SetLastWidth();

        saveChatCount = ChattingCount;

        return textUI;
    }


    private void CreateImageUI(Sprite sprite, float YSize = 100)
    {
        CheckNewChatting();

        GameObject imageUI = Instantiate(imagePrefab, textParent); //이미지 프리팹 생성

        Image image = imageUI.transform.GetChild(0).GetComponent<Image>(); //이미지 컴포넌트 가져오고

        float x = sprite.bounds.size.x;
        float y = sprite.bounds.size.y;

        float remain = YSize / x;

        float spriteY = y * remain;

        Vector2 size = new Vector2(YSize, spriteY);
        Debug.Log("Remain값 = " + remain + "   size 값 = " + size);

        image.GetComponent<RectTransform>().sizeDelta = size; //크기 맞춰주고

        image.sprite = sprite; //스프라이트 변경

        RectTransform imageRect = imageUI.GetComponent<RectTransform>(); //자식의 이미지 크기랑 height랑 같게함
        imageRect.sizeDelta = new Vector2(imageRect.sizeDelta.x, spriteY);

        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(imageRect);
        SetScrollView();
        imageUI.gameObject.SetActive(true);
        SetLastWidth();

        saveChatCount = ChattingCount;
    }

    private void ActiveNewImageUI(bool flag)
    {
        newImagePrefab.transform.SetAsLastSibling();
        Debug.Log("asdf");
        newImagePrefab.gameObject.SetActive(flag);
        SetScrollView();
        LayoutRebuilder.ForceRebuildLayoutImmediate(((RectTransform)newImagePrefab.transform));
    }

    protected void SetScrollView()
    {
        if (this.gameObject.activeInHierarchy)
        {
            StartCoroutine(ScrollCor());
        }
        else
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
            scroll.verticalNormalizedPosition = 0;
        }
    }

    protected IEnumerator ScrollCor()
    {
        yield return new WaitForSeconds(0.025f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        scroll.verticalNormalizedPosition = 0;
    }

    protected void SetLastWidth()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        if(rects[rects.Length - 1].gameObject.GetComponent<Image>() == null)
        {
            rects[rects.Length - 1].sizeDelta = new Vector2(currentValue - 60, 0);
        }
    }


    protected void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.ProfilerSendMessage, PrintChat);
    }
}
