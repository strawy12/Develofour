using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entertainers : MonoBehaviour
{
    [Header("EntertainerA")]
    [SerializeField]
    private AnimationObject entertainerA;

    [Header("EntertainerB")]
    [SerializeField]
    private AnimationObject entertainerB;

    [SerializeField]
    private float textDelay;

    [SerializeField]
    private float movePosX;
    [SerializeField]
    private float moveDuration;

    private RectTransform rectTransform;

    public IEnumerator Move()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.DOAnchorPosX(movePosX, moveDuration);
        yield return new WaitForSeconds(moveDuration);
    }

    public IEnumerator ShowAnimation()
    {
        float imageDuration = entertainerA.ShowImage();
        entertainerB.ShowImage();
        yield return new WaitForSeconds(imageDuration);

        float textDuration = entertainerA.ShowText();
        yield return new WaitForSeconds(textDuration);

        yield return new WaitForSeconds(textDelay);

        textDuration = entertainerB.ShowText();
        yield return new WaitForSeconds(textDuration);
    }

    public IEnumerator HideAnimation()
    {
        float duration = entertainerA.Hide(true);
        entertainerB.Hide(true);

        entertainerA.transform.DOScale(Vector3.zero, duration);
        entertainerB.transform.DOScale(Vector3.zero, duration);

        yield return new WaitForSeconds(duration);
    }
}
