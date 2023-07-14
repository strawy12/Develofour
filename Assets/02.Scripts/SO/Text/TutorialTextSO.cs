using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "SO/TextDataSO/TutorialTextSO")]
public class TutorialTextSO : ScriptableObject
{
    public List<AIChattingTextDataSO> tutorialChatData;
    public string popText;
}
