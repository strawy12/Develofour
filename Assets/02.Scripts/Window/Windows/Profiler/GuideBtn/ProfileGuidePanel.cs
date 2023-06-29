using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileGuidePanel : MonoBehaviour
{
    [Header("가이드 관련")]
    [SerializeField]
    private ProfilerGuideButtonParent guideParent;
    public void Init()
    {
        guideParent.Init();
    }
}
