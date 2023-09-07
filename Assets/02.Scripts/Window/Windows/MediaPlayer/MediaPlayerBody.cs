using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class TextTriggerData
{
#if UNITY_EDITOR
    [Header("메모용")]
    [SerializeField]
    public string text;
#endif

    [Header("사용 변수")]
    public int id;
    public InformationTrigger trigger;
}

public class MediaPlayerBody : MonoBehaviour
{
    [SerializeField] private TMP_Text _mediaDetailText;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private RectTransform _coverRectTrm;

    [SerializeField] private List<TextTriggerData> mediaPlayerTriggerList;

    public TMP_Text mediaDetailText => _mediaDetailText;
    public ScrollRect scroll => _scroll;

    public RectTransform coverRectTrm => _coverRectTrm;

    [SerializeField]
    private InformationTrigger endMediaInfoTrigger;


    private MediaPlayer ownerPlayer; 


#if UNITY_EDITOR
    [ContextMenu("DebugTool")]
    public void DebugTool()
    {
        Init(null);
        _mediaDetailText.text = _mediaDetailText.text.Replace("\r", "");
        foreach (TextTriggerData data in mediaPlayerTriggerList)
        {
            Debug.Log($"{data.text}의 위치는 {_mediaDetailText.text.IndexOf(data.text)}입니다");
        }
    }
    public void SetTriggerText()
    {
        Init(null);

        foreach (TextTriggerData data in mediaPlayerTriggerList)
        {
            data.id = mediaDetailText.text.IndexOf(data.text);
        }
    }
    public void AddTextTriggerData(TextTriggerData textData)
    {
        mediaPlayerTriggerList.Add(textData);
    }
    public void ClearTextTrigger()
    {
        mediaPlayerTriggerList.Clear();
    }
#endif
    public void ShowTrigger(int showIdx)
    {
        foreach(var triggerData in mediaPlayerTriggerList)
        {
            if(showIdx > triggerData.id)
            {
                triggerData.trigger.gameObject.SetActive(true);
            }
            else
            {
                triggerData.trigger.gameObject.SetActive(false);
            }
        }
    } 
    public void Init(MediaPlayer player)
    {
        _mediaDetailText ??= transform.Find("Viewport/MediaDetailText").GetComponent<TMP_Text>();
        _scroll ??= GetComponent<ScrollRect>();

        if (_coverRectTrm == null)
            _coverRectTrm = (mediaDetailText.transform.Find("CoverImage") as RectTransform);

        ownerPlayer = player;
        if (ownerPlayer != null)
        {
            if (EndMediaInfoFlag())
            {
                ownerPlayer.OnEnd += EndMediaTrigger;
            }
        }
    }

    public void SetPosition()
    {
        int idx = mediaDetailText.maxVisibleCharacters;

        Define.SetTriggerPosition(mediaDetailText, mediaPlayerTriggerList);
        
        TMP_CharacterInfo charInfo = mediaDetailText.textInfo.characterInfo[Mathf.Min(idx, mediaDetailText.textInfo.characterInfo.Length - 1)];
        SetPositionCoverImage(charInfo);
    }

    public void SetPositionCoverImage(TMP_CharacterInfo charInfo)
    {
        Vector2 coverSize = coverRectTrm.sizeDelta;
        coverSize.y = mediaDetailText.rectTransform.rect.height + charInfo.bottomRight.y;
        coverRectTrm.sizeDelta = coverSize;
    }

    public void EndMediaTrigger()
    {
        if (EndMediaInfoFlag())
        {
            endMediaInfoTrigger.GetInfo();
            for(int i = 0; i < endMediaInfoTrigger.infoDataIDList.Count; i++)
            {
                ProfileOverlaySystem.OnAdd?.Invoke(endMediaInfoTrigger.infoDataIDList[i]);
            }

            if(ownerPlayer != null)
                ownerPlayer.OnEnd -= EndMediaTrigger;
        }
    }

    public bool EndMediaInfoFlag()
    {
        return (endMediaInfoTrigger != null) && (!DataManager.Inst.IsMonologShow(endMediaInfoTrigger.MonologID));
    }

    private void Reset()
    {
        Init(null);
    }
}
