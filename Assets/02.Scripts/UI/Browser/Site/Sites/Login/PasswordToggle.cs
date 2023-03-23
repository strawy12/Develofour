using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PasswordToggle : MonoBehaviour
{
    private Toggle toggle;

    [SerializeField]
    private Image backgroundImage;
    [SerializeField]
    private RectTransform checkIconRectTrm;

    [SerializeField]
    private float colorDuration = 0.25f;

    [SerializeField]
    private float checkDuration = 0.25f;

    public UnityEvent<bool> onValueChanged
    {
        get
        {
            toggle ??= GetComponent<Toggle>();
            return toggle.onValueChanged;
        }
    }

    private Sequence seq = null;

    private void Awake()
    {
        toggle ??= GetComponent<Toggle>();
    }

    private void Start()
    {
        toggle.onValueChanged.AddListener(OnValueChanged);
    }

    public void OnValueChanged(bool isOn)
    {
        if(seq != null)
        {
            seq.Kill(true);
        }

        seq = DOTween.Sequence();
        seq.Append(backgroundImage.DOColor(isOn ? Color.black : Color.white, colorDuration));
        seq.Append(checkIconRectTrm.DOSizeDelta(isOn ? new Vector2(25f, 25f) : new Vector2(0f, 25f), checkDuration));
    }

}
