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
    private GameObject coverPanel;

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
    private bool isDrag;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = transform as RectTransform;
        originPos = rectTransform.anchoredPosition;
    }

    private void Start()
    {
        InputManager.Inst.AddAnyKeyInput(onKeyUp: AnyKeyUp);
        EventManager.StartListening(ECutSceneEvent.EndStartCutScene, TurnInteractable);
        isTutorialEnd = DataManager.Inst.SaveData.isClearStartCutScene;
    }

    private void TurnInteractable(object[] ps)
    {
        isTutorialEnd = true;
        DataManager.Inst.SaveData.isClearStartCutScene = true; 
        EventManager.StopListening(ECutSceneEvent.EndStartCutScene, TurnInteractable);
    }

    private void AnyKeyUp()
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
        loginScreen.SetActive(true);

        StartCoroutine(CoverSetting());
        MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.StartMonolog, 0.2f, true);
    }


    public void EndMonolog()
    {
        //GuideManager.OnPlayGuide?.Invoke(EGuideTopicName.GuestLoginGuide, 40);
    }


    public IEnumerator CoverSetting()
    {
        coverPanel.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.26f);
        coverPanel.gameObject.SetActive(false);

        gameObject.SetActive(false);
    }
}
