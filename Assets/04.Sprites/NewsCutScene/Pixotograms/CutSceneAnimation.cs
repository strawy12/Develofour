using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CutSceneAnimation : MonoBehaviour
{
    [SerializeField]
    private XAnim xObject;
    [SerializeField]
    private CameraMove cameraMove;

    [Header("House")]
    [SerializeField]
    private AnimationObject house;

    [Header("Enterainers")]
    [SerializeField]
    private Entertainers entertainers;

    [Header("BodyGuard")]
    [SerializeField]
    private AnimationObject bodyGuard;


    private CanvasGroup canvasGroup;

    
    private bool isInit = false;

    private void Init()
    {
        if (isInit) { return; }

        isInit = true;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    [ContextMenu("Play")]
    public void Play()
    {
        Init();
        StartCoroutine(PlayAnimation());
    }

    private IEnumerator PlayAnimation()
    {
        canvasGroup.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);

        // �� ����
        yield return house.ShowAnimation();

        // ī�޶� ����
        float duration = cameraMove.Move();
        yield return new WaitForSeconds(duration);

        // ȭ�� ��ȯ
        house.Hide();
        cameraMove.ResetCam();

        // ������ A,B ����
        yield return entertainers.PlayAnimation();

        // delay
        yield return new WaitForSeconds(3f);
        yield return entertainers.Move();

        // ��ȣ�� ����
        yield return bodyGuard.ShowAnimation();
    }
}
