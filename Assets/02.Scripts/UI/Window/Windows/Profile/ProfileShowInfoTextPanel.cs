using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ProfileShowInfoTextPanel : MonoBehaviour
{
    public TMP_Text text;

    public GameObject showPanelParent;

    public TMP_Text downText;

    public void SetDownText()
    {
        downText.text = text.text;
    }
}
