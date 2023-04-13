using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BranchPostPanel : MonoBehaviour
{
    private BranchPostDataSO postData;

    [SerializeField]
    private TMP_Text titleText;
    [SerializeField]
    private TMP_Text subTitleText;
    [SerializeField]
    private TMP_Text writerText;
    [SerializeField]
    private TMP_Text mainText;

    [SerializeField]
    private Button backBtn;

    [SerializeField]
    private RectTransform contentRect;

    private Action backButtonAction;
    private RectTransform rectTransform;

    public void Init(Action action)
    {
        Bind();

        backButtonAction = action;
        backBtn.onClick.AddListener(ClickBackButton);
    }

    private void Bind()
    {
        rectTransform ??= GetComponent<RectTransform>();
    }

    private void ClickBackButton()
    {
        backButtonAction?.Invoke();
    }

    public void Show(BranchPostDataSO newPostData)
    {
        EventManager.TriggerEvent(EBranchEvent.HideAllPanel);
        gameObject.SetActive(true);
        postData = newPostData;
        titleText.text = postData.wirteTitle;
        subTitleText.text = postData.subTitle;
        writerText.text = $"by.{postData.writerName}";
        mainText.text = postData.mainText;

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);

        rectTransform.sizeDelta = new Vector3(rectTransform.sizeDelta.x, contentRect.sizeDelta.y);
        
    }
}
