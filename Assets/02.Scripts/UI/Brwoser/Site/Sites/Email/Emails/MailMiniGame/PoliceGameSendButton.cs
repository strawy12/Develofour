using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PoliceGameSendButton : MonoBehaviour
{
    
    public void SuccessEffect()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalMoveY(20, 0.2f));
        seq.Join(transform.DOLocalRotate(new Vector3(0, 360, 0), 0.8f));
        seq.Join(transform.DOScale(1.2f, 0.5f)).OnComplete(() => transform.DOScale(1, 0.3f));
        seq.Insert(0.6f, transform.DOLocalMoveX(0, 0.2f));
    }
}
