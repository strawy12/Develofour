using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingIcon : MonoBehaviour
{
    public void StartLoading(float lodingDelay, Action callBack = null)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
        }

        RotationLoading(lodingDelay, callBack);
    }

    private void RotationLoading(float lodingDelay, Action callBack)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotate(new Vector3(0, 0, -360), lodingDelay, RotateMode.FastBeyond360));
        seq.AppendCallback(() => callBack?.Invoke());

    }

    public void StopLoading()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);

            transform.rotation = Quaternion.Euler(0, 0, 0);
            DOTween.Kill(transform);
        }
    }
}
