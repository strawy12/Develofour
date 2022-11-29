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
        if (isCheck) // üũ�� ���� �Ķ�
        {
            checkMarkCircle.color = checkingColor;
            Color changeAlpha = checkMarkCircle.color;
            changeAlpha.a -= 0.5f;

            checkMarkCircle.color = changeAlpha;
        }
        else if (!isCheck) // üũ �� ���� ���� ������ �� ���� ȸ���� ���ۿ� ����
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