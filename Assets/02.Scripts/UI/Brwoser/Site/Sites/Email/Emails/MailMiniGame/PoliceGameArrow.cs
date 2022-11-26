using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using DG.Tweening;
public enum EMiniGameArrowType
{
    UP,
    Down,
    Right,
    Left,
    End,
} 

public class PoliceGameArrow : MonoBehaviour
{
    [SerializeField]
    private List<Sprite> arrowSprite = new List<Sprite>();

    [SerializeField]
    private Image arrowImage;
    public Action<PoliceGameArrow> OnPush;
    private int arrowType;
    private KeyCode answerKey;

    public KeyCode AnswerKey
    {
        get
        {
            return answerKey;
        }
    }

    private bool isInputed = false;

    public bool IsInputed { get { return isInputed; } }

    public void ResetObject()
    {
        arrowImage.DOFade(1, 0);
        arrowImage.rectTransform.DOScale(1, 0);
    }

    public void Init()
    {
        isInputed = false;
        ArrowRandomSetting();
    }

    public void Release()
    {
    }

    private void ArrowRandomSetting()
    {
        isInputed = false;
        arrowType = Random.Range((int)EMiniGameArrowType.UP, (int)EMiniGameArrowType.End);
        arrowImage.sprite = arrowSprite[arrowType];

        SettingArrowInput();
    }

    private void SettingArrowInput()
    {
        switch (arrowType)
        {
            case (int)EMiniGameArrowType.UP:
                answerKey = KeyCode.UpArrow;
                break;
            case (int)EMiniGameArrowType.Down:
                answerKey = KeyCode.DownArrow;
                break;
            case (int)EMiniGameArrowType.Right:
                answerKey = KeyCode.RightArrow;
                break;
            case (int)EMiniGameArrowType.Left:
                answerKey = KeyCode.LeftArrow;
                break;
            case (int)EMiniGameArrowType.End:
                Debug.Log("Random Index Out Error");
                break;
        }
    }

    public void Fail()
    {
        isInputed = true; 
        arrowImage.rectTransform.DOShakePosition(1f, 10f, 30).OnComplete(() => { isInputed = false; });
    }

    public void Succcess()
    {
        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.PoliceMinigameArrowSuccess);
        Sequence seq = DOTween.Sequence();
        seq.Append(arrowImage.rectTransform.DOScale(0.6f, 0.075f));
        seq.Join(arrowImage.DOFade(0.1f, 0.175f));
        seq.Append(arrowImage.rectTransform.DOScale(1.3f, 0.1f))
            .OnComplete(() => { gameObject.SetActive(false); OnPush.Invoke(this); });     
        Pop();
    }

    public void Pop()
    {
        gameObject.SetActive(false);
        OnPush.Invoke(this);
    }
}
