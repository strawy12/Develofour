using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
    private BodyGuard bodyGuard;
    [SerializeField]
    private float bodyGuardRedEyeDelay = 10f;

    [SerializeField]
    private CanvasGroup questions;



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
        // 선딜레이
        yield return new WaitForSeconds(3f);


        canvasGroup.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);

        // 집 등장
        yield return house.ShowAnimation();

        // 카메라 무브
        float duration = cameraMove.Move();
        yield return new WaitForSeconds(duration);

        // 화면 전환
        house.Hide(false);
        cameraMove.ResetCam();

        // 연예인 A,B 등장
        yield return entertainers.ShowAnimation();

        // delay
        yield return new WaitForSeconds(4.5f);
        yield return entertainers.Move();

        // 경호원 등장
        yield return bodyGuard.ShowAnimation();

        duration = xObject.DrawAnim();
        yield return new WaitForSeconds(duration);

        // Delay
        yield return new WaitForSeconds(2f);
        xObject.EraseAnim();

        yield return entertainers.HideAnimation();

        yield return bodyGuard.Move();

        // 딜레이
        yield return new WaitForSeconds(2f);
        questions.DOFade(1f, 1f);
        yield return new WaitForSeconds(bodyGuardRedEyeDelay);

        duration = cameraMove.Move();
        yield return new WaitForSeconds(duration);

        duration = bodyGuard.ShowRedEye();
        yield return new WaitForSeconds(duration);

        yield return new WaitForSeconds(2f);

        //cameraMove.ResetCam();
        //yield return new WaitForSeconds(1f);
        duration = cameraMove.Move();
        yield return new WaitForSeconds(duration);

        duration = bodyGuard.ChangeColorRed();
        yield return new WaitForSeconds(duration);
        yield return new WaitForSeconds(1f);

        canvasGroup.DOFade(0f, 1f);
    }
}
