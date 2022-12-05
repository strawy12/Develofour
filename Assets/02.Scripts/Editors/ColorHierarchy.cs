using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ColorHierarchy : MonoBehaviour
{
#if UNITY_EDITOR
    private static Dictionary<Object, ColorHierarchy> coloredObjects = new Dictionary<Object, ColorHierarchy>();

    static ColorHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HandleDraw;
    }

    private static void HandleDraw(int instanceId, Rect selectionRect)
    {
        Object obj = EditorUtility.InstanceIDToObject(instanceId);

        if(obj != null && coloredObjects.ContainsKey(obj))
        {
            GameObject gObj = obj as GameObject;
            ColorHierarchy ch = gObj.GetComponent<ColorHierarchy>();
            if(ch != null)
            {
                PaintObject(obj, selectionRect, ch);
            }
            else
            {
                coloredObjects.Remove(obj); //이제 없으니 제거
            }
        }
            
    }

    private static void PaintObject(Object obj, Rect selectionRect, ColorHierarchy ch)
    {
        Rect bgRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width + 50, selectionRect.height);

        if (Selection.activeObject != obj && ch.gameObject.activeSelf)
        {
            EditorGUI.DrawRect(bgRect, ch.backColor);

            string name = $"{ch.prefix}  {obj.name}";

            EditorGUI.LabelField(bgRect, name, new GUIStyle()
            {
                normal = new GUIStyleState() { textColor = ch.fontColor },
                fontStyle = FontStyle.Bold
            });
        }
        //선택상태일경우에는 유니티 기본으로 되도록
    }

    public string prefix;
    public Color backColor;
    public Color fontColor;

    private void Reset()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if (false == coloredObjects.ContainsKey(this.gameObject)) // notify editor of new color
        {
            coloredObjects.Add(this.gameObject, this);
        }
    }
#endif
}
