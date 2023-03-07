using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChangeSystem : MonoBehaviour
{
    [SerializeField]
    private Texture2D provisoCursor;
    [SerializeField]
    private Texture2D findedProvisoCursor;

    private void Start()
    {
        EventManager.StartListening(ECoreEvent.CursorChange, CursorChange);
    }

    private void CursorChange(object[] ps)
    {
        if (ps[0] == null) 
        {
            return;
        }

        string commandWord = ps[0].ToString();

        if(commandWord == "FindingWord")
        {
            Cursor.SetCursor(provisoCursor, new Vector2(provisoCursor.width / 2, provisoCursor.height / 2), CursorMode.Auto);
        }
        else if(commandWord == "FindedWord")
        {
            Cursor.SetCursor(findedProvisoCursor, new Vector2(findedProvisoCursor.width / 2, findedProvisoCursor.height / 2), CursorMode.Auto);
        }
    }
    private void OnApplicationQuit()
    {
        EventManager.StopListening(ECoreEvent.CursorChange, CursorChange);
    }
}
