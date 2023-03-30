using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TextList
{
    public List<TextData> data;
}

[CreateAssetMenu(fileName = "TextData_", menuName = "SO/TextDataSO/TutorialTextSO")]
public class TutorialTextSO : ScriptableObject
{
    public List<TextList> tutorialTexts;
    [Multiline]
    public string popText;
}
