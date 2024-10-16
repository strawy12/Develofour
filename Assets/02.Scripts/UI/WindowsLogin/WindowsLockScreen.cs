﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class WindowsLockScreen : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField]
    private WindowsLoginScreen loginScreen;

    [SerializeField]
    private float targetMovementY;
    [SerializeField]
    private float offset;

    private float beginPosY;
    private Vector3 originPos;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    //저장기능에 꼭 넣어야함
    private bool isTutorialEnd;
    private bool holdingDown;
    private bool anyKeyUp;
    [SerializeField]
    private GameObject loginCanvas;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = transform as RectTransform;
        originPos = rectTransform.anchoredPosition;
    }

    public void Init()
    {
        if(!DataManager.Inst.SaveData.isWatchStartCutScene)
        {
            EventManager.StartListening(ECutSceneEvent.EndStartCutScene, AnyKeyUp);
            EventManager.StartListening(ECutSceneEvent.EndStartCutScene, TurnInteractable);
        }else
        {
            AnyKeyUp(null);
            TurnInteractable(null);
        }
        isTutorialEnd = DataManager.Inst.SaveData.isClearStartCutScene;
    }
    public void LockReset()
    {
        gameObject.SetActive(true);
        loginScreen.gameObject.SetActive(false);
        rectTransform.anchoredPosition = originPos;
        anyKeyUp = false;
        loginCanvas.SetActive(true);
        Init();
    }
    private void AnyKeyUp(object[] ps)
    {
        StartCoroutine(KeyUpCor());
    }

    IEnumerator KeyUpCor()
    {
        yield return new WaitForSeconds(0.1f);
        InputManager.Inst.AddAnyKeyInput(onKeyUp: AnyKeyUp);
        EventManager.StopListening(ECutSceneEvent.EndStartCutScene, AnyKeyUp);
    }

    private void AnyKeyUp()
    {
        if (anyKeyUp) return;
        anyKeyUp = true;
        rectTransform.DOAnchorPos(originPos + Vector3.up * targetMovementY, 0.1f).OnComplete(OpenLoginScreen);
    }

    private void TurnInteractable(object[] ps)
    {
        isTutorialEnd = true;
        DataManager.Inst.SaveData.isClearStartCutScene = true; 
        EventManager.StopListening(ECutSceneEvent.EndStartCutScene, TurnInteractable);
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isTutorialEnd) return;
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

        if (movementY + offset >= targetMovementY)
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
        loginScreen.LoginReset();
        MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.WINDOWS_LOGIN_SCREEN_OPEN, true);
        gameObject.SetActive(false);
    }
}
