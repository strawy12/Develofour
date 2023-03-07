using Coffee.UIEffects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideSystem : MonoBehaviour
{
    public static Action<int> OnGuideCreateionTimer;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        OnGuideCreateionTimer += TimerGuide;
    }

    private void TimerGuide(int timer)
    {

    }

}
