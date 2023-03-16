using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CdPlayMedia : MonoBehaviour
{
    [SerializeField]
    private Image cd;

    public void Init()
    {
        PlayCdAnimation();
    }

    public void PlayCdAnimation()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(cd.transform.DORotate(new Vector3(0, 0, -360), 99999f, RotateMode.FastBeyond360));
    }

    public void StopCdAnimation()
    {
        cd.transform.rotation = Quaternion.Euler(0, 0, 0);
        DOTween.Kill(cd.transform);
    }

}
