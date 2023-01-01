using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CameraMoveInfo
{
    public float fovAmount;
    public Vector2 movePos;
    public float duration;
    public Ease ease;

    public float startDelay;
    public float afterDelay;
}

public class CameraMove : MonoBehaviour
{
    [SerializeField]
    private List<CameraMoveInfo> infoList;
    private Camera mainCam;

    private Vector3 originPos = new Vector3(0, 0, -10f);
    private float originFovAmount = 60;

    private int count = 0;

    private void Awake()
    {
        mainCam = Define.MainCam;
        originPos = mainCam.transform.position;
        originFovAmount = mainCam.fieldOfView;
    }

    [ContextMenu("Move")]
    public float Move()
    {
        mainCam = Define.MainCam;
        CameraMoveInfo info = infoList[count++];

        Vector3 pos = info.movePos;
        pos.z = -10f;

        Sequence seq = DOTween.Sequence(); 
        seq.AppendInterval(info.startDelay);
        seq.Append(mainCam.transform.DOMove(pos, info.duration).SetEase(info.ease));
        seq.Join(mainCam.DOFieldOfView(info.fovAmount, info.duration).SetEase(info.ease));

        return info.startDelay + info.duration + info.afterDelay;
    }


    [ContextMenu("ResetCam")]
    public void ResetCam()
    {
        mainCam = Define.MainCam;
        originPos = new Vector3(0, 0, -10f);
        originFovAmount = 60;

        mainCam.fieldOfView = originFovAmount;
        mainCam.transform.position = originPos;
    }
}
