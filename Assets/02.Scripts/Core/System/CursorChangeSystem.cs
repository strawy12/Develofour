using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CursorChangeSystem : MonoBehaviour
{
    [SerializeField]
    private Texture2D defaultCursor;
    [SerializeField]
    private Texture2D provisoCursor;
    [SerializeField]
    private Texture2D findedProvisoCursor;

    private void Start()
    {
        EventManager.StartListening(ECoreEvent.CursorChange, CursorChange);
    }

    public enum ECursorState
    {
        Default,
        FindInfo,
        FoundInfo,
    }

    private void CursorChange(object[] ps)
    {
        if (ps[0] == null || !Define.CheckIntallProfile())
        {
            return;
        }


        ECursorState state = (ECursorState)ps[0];

        switch (state)
        {
            case ECursorState.Default:
                {
                    Cursor.SetCursor(defaultCursor, new Vector2(0, 0), CursorMode.Auto);
                    break;
                }

            case ECursorState.FindInfo:
                {
                    Cursor.SetCursor(provisoCursor, new Vector2(provisoCursor.width / 2, provisoCursor.height / 2), CursorMode.Auto);
                    break;
                }
            case ECursorState.FoundInfo:
                {
                    Cursor.SetCursor(findedProvisoCursor, new Vector2(findedProvisoCursor.width / 2, findedProvisoCursor.height / 2), CursorMode.Auto);
                    break;
                }

        }
    }

    private void OnApplicationQuit()
    {
        EventManager.StopListening(ECoreEvent.CursorChange, CursorChange);
    }
}
