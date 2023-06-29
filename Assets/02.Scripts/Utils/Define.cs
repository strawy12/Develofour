using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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

        if (rayList.Count == 0)
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


    public static bool CheckIntallProfiler()
    {
        return FileManager.Inst.IsExistFile(Constant.FileID.PROFILER);
    }

    public static CursorChangeSystem.ECursorState ChangeInfoCursor(List<NeedInfoData> needInfoList, List<int> infoIDList)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return CursorChangeSystem.ECursorState.Default;
        }


        CursorChangeSystem.ECursorState state = CursorChangeSystem.ECursorState.NeedInfo;
        if (needInfoList.Count > 0)
        {
            foreach (NeedInfoData needData in needInfoList)
            {
                if (!DataManager.Inst.IsProfilerInfoData(needData.needInfoID))
                {
                    EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
                    return CursorChangeSystem.ECursorState.NeedInfo;
                }
            }
        }
        foreach (var infoID in infoIDList)
        {
            if (DataManager.Inst.IsProfilerInfoData(infoID))
            {
                state = CursorChangeSystem.ECursorState.FoundInfo;
            }
            else
            {
                state = CursorChangeSystem.ECursorState.FindInfo;
                break;
            }

        }
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
        return state;
    }
    public static bool CheckTodayDate(int day)
    {
        if (Constant.NOWDAY == day)
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
        if (Constant.NOWDAY - 1 == day)
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
        if (sprite == null)
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
        if (list == null) return true;

        foreach (MonologLockDecision decision in list)
        {
            switch (decision.decisionType)
            {
                case MonologLockDecision.EDecisionType.Information:
                    // 해당 정보 id를 비교하여 정보를 획득했는지 확인
                    if (!DataManager.Inst.IsProfilerInfoData(decision.key))
                    {
                        return false;
                    }
                    break;

                case MonologLockDecision.EDecisionType.Monolog:
                    if (!DataManager.Inst.IsMonologShow(decision.key))
                    {
                        return false;
                    }
                    break;
            }
        }

        return true;
    }

    public static void SetTriggerPosition(TMP_Text text, List<TextTriggerData> triggerList)
    {
        text.ForceMeshUpdate();
        if (triggerList != null && triggerList.Count > 0)
        {
            foreach (TextTriggerData trigger in triggerList)
            {
                Vector2 pos = text.textInfo.characterInfo[trigger.id].topLeft; 

                for (int i = trigger.id + 1; i < text.text.Length; i++)
                {
                    if (text.text[i] == ' ') break;
                    Vector2 temp = text.textInfo.characterInfo[i].topLeft;
                    if (pos.y < temp.y)
                    {
                        pos.y = temp.y;
                    }
                }

                (trigger.trigger.transform as RectTransform).anchoredPosition = pos;
            }
        }
    }
}