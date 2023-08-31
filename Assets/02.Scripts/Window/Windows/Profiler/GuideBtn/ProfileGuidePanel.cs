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
    private ProfilerGuideButtonParent guideParent;
    public void Init()
    {
        guideParent.Init();
    }

    #region 이동관련
    protected void HidePanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        guideParent.SetActiveBtn(false);

        EventManager.TriggerEvent(EProfilerEvent.ClickGuideToggleButton, new object[] { false });

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

        EventManager.TriggerEvent(EProfilerEvent.ClickGuideToggleButton, new object[] { true });

        movePanelRect.DOSizeDelta(new Vector2(0, showValue), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = showValue;
            moveButton.onClick.RemoveAllListeners();
            moveButton.onClick.AddListener(HidePanel);
            showImage.SetActive(false);
            hideImage.SetActive(true);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
    }

    public void SetGuideParentWeight(bool value)
    {
        guideParent.isWeightSizeUp = value;
    }

    private void GuideMoveButton(object[] ps)
    {
        if (currentValue == hideValue)
        {
            GuideUISystem.OnGuide?.Invoke((RectTransform)moveButton.transform);
        }
    }
    #endregion
}
