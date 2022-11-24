using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class CursorChange : MonoBehaviour
{
    [SerializeField]
    private Texture2D cursorImg;
    [SerializeField]
    private Texture2D clickCursorImg;
    
    void Start()
    {
        Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);
    }

    private void Update()
    {
        // TODO :: ���ӸŴ��� ���� �������� �ٲܰ���, �ӽ�
        if(Input.GetMouseButtonDown(0))
            Cursor.SetCursor(clickCursorImg, Vector2.zero, CursorMode.Auto);
        else if(Input.GetMouseButtonUp(0))
            Cursor.SetCursor(cursorImg, Vector2.zero, CursorMode.Auto);

    }
}
