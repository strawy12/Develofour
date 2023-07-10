using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextList
{
    [TextArea(5,30)]
    public List<string> data;
}

[CreateAssetMenu(menuName = "SO/TextDataSO/TutorialTextSO")]
public class TutorialTextSO : ScriptableObject
{
    public List<AIChattingDataSO> tutorialss;
    public List<TextList> tutorialTexts;
    [Multiline]
    public string popText;
}
