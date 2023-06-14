using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class CreditPanel : MonoBehaviour
{
    private RectTransform contents;

    private ScrollRect scrollRect;
    [SerializeField]
    private float duration = 5f;
    [SerializeField]
    private float startDelay = 2f;
    [SerializeField]
    private float endDelay = 2f;
    private void Start()
    {
        Init();        
    }

    private void Init()
    {
        scrollRect ??= GetComponent<ScrollRect>();
        contents = scrollRect.content;
    }

    public void StartCredit()
    {
        Init();
        scrollRect.verticalNormalizedPosition = 1f;
        gameObject.SetActive(true);
        InputManager.Inst.AddKeyInput(KeyCode.Escape, SkipCredit);

        StartCoroutine(StartCreditCoroutine());
    }

    private IEnumerator StartCreditCoroutine()
    {
        yield return new WaitForSeconds(startDelay);
        DownCredit();
    }
    private void DownCredit()
    {
        Sequence sequence = DOTween.Sequence();
        sequence.Append(scrollRect.DOVerticalNormalizedPos(0f, duration));
        sequence.AppendCallback(() => { StartCoroutine(EndCreditCoroutine()); });
    }
    private IEnumerator EndCreditCoroutine()
    {
        yield return new WaitForSeconds(endDelay);
        SkipCredit();
    }

    private void SkipCredit()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        CreditReset();
    }

    private void CreditReset()
    {
        DOTween.KillAll();
        scrollRect.verticalNormalizedPosition = 1f;
        InputManager.Inst.RemoveKeyInput(KeyCode.Escape, SkipCredit);
    }
}
