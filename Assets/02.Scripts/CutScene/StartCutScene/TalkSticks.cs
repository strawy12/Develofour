using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkSticks : MonoBehaviour
{
    [SerializeField]
    private List<RectTransform> sticks;

    private bool isStop =true;

    public void PlayEffect()
    {
        if (!isStop) return;
        isStop = false;
        StartCoroutine(EffectCoroutine());
    }

    public void StopEffect()
    {
        if (isStop) return;
        isStop = true;
        StopAllCoroutines();
        foreach (var stick in sticks)
        {
            stick.DOKill();
            stick.DOSizeDelta(new Vector2(5f, 20f), 0.2f);
        }
    }

    private IEnumerator EffectCoroutine()
    {
        while(!isStop)
        {
            foreach(var stick in sticks)
            {
                stick.DOKill();
                float value = Random.Range(5f, 30f);
                stick.DOSizeDelta(new Vector2(5f, value), 0.3f);
            }

            yield return new WaitForSeconds(0.3f);
        }
    }
}
