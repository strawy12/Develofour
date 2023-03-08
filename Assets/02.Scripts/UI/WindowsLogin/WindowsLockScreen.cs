using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WindowsLockScreen : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler 
{
    [SerializeField]
    private GameObject loginScreen;
    [SerializeField]
    private GameObject loginChoice;

    [SerializeField]
    private float targetMovementY;
    [SerializeField]
    private float offset;

    private float beginPosY;
    private Vector3 originPos;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    //저장기능에 꼭 넣어야함
    public bool isTutorialEnd;
    private bool holdingDown;
    private bool anyKeyUp;
    private bool isDrag;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = transform as RectTransform;
        originPos = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        EventManager.StartListening(EInputType.InputAnyKeyUp, AnyKeyUp);
        EventManager.StartListening(ECutSceneEvent.EndStartCutScene, TurnInteractable);
    }

    private void TurnInteractable(object[] ps)
    {
        isTutorialEnd = true;
        EventManager.StopListening(ECutSceneEvent.EndStartCutScene, TurnInteractable);
    }

    private void AnyKeyUp(object[] ps)
    {
        if (anyKeyUp) return;
        if (!isTutorialEnd) return;
        anyKeyUp = true;
        rectTransform.DOAnchorPos(originPos + Vector3.up * targetMovementY, 0.1f).OnComplete(OpenLoginScreen);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isTutorialEnd) return;
        isDrag = true;
        beginPosY = eventData.position.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (anyKeyUp) return;

        float movementY = eventData.position.y - beginPosY;
        if (movementY < 0f) return;
        
        rectTransform.anchoredPosition = originPos + Vector3.up * movementY;
        canvasGroup.alpha = (targetMovementY - movementY) / targetMovementY;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        float movementY = eventData.position.y - beginPosY;

        if(movementY + offset >= targetMovementY)
        {
            OpenLoginScreen();
        }
        else
        {
            rectTransform.DOAnchorPos(originPos, 0.2f);
            canvasGroup.alpha = 1f;
        }

    }

    private void OpenLoginScreen()
    {
        loginScreen.SetActive(true);
        loginChoice.SetActive(true);
        gameObject.SetActive(false);
    }

}
