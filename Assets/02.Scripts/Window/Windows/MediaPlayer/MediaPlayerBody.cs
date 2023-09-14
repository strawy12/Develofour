using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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
    public int startIdx;
    public int endIdx;
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
            data.startIdx = mediaDetailText.text.IndexOf(data.text);
            data.trigger.isHide = true;
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
    public void CheckShowTrigger(int showIdx)
    {
        foreach (var trigger in mediaPlayerTriggerList)
        {
            if (showIdx > trigger.startIdx)
            {
                trigger.trigger.isHide = false;
            }
            else
            {
                trigger.trigger.isHide = true;
            }
        }
    }
    [ContextMenu("posDebug")]
    public void SetPosition()
    {
        int idx = _mediaDetailText.maxVisibleCharacters;
        _mediaDetailText.ForceMeshUpdate();
        Define.SetTriggerPosition(_mediaDetailText, mediaPlayerTriggerList);

        TMP_CharacterInfo charInfo = _mediaDetailText.textInfo.characterInfo[Mathf.Min(idx, _mediaDetailText.textInfo.characterInfo.Length - 1)];
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

            if (ownerPlayer != null)
                ownerPlayer.OnEnd -= EndMediaTrigger;
        }
    }

    public bool EndMediaInfoFlag()
    {
        return (endMediaInfoTrigger != null) && (!DataManager.Inst.IsMonologShow(endMediaInfoTrigger.TriggerData.monoLogType));
    }

    private void Reset()
    {
        Init(null);
    }
}
