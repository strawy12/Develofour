using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuidePanel : MonoBehaviour
{

    [Header("움직임 관련")]
    [SerializeField]
    protected Button moveButton;
    [SerializeField]
    protected GameObject showImage;
    [SerializeField]
    protected GameObject hideImage;
    [SerializeField]
    protected float hideValue;
    [SerializeField]
    protected float showValue;

    [SerializeField]
    protected GameObject loadingPanel;

    protected float currentValue;
    [SerializeField]
    protected float moveDuration;
    protected bool isMoving;
    protected RectTransform movePanelRect;

    [Header("가이드 관련")]
    [SerializeField]
    private ProfileGuideButtonParent guideParent;

    public void Init()
    {
        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        //스크롤뷰 가장 밑으로 내리기;
        moveButton.onClick.AddListener(ShowPanel);
        movePanelRect = GetComponent<RectTransform>();
        guideParent.Init();
        guideParent.OnClickGuideButton += HidePanel;
        EventManager.StartListening(EGuideButtonTutorialEvent.GuideMoveBtn, GuideMoveButton);

    }
    #region 이동관련
    protected void HidePanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        guideParent.SetActiveBtn(false);

        movePanelRect.DOSizeDelta(new Vector2(0, hideValue), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = hideValue;
            moveButton.onClick.RemoveAllListeners();
            moveButton.onClick.AddListener(ShowPanel);
            showImage.SetActive(true);
            hideImage.SetActive(false);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
    }

    protected void ShowPanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        guideParent.SetActiveBtn(true);
        movePanelRect.DOSizeDelta(new Vector2(0, showValue), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = showValue;
            moveButton.onClick.RemoveAllListeners();
            moveButton.onClick.AddListener(HidePanel);
            showImage.SetActive(false);
            hideImage.SetActive(true);
            isMoving = false;
            loadingPanel.SetActive(false);
            EventManager.TriggerEvent(EGuideButtonTutorialEvent.ClickMoveBtn);
        });
    }

    public void SetGuideParentWeight(bool value)
    {
        guideParent.isWeightSizeUp = value;
        guideParent.UpdateButton();
    }

    private void GuideMoveButton(object[] ps)
    {
        if (!DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
        {
            return;
        }
        GuideUISystem.EndGuide?.Invoke();
        if (currentValue == hideValue)
        {
            GuideUISystem.OnGuide?.Invoke((RectTransform)moveButton.transform);
        }
        else
        {
            EventManager.TriggerEvent(EGuideButtonTutorialEvent.ClickMoveBtn);
        }
    }

    #endregion

}
