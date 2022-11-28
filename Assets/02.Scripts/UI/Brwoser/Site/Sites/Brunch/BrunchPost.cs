using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BrunchPost : MonoBehaviour
{
    public TextMeshProUGUI novelTitleText;
    public TextMeshProUGUI novelScriptsText;
    public TextMeshProUGUI novelInfoText;
    public Image novelImage;

    public void Init(BrunchPostSO postData)
    {
        novelTitleText.text = postData.postTitleText;
        novelScriptsText.text = postData.postScriptsText;
        novelInfoText.text = postData.postInfoText;
        novelImage.sprite = postData.postImage;
    }
}
