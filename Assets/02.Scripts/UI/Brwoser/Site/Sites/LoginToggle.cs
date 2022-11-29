using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class LoginToggle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Check Mark")]
    [SerializeField]
    private Image checkMarkCircle;
    [SerializeField]
    private Color checkingColor;

    private Color defaultColor;

    private void Start()
    {
        defaultColor = checkMarkCircle.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        checkMarkCircle.gameObject.SetActive(true);

        Color changeAlpha = defaultColor;
        changeAlpha.a -= 0.9f;

        checkMarkCircle.color = changeAlpha;
    }

    public void CheckMarkEffect(bool isCheck)
    {
        if (isCheck) // 체크는 연한 파랑
        {
            checkMarkCircle.color = checkingColor;
            Color changeAlpha = checkMarkCircle.color;
            changeAlpha.a -= 0.5f;

            checkMarkCircle.color = changeAlpha;
        }
        else if (!isCheck) // 체크 안 됐을 때는 눌렀을 때 진한 회색의 경우밖에 없음
        {
            checkMarkCircle.color = defaultColor;

            Color changeAlpha = checkMarkCircle.color;
            changeAlpha.a -= 0.4f;

            checkMarkCircle.color = changeAlpha;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Color changeAlpha = defaultColor;
        changeAlpha.a -= 1f;

        checkMarkCircle.color = changeAlpha;
    }
}