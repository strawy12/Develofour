using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DebugEvent
{
    public KeyCode keyCode;
    public UnityEvent debugEvent;
}

public class Debugger : MonoBehaviour
{
    [SerializeField]
    private List<DebugEvent> debugEventList;

    [MenuItem("Tools/ChangeTMPFonts")]
    public static  void ChangeTMPFonts()
    {
        TMP_Text[] tmpTexts = Selection.activeGameObject.GetComponentsInChildren<TMP_Text>();
        TMP_InputField[] tmpInputFields = Selection.activeGameObject.GetComponentsInChildren<TMP_InputField>();

        TMP_FontAsset font = Resources.Load<TMP_FontAsset>("NanumGothic SDF");
        Debug.Log(font);
        List<GameObject> prefabs = new List<GameObject>();

        foreach (TMP_Text text in tmpTexts)
        {
            if (text.font.name == "LiberationSans SDF")
            {
                text.font = font;
                if (PrefabUtility.IsPartOfPrefabInstance(text))
                {
                    prefabs.Add(PrefabUtility.GetOutermostPrefabInstanceRoot(text.gameObject));
                }
            }
        }

        foreach (TMP_InputField field in tmpInputFields)
        {
            if (field.fontAsset.name == "LiberationSans SDF")
            {
                field.fontAsset = font;
                if (PrefabUtility.IsPartOfPrefabInstance(field))
                {

                    prefabs.Add(PrefabUtility.GetOutermostPrefabInstanceRoot(field.gameObject));
                }
            }
        }

        foreach (GameObject prefab in prefabs)
        {
            Debug.Log(prefab.name);
            PrefabUtility.ApplyPrefabInstance(prefab, InteractionMode.UserAction);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSiteAll);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            EventManager.TriggerEvent(ECutSceneEvent.SkipCutScene);
        }

        if (Input.GetKeyDown(KeyCode.V))
        {
            Time.timeScale = 10;
        }
        if (Input.GetKeyUp(KeyCode.V))
        {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[] { EMailType.Default });
        }

        foreach (DebugEvent e in debugEventList)
        {
            if (Input.GetKeyDown(e.keyCode))
            {
                e.debugEvent?.Invoke();
            }
        }
    }

}
