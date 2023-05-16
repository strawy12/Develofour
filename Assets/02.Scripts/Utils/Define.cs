﻿using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public static class Define
{
    public static Vector2 MaxWindowSize
    {
        get
        {
            Vector2 maxWIndowSize = new Vector2(Screen.width, Screen.height);
            return maxWIndowSize;
        }
    }

    private static Camera mainCam;

    public static Camera MainCam
    {
        get
        {
            if (mainCam == null)
            {
                mainCam = Camera.main;
            }

            return mainCam;
        }

    }

    public static Vector2 CanvasMousePos
    {
        get
        {
            Vector3 mousePos = Input.mousePosition;
            Vector3 windowSize = MaxWindowSize;
            Vector3 maxCanvasPos = Constant.MAX_CANVAS_POS;
            mousePos.x = Mathf.Lerp(-maxCanvasPos.x, maxCanvasPos.x, mousePos.x / windowSize.x);
            mousePos.y = Mathf.Lerp(-maxCanvasPos.y, maxCanvasPos.y, mousePos.y / windowSize.y);
            mousePos.z = 0f;

            return mousePos;
        }
    }

    private static Transform windowCanvasTrm;

    public static Transform WindowCanvasTrm
    {
        get
        {
            if (windowCanvasTrm == null)
            {
                windowCanvasTrm = GameObject.FindGameObjectWithTag("WindowCanvas").transform;
            }

            return windowCanvasTrm;
        }
    }

    public static void GameQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public static bool ExistInHits(GameObject obj, object hits)
    {
        if (hits == null || !(hits is List<RaycastResult>))
        {
            Debug.LogError("Hits가 null이거나 타입이 맞지 않습니다");
            return true;
        }

        foreach (RaycastResult hit in hits as List<RaycastResult>)
        {
            if (hit.gameObject == obj && (obj.transform.IsChildOf(hit.gameObject.transform) || hit.gameObject.transform.IsChildOf(obj.transform)))
            {
                return true;
            }
        }

        return false;
    }

    public static bool ExistInFirstHits(GameObject obj, object hits)
    {
        if (hits == null || !(hits is List<RaycastResult>))
        {
            Debug.LogError("Hits가 null이거나 타입이 맞지 않습니다");
            return true;
        }

        List<RaycastResult> rayList = hits as List<RaycastResult>;

        if(rayList.Count == 0)
        {
            return false;
        }

        RaycastResult hit = rayList[0];
        if (obj.transform.IsChildOf(hit.gameObject.transform) || hit.gameObject.transform.IsChildOf(obj.transform))
        {
            return true;
        }
        return false;
    }

    public static Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value, Random.value);
    }

    public static bool IsBoundOver(int idx, int maxCnt)
    {
        return idx < 0 || idx >= maxCnt;
    }

    public static bool CheckGameState(EGameState state)
    {
        return GameManager.Inst.GameState == state;
    }


    public static bool CheckIntallProfile()
    {
        return FileManager.Inst.SearchFile("Profile") != null;
    }

    public static CursorChangeSystem.ECursorState ChangeInfoCursor(List<ProfileInfoTextDataSO> needInfoList, EProfileCategory category, string infoKey)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return CursorChangeSystem.ECursorState.Default;
        }


        CursorChangeSystem.ECursorState state = CursorChangeSystem.ECursorState.Default;

        if(category == EProfileCategory.None)
        {
            state = CursorChangeSystem.ECursorState.FindInfo;
            EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });

            return state;
        }

        if (needInfoList.Count > 0)
        {
            foreach(ProfileInfoTextDataSO needData in needInfoList)
            {
                if (!DataManager.Inst.IsProfileInfoData(needData.category, needData.key))
                {
                    EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
                    return CursorChangeSystem.ECursorState.Default;
                }
            }
        }

        if (DataManager.Inst.IsProfileInfoData(category, infoKey))
        {
            state = CursorChangeSystem.ECursorState.FoundInfo;
        }
        else
        {
            state = CursorChangeSystem.ECursorState.FindInfo;
        }
        
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
        return state;
    }

    public static bool CheckTodayDate(int day)
    {
        if(Constant.NOWDAY == day)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public static bool CheckYesterDayDate(int day)
    {
        if (Constant.NOWDAY -1 == day)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void SetSprite(Image image, Sprite sprite, Vector2 maxSize)
    {
        if(sprite == null)
        {
            return;
        }
        image.sprite = sprite;

        Vector2 size = image.sprite.rect.size;

        float scale = 1f;
        if (size.y > maxSize.y)
        {
            scale = maxSize.y / size.y;
        }
        else if (size.x > maxSize.x)
        {
            scale = maxSize.x / size.x;
        }

        image.rectTransform.sizeDelta = size * scale;
    }

    public static bool MonologLockDecisionFlag(List<MonologLockDecision> list)
    {
        bool result = true;

        foreach(MonologLockDecision decision in list)
        {
            switch (decision.decisionType)
            {
                case MonologLockDecision.EDecisionType.Infomation:
                    // 해당 정보 id를 비교하여 정보를 획득했는지 확인
                    break;

                case MonologLockDecision.EDecisionType.Monolog:
                    // 해당 독백 id를 비교하여 독백을 봤는지 확인
                    break;
            }
        }

        return result;
    }
}