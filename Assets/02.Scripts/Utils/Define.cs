using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Define
{
    private static Camera _mainCam;

    public static Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }

            return _mainCam;
        }
        
    }

    public static Vector2 CanvasMousePos
    {
        get
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 0f;
            mousePos -= (Vector3)Constant.MAXCANVASPOS;

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
}
