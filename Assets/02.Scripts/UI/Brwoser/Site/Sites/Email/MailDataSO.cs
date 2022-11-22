using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/Mail/Data")]
public class MailDataSO : ScriptableObject
{
    public EEmailCategory emailType;
    public string nameText;
    public string informationText;
    public string timeText;
    public bool isHighlighted;
}
