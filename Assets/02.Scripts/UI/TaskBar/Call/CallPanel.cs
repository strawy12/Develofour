using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallPanel : MonoBehaviour
{
    [SerializeField]
    private Button CallButton;
    [SerializeField]
    private PhoneCallUI phoneCallUI;

    void Start()
    {
        CallButton.onClick?.AddListener(OnPhoneCallUI);
    }

    private void OnPhoneCallUI()
    {
        phoneCallUI.Open();
    }
}
