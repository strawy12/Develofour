using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileInfoText : MonoBehaviour
{
    public string infoNameKey;

    public string afterText;

    public TMP_Text infoText;

    //이전 텍스트로 변경, 이후 텍스트로 변경해주는 함수


    public void ChangeText()
    {
        infoText.text = afterText;
    }
}
