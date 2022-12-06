using UnityEditor;
using UnityEditor.UI;
using UnityEngine.UI;

[CustomEditor(typeof(HighlightBtn))]
public class HighlightBtnEditor : ButtonEditor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        HighlightBtn targetButton = (HighlightBtn)target;

        targetButton.maxAlpha = EditorGUILayout.FloatField("MaxAlpha", targetButton.maxAlpha);
        targetButton.duration = EditorGUILayout.FloatField("Duration", targetButton.duration);
        targetButton.highLightImage = (Image)EditorGUILayout.ObjectField("HighLightImage", targetButton.highLightImage, typeof(Image), true);

    }
}