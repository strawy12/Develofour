using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public void StartLoading(float lodingDelay)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);

            RotationLoading(lodingDelay);
        }
    }

    private void RotationLoading(float lodingDelay)
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(transform.DORotate(new Vector3(0, 0, -360), lodingDelay, RotateMode.FastBeyond360));
     
    }

    public void StopLoading()
    {
        if(gameObject.activeSelf)
        {
            gameObject.SetActive(false);

            transform.rotation = Quaternion.Euler(0, 0, 0);
            DOTween.Kill(transform);
        }
    }
}
