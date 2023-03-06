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

    private void CursorChange(object ps)
    {
        if (ps == null) 
        {
            return;
        }

        string commandWord = ps.ToString();

        Debug.Log(commandWord);

        if(commandWord == "FindingWord")
        {
            Cursor.SetCursor(provisoCursor, new Vector2(0, 0), CursorMode.Auto);
        }
        else if(commandWord == "FindedWord")
        {
            Cursor.SetCursor(findedProvisoCursor, new Vector2(0, 0), CursorMode.Auto);
        }
    }
    private void OnApplicationQuit()
    {
        EventManager.StopListening(ECoreEvent.CursorChange, CursorChange);
    }
}
