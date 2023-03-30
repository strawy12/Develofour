using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProfilePanelButton : MonoBehaviour
{
    [SerializeField]
    private TMP_Text btnText;

    public Button button;

    private RectTransform rectTransform;

    public RectTransform RectTrm
    {
        get
        {
            rectTransform ??= GetComponent<RectTransform>();
            return rectTransform;
        }
    }

    public void Setting(ProfilePanelButton beforeButton)
    {
        if(beforeButton == this)
        {
            button.image.color = Color.black;
            btnText.color = Color.white;
        }
        else
        {
            button.image.color = Color.white;
            btnText.color = Color.black;
        }
    }
}
