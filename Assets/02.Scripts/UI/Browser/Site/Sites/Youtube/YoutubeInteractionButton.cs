//using DG.Tweening;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Security.Cryptography;
//using TMPro;
using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.UI;

public class YoutubeInteractionButton : MonoBehaviour { }
//{
//    private static Action<bool> OnTriggerInteraction;

//    [SerializeField] private bool isHateBtn;

//    private bool isClicked = false;
//    private bool isEffecting = false;

//    private float durationTime = 0.4f;

//    [SerializeField]
//    private TMP_Text peopleText;

//    private Button button;
//    private Image image;
//    private RectTransform rectTransform;

//    private Vector3 originScale;

//    public UnityEvent OnClick { get { return button.onClick; } }

//    private void Awake()
//    {
//        button = GetComponent<Button>();
//        image = GetComponent<Image>();
//        rectTransform = GetComponent<RectTransform>();

//        image.enabled = false;
//        originScale = rectTransform.localScale;

//        button.onClick.AddListener(ClickEffect);
//    }

//    private void Start()
//    {
//        EYoutubeInterationType type = DataManager.Inst.CurrentPlayer.CurrentChapterData.youtubeInterationType;

//        if (type != EYoutubeInterationType.None)
//        {
//            bool flag = type == EYoutubeInterationType.Hate;
//            if (isHateBtn == flag)
//            {
//                ButtonInteraction(isHateBtn);
//            }
//        }

//        OnTriggerInteraction += ButtonInteraction;
//    }

//    private void ButtonInteraction(bool isHateButton)
//    {

//        if (isHateBtn == isHateButton) // 누른 버튼이 같을 때
//        {
//            EYoutubeInterationType type = EYoutubeInterationType.None;
//            if (isClicked)
//            {
//                CancelButton();
//            }
//            else if (!isClicked)
//            {
//                if (isHateBtn)
//                {
//                    SelectHateButton();
//                    type = EYoutubeInterationType.Hate;
//                }
//                else if (!isHateBtn)
//                {
//                    SelectLikeButton();
//                    type = EYoutubeInterationType.Like;
//                }
//            }

//            //DataManager.Inst.CurrentPlayer.CurrentChapterData.youtubeInterationType = type;
//        }
//        else if (isHateBtn != isHateButton) // 다를 때
//        {
//            if (isClicked)
//            {
//                CancelButton();
//            }
//        }

//    }

//    private void ClickEffect()
//    {
//        if (isEffecting)
//        {
//            return;
//        }

//        isEffecting = true;
//        OnTriggerInteraction?.Invoke(isHateBtn);
//    }

//    private void SelectHateButton()
//    {
//        Sequence seq = DOTween.Sequence();
//        isClicked = true;
//        image.enabled = true;

//        //Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.YoutubeHateBtnSound);

//        int num = int.Parse(peopleText.text);
//        num += 1;
//        peopleText.SetText(num.ToString());

//        seq.Append(rectTransform.DOScale(originScale * 1.2f, durationTime));
//        seq.Join(image.DOColor(Color.yellow, durationTime));

//        seq.Join(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x - 1f, durationTime));
//        seq.Join(rectTransform.DORotate(new Vector3(0, 0, 20), durationTime));

//        //after
//        seq.Append(rectTransform.DOScale(originScale, durationTime));

//        seq.Join(rectTransform.DOAnchorPosX(rectTransform.anchoredPosition.x + 1f, durationTime));
//        seq.Join(rectTransform.DORotate(new Vector3(0, 0, 0), durationTime));

//        //end
//        seq.AppendCallback(() =>
//        {
//            isEffecting = false;
//        });
//    }

//    private void SelectLikeButton()
//    {
//        isClicked = true;
//        isEffecting = false;

//        image.enabled = true;
//        image.color = Color.black;
//        //Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.YoutubeDefaultBtnSound);

//    }

//    private void CancelButton()
//    {
//        Sequence seq = DOTween.Sequence();
//        isClicked = false;

//        //Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.YoutubeDefaultBtnSound);

//        if (isHateBtn)
//        {
//            int num = int.Parse(peopleText.text);
//            num -= 1;
//            peopleText.SetText(num.ToString());
//        }

//        image.enabled = false;
//        seq.Join(image.DOColor(Color.white, durationTime));

//        seq.AppendCallback(() =>
//        {
//            isEffecting = false;
//        });
//    }

//    private void OnDestroy()
//    {
//        OnTriggerInteraction -= ButtonInteraction;
//    }
//}
