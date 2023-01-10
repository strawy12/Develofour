using DG.Tweening;
using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class BodyGuard : AnimationObject
{
    [Header("Move")]
    [SerializeField]
    private float movePosX = 0f;
    [SerializeField]
    private float moveDuration = 1f;

    [Header("RedEye")]
    [SerializeField]
    private Image redEyeImage;
    [SerializeField]
    private float redEyeDuration;

    [Header("ChangeColor")]
    [SerializeField]
    private Color changeColor;
    [SerializeField]
    private float changeDuration;

    public IEnumerator Move()
    {
        (transform as RectTransform).DOAnchorPosX(movePosX, moveDuration);

        yield return new WaitForSeconds(moveDuration);
    }

    public float ShowRedEye()
    {
        redEyeImage.DOFade(1f, redEyeDuration);
        return redEyeDuration;
    }

    public float ChangeColorRed()
    {
        redEyeImage.DOFade(0f, changeDuration / 2f);
        iconImage.DOColor(changeColor, changeDuration);
        return changeDuration;
    }
}
