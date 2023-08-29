using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    [SerializeField]
    private TMP_Text guideText; 

    [Header("가이드 관련")]
    [SerializeField]
    private ProfilerGuideButtonParent guideParent;
    [SerializeField]
    private List<ProfilerGuideDataSO> guideDataList;
    public void Init()
    {
        //세이브에서 isAdd가 true인 놈들만 가져오기
        guideDataList.Clear();
        foreach(var guide in DataManager.Inst.SaveData.profilerGuideData)
        {
            if(guide.isAdd == true)
            {
                guideDataList.Add(ResourceManager.Inst.GetProfilerGuideDataSO(guide.guideName));
            }
        }

        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        //스크롤뷰 가장 밑으로 내리기;
        moveButton.onClick.AddListener(ShowPanel);
        movePanelRect = GetComponent<RectTransform>();
        guideParent.Init(guideDataList);
        guideParent.OnClickGuideButton += ShowPanel;
        EventManager.StartListening(EGuideButtonTutorialEvent.GuideMoveBtn, GuideMoveButton);
        SetGuideParentWeight(true);

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
            guideText.gameObject.SetActive(false);
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
            guideText.gameObject.SetActive(true);
            guideParent.UpdateButton();
        });
    }

    public void SetGuideParentWeight(bool value)
    {
        guideParent.isWeightSizeUp = value;
        guideParent.UpdateButton();
    }

    private void GuideMoveButton(object[] ps)
    {
        GuideUISystem.OnEndAllGuide?.Invoke();

        if (currentValue == hideValue)
        {
            GuideUISystem.OnGuide?.Invoke((RectTransform)moveButton.transform);
        }
    }
    #endregion

}
