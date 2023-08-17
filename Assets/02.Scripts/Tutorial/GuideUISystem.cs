﻿using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;
public class GuideUISystem : MonoBehaviour
{
    [SerializeField]
    private GuideUI guideUITemp;

    private Dictionary<RectTransform, GuideUI> guideUIList;
    private Stack<GuideUI> guideUIPool;


    public static Action<RectTransform> OnGuide;
    public static Action OnEndAllGuide;
    public static Action<RectTransform> OnEndGuide;
    public static Action<RectTransform> OnFullSizeGuide;
    public static Action<RectTransform> OnCenterSizeGuide;


    private void Start()
    {
        guideUIList = new Dictionary<RectTransform, GuideUI>();
        guideUIPool = new Stack<GuideUI>();

        OnGuide += StartGuide;
        OnEndAllGuide += StopGuideUICor;
        OnEndGuide += StopGuide;
        OnFullSizeGuide += ChangeFullSize;
        OnCenterSizeGuide += ChangeCenterSize;
    }

    private GuideUI GetGuideUI(RectTransform parent)
    {
        if (guideUIList.ContainsKey(parent))
        {
            return null;
        }

        GuideUI guideUI = null;
        if (!guideUIPool.TryPeek(out guideUI))
        {
            guideUI = Instantiate(guideUITemp, parent);
            guideUI.OnObjectDestroy += DestroyObject;
        }
        Debug.Log($"============={guideUI}_{parent}==============");
        guideUI.transform.SetParent(parent);
        guideUI.transform.SetAsFirstSibling();

        guideUIList.Add(parent, guideUI);

        return guideUI;
    }


    private void StartGuide(RectTransform rect)
    {
        if (rect == null) return;
        if (!rect.gameObject.activeSelf) return;
        GuideUI ui = GetGuideUI(rect);
        if (ui == null) return;

        ui.Show(rect);
    }

    private void StopGuideUICor()
    {
        foreach (GuideUI guideUI in guideUIList.Values)
        {
            guideUI.Hide();
            guideUIPool.Push(guideUI);
        }

        guideUIList.Clear();
    }

    private void StopGuide(RectTransform rectTrm)
    {
        if (guideUIList.TryGetValue(rectTrm, out GuideUI ui))
        {
            ui.Hide();
            guideUIPool.Push(ui);
        }
    }

    private void ChangeFullSize(RectTransform rect)
    {
        if (guideUIList.TryGetValue(rect, out GuideUI ui))
        {
            ui.ChangeFullSize();
        }
    }

    private void ChangeCenterSize(RectTransform rect)
    {
        if (guideUIList.TryGetValue(rect, out GuideUI ui))
        {
            ui.ChangeCenterSize();
        }
    }

    private void DestroyObject(GuideUI ui)
    {
        if (guideUIList == null) return;
        guideUIList.Remove(ui.targetRectTrm);
        if(guideUIPool.Contains(ui))
        {
            var list = guideUIPool.ToList();
            guideUIPool.Clear();

            foreach(var guideUI in list)
            {
                if(guideUI != ui)
                {
                    guideUIPool.Push(guideUI);
                }
            }
        }
    }
}