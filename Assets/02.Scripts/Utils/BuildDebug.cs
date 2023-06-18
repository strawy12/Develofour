using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildDebug : MonoBehaviour
{
#if UNITY_EDITOR
#else
private void Update()
{
        if (Input.GetKeyDown(KeyCode.D))
        {
            MonologSystem.OnStopMonolog?.Invoke();
        }
}
#endif
}
