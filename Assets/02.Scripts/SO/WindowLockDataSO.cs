using UnityEngine;

[CreateAssetMenu(menuName = "SO/Window/LockData")]
public class WindowLockDataSO : ScriptableObject
{
    public int fileId;

    public string windowPin;
    public string windowPinHintGuide;

    public AutoAnswerData answerData;
}
