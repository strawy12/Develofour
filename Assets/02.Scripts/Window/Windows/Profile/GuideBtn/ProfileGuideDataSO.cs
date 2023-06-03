using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Profile/Guide")]
public class ProfileGuideDataSO : ScriptableObject
{
    public string guideName;
    [TextArea(5,15)]
    public List<string> guideTextList;
}
