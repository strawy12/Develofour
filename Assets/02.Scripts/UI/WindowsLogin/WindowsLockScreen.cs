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
    private float targetMovementY;
    [SerializeField]
    private float offset;

    private float beginPosY;
    private Vector3 originPos;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private bool holdingDown;
    private bool anyKeyUp;
    private bool isDrag;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = transform as RectTransform;
        originPos = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        if (anyKeyUp) return;
        if (Input.anyKey)
        {
            holdingDown = true;
        }

        if (!Input.anyKey && holdingDown)
        {
            holdingDown = false;

            if (isDrag)
            {
                isDrag = false;
                return;
            }

            AnyKeyUp();
        }
    }

    private void AnyKeyUp()
    {

        anyKeyUp = true;
        rectTransform.DOAnchorPos(originPos + Vector3.up * targetMovementY, 0.1f).OnComplete(OpenLoginScreen);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        beginPosY = eventData.position.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (anyKeyUp) return;
        isDrag = true;

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
        gameObject.SetActive(false);
    }

}
