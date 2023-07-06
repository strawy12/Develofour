using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraResolution : MonoBehaviour
{
    private Camera mainCam;

    void Start()
    {
        mainCam = GetComponent<Camera>();
    }

    private void Update()
    {
        if(mainCam.rect.width != Screen.width || mainCam.rect.height != Screen.height)
        {
            Rect rect = mainCam.rect;

            float scaleheight = ((float)Screen.width / Screen.height) / ((float)16f / 9f);
            float scalewidth = 1f / scaleheight;

            if (scaleheight < 1)
            {
                rect.height = scaleheight;
                rect.y = (1f - scaleheight) / 2f;
            }
            else
            {
                rect.width = scalewidth;
                rect.x = (1f - scalewidth) / 2f;
            }

            mainCam.rect = rect;
        }
    }
}