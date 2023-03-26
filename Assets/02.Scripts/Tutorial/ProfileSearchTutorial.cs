using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSearchTutorial : MonoBehaviour
{
    private void Start()
    {
        EventManager.StartListening(EProfileSearchTutorialEvent.TutorialStart, TutorialStartMonolog);
    }

    private void TutorialStartMonolog(object[] ps)
    {
        MonologSystem.OnStartMonolog?.Invoke(EMonologTextDataType.StartSearchTutoMonolog, 0.3f);
    }
}
