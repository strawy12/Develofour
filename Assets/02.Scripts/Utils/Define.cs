using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityRandom = UnityEngine.Random;
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
        return new Color(UnityRandom.value, UnityRandom.value, UnityRandom.value, UnityRandom.value);
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

    public static CursorChangeSystem.ECursorState ChangeInfoCursor(List<NeedInfoData> needInfoList, List<string> infoIDList)
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

    public static bool NeedInfoFlag(List<string> list)
    {
        if (list == null) return false;

        foreach (string infoID in list)
        {
            if (!DataManager.Inst.IsProfilerInfoData(infoID))
            {
                return false;
            }
        }

        return true;
    }

    public static string GetOutStarTimeText(DateTime dateTime)
    {
        string timeText = "";
        if (dateTime.Hour > 12)
        {
            timeText = $"{dateTime.Year}.{dateTime.Month: 00}.{dateTime.Day: 00} 오후{dateTime.Hour - 12}시 {dateTime.Minute}";
        }
        else if (dateTime.Hour == 12)
        {
            timeText = $"{dateTime.Year}.{dateTime.Month: 00}.{dateTime.Day: 00} 오후{dateTime.Hour}시 {dateTime.Minute}";
        }
        else if (dateTime.Hour == 0)
        {
            timeText = $"{dateTime.Year}.{dateTime.Month: 00}.{dateTime.Day: 00} 오전{12}시 {dateTime.Minute}";
        }
        else
        {
            timeText = $"{dateTime.Year}.{dateTime.Month: 00}.{dateTime.Day: 00} 오전{dateTime.Hour}시 {dateTime.Minute}";
        }
        return timeText;
    }
    #region Trigger
    public static void SetTriggerPosition(TMP_Text text, List<TextTriggerData> triggerList)
    {
        text.ForceMeshUpdate();
        if (triggerList != null && triggerList.Count > 0)
        {
            foreach (TextTriggerData trigger in triggerList)
            {

                Vector2 pos = text.textInfo.characterInfo[trigger.startIdx].topLeft;

                for (int i = trigger.startIdx + 1; i < text.text.Length; i++)
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

    public static void SetTiggerSize(TMP_Text text, List<TextTriggerData> triggerList)
    {
        text.ForceMeshUpdate();
        if (triggerList != null && triggerList.Count > 0)
        {
            foreach (TextTriggerData triggerData in triggerList)
            {

                RectTransform triggerRectTrm = (RectTransform)triggerData.trigger.transform;
                Vector2 newSize = CalcTextMaxSize(triggerData.startIdx, triggerData.endIdx, text);
                triggerRectTrm.sizeDelta = newSize;
            }
        }
    }
    #endregion
    public static Vector2 CalcTextMaxSize(int minIdx, int maxIdx, TMP_Text text)
    {
        Vector2 topLeft = new Vector2(int.MaxValue, 0);
        Vector2 bottomRight = new Vector2(0, int.MaxValue);
        var characterInfos = text.textInfo.characterInfo;
        for (int i = minIdx; i <= maxIdx; i++)
        {
            if (characterInfos[i].topLeft.x < topLeft.x)
            {
                topLeft.x = characterInfos[i].topLeft.x;
            }

            if (characterInfos[i].topLeft.y > topLeft.y)
            {
                topLeft.y = characterInfos[i].topLeft.y;
            }

            if (characterInfos[i].bottomRight.x > bottomRight.x)
            {
                bottomRight.x = characterInfos[i].bottomRight.x;
            }

            if (characterInfos[i].bottomRight.y < bottomRight.y)
            {
                bottomRight.y = characterInfos[i].bottomRight.y;
            }
        }

        Vector2 newSize = new Vector2(bottomRight.x - topLeft.x, topLeft.y - bottomRight.y);
        return newSize;
    }


#if UNITY_EDITOR
    public static List<T> GuidsToList<T>(string filtter) where T : UnityEngine.Object
    {
        string[] guids = AssetDatabase.FindAssets(filtter, null);
        List<T> SOList = new List<T>();
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            SOList.Add(AssetDatabase.LoadAssetAtPath<T>(path));
        }

        return SOList;
    }
#endif

}