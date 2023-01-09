using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DiscordMessagePanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text messageText;

    [SerializeField]
    private Image messageImage;

    public void Reset()
    {
        messageText.text = "";
        messageImage = null;
        messageImage.gameObject.SetActive(false);
    }

    
}
