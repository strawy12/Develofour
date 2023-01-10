using DG.Tweening;
using ExtenstionMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class NewsBackground : MonoBehaviour
{
    public enum EBackgroundType
    {
        None = -1,
        AI_Regulation = 0,
        AI_MurderCase,
        Regulation_Initiative
    }

    [SerializeField]
    private List<Image> backgroundImageList;

    private EBackgroundType currentType = EBackgroundType.None;

    [SerializeField]
    private float turnOnDuration;

    [SerializeField]
    private float changeDelay;

    [SerializeField]
    private float turnOffDuration;

    public float ChangeBackground(EBackgroundType type, bool useEffect)
    {
        if (currentType == type) { return 0f; }

        if (currentType == EBackgroundType.None)
        {
            currentType = type;

            if (useEffect)
            {
                backgroundImageList[(int)type].DOFade(1f, turnOnDuration);
                return turnOnDuration;
            }
            else
            {
                backgroundImageList[(int)type].ChangeImageAlpha(1f);

                return 0f;
            }

        }

        if (useEffect)
        {
            StartCoroutine(ChangeCoroutine(type));
            return turnOnDuration + changeDelay;
        }
        else
        {
            backgroundImageList[(int)type].ChangeImageAlpha(1f);
            backgroundImageList[(int)currentType].ChangeImageAlpha(0f);
            currentType = type;
            return 0f;
        }

    }

    private IEnumerator ChangeCoroutine(EBackgroundType type)
    {
        backgroundImageList[(int)currentType].DOFade(0f, turnOffDuration);
        yield return new WaitForSeconds(changeDelay);
        backgroundImageList[(int)type].DOFade(1f, turnOnDuration);
        yield return new WaitForSeconds(turnOnDuration);
        currentType = type;
    }
}
