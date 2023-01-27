using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
        Application.Quit();
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
            if (hit.gameObject == obj)
            {
                return true;
            }
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
}
