using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProfileInfoText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string infoNameKey;

    public string afterText;

    public string getInfoText;

    public TMP_Text infoText;

    public ProfileShowInfoTextPanel showPanel;

    public TMP_Text infoTitleText;

    private string infoTitle;
    //이전 텍스트로 변경, 이후 텍스트로 변경해주는 함수

    public void Init()
    {
        infoTitle = infoTitleText.text;

        string[] info = infoTitle.Split(" ");
        string str ="";
        for(int i = 0; i< info[0].Length; i++)
        {
            str += "?";
        }
        str += " :";
        infoTitleText.text = str;
    }

    public void ShowTitle()
    {
        infoTitleText.text = infoTitle;
    }


    public void ChangeText()
    {
        infoTitleText.text = infoTitle;
        infoText.text = afterText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoText.text != afterText)
        {
            return;
        }
        showPanel.text.text = getInfoText;
        showPanel.transform.parent = this.gameObject.transform.parent;
        showPanel.transform.position = gameObject.transform.position + new Vector3(0, 0.3f,0);
        showPanel.transform.parent = showPanel.showPanelParent.transform;
        showPanel.SetDownText();
        showPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showPanel == null) return;
        showPanel.gameObject.SetActive(false);
    }
}
