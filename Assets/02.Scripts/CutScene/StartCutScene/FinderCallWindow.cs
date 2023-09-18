using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FinderCallWindow : MonoBehaviour
{
    private RectTransform _rectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (_rectTransform == null)
            {
                _rectTransform = transform as RectTransform;
            }

            return _rectTransform;
        }
    }

    [SerializeField]
    private RectTransform myCallBox;
    [SerializeField]
    private RectTransform otherCallBox;
    [SerializeField]
    private TMP_Text otherNameText;
    [SerializeField]
    private Transform otherCharacterParent;
    [SerializeField]
    private GameObject myTalkGuideUI;
    [SerializeField]
    private GameObject otherTalkGuideUI;

    private bool isPlayEffect = false;
    public int ignoreTextGuideCnt = 0;

    private Coroutine hideTalkGuideUICoroutine;

    private CharacterAnimator otherCharacter;
    public void Show()
    {
        if (gameObject.activeSelf) return;
        EventManager.StartListening(ETextboxEvent.StartPrintText, StartTalk);
        EventManager.StartListening(ETextboxEvent.EndPrintText, EndTalk);
        SizeDoTween();
    }

    public void DeleteCharacter()
    {
        if (otherCallBox.gameObject.activeSelf)
        {
            StartCoroutine(DeleteCharacterEffect(null));
        }
    }

    public void AddChacter(ECharacterType type, Action callback)
    {
        if (otherCallBox.gameObject.activeSelf)
        {
            StartCoroutine(DeleteCharacterEffect(() =>
            {
                AddChacter(type, callback);
            }));

            return;
        }

        CharacterAnimator temp = ResourceManager.Inst.GetCharacterPrefab(type);
        otherCharacter = Instantiate(temp, otherCharacterParent);
        otherCharacter.transform.localScale = Vector3.one * 0.25f;
        ((RectTransform)otherCharacter.transform).anchoredPosition = new Vector2(0f, -50f);

        if (type == ECharacterType.Assistant)
            otherNameText.text = "조수";
        else
            otherNameText.text = "형사";

        if (!otherCallBox.gameObject.activeSelf)
        {
            StartCoroutine(AddCharacterEffect(callback));
        }
        else
        {
            callback?.Invoke();
            otherCharacter.PlayBlink();
        }
    }

    private IEnumerator AddCharacterEffect(Action callback)
    {
        yield return new WaitUntil(() => !isPlayEffect);
        isPlayEffect = true;

        otherCallBox.transform.localScale = Vector3.zero;
        otherCallBox.gameObject.SetActive(true);
        myCallBox.DOAnchorPosX(200f, 0.7f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.3f);
        otherCallBox.anchoredPosition = new Vector2(-200f, 0f);
        otherCallBox.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCubic);

        yield return new WaitForSeconds(0.5f);
        callback?.Invoke();
        otherCharacter.PlayBlink();
        isPlayEffect = false;
    }

    private IEnumerator DeleteCharacterEffect(Action callback)
    {
        yield return new WaitUntil(() => !isPlayEffect);
        isPlayEffect = true;

        otherCallBox.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.3f);
        myCallBox.DOAnchorPosX(0f, 0.7f).SetEase(Ease.OutCubic);
        yield return new WaitForSeconds(0.2f);
        otherCallBox.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Destroy(otherCharacter.gameObject);
        callback?.Invoke();
        isPlayEffect = false;
    }

    private void StartTalk(object[] ps)
    {
        if (otherCharacter == null) return;
        if (ps.Length == 0) return;

        Color color = (Color)ps[0];

        if (hideTalkGuideUICoroutine != null)
        {
            StopCoroutine(hideTalkGuideUICoroutine);
            hideTalkGuideUICoroutine = null;
        }

        if (color != Color.white)
        {
            otherCharacter.PlayTalk();

            myTalkGuideUI.SetActive(false);
            if (ignoreTextGuideCnt >0)
            {
                ignoreTextGuideCnt--;
            }
            otherTalkGuideUI.SetActive(true);
        }

        else
        {
            otherTalkGuideUI.SetActive(false);
            if (ignoreTextGuideCnt > 0)
            {
                ignoreTextGuideCnt--;
            } 
            myTalkGuideUI.SetActive(true);
        }
    }

    private void EndTalk(object[] ps)
    {
        if (otherCharacter != null)
        {
            otherCharacter.StopTalk();
        }

        if (hideTalkGuideUICoroutine != null)
        {
            StopCoroutine(hideTalkGuideUICoroutine);
            hideTalkGuideUICoroutine = null;
        }

        hideTalkGuideUICoroutine = StartCoroutine(HideTalkGuideUI());
    }

    private IEnumerator HideTalkGuideUI()
    {
        yield return new WaitForSeconds(1f);
        myTalkGuideUI.SetActive(false);
        otherTalkGuideUI.SetActive(false);
        hideTalkGuideUICoroutine = null;
    }

    private void SizeDoTween()
    {
        float minDuration = 0.16f;
        transform.localScale = new Vector2(0.9f, 0.9f);
        gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(transform.DOScale(1, minDuration));
    }
}
