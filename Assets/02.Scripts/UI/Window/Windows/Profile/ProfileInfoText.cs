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

    public float x;
    public float y;

    //이전 텍스트로 변경, 이후 텍스트로 변경해주는 함수


    public void ChangeText()
    {
        infoText.text = afterText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoText.text != afterText)
        {
            return;
        }
        showPanel.text.text = getInfoText;
        if(eventData.position.y > 200)
        {
            Debug.Log(eventData.position.y);
            Debug.Log(transform.localPosition);
            showPanel.transform.position = transform.position + new Vector3(0.5f, -0.35f,0);
        }
        else
        {
            showPanel.transform.position = transform.position + new Vector3(0.5f, 0.35f, 0);
            //showPanel.transform.position = this.gameObject.transform.position + new Vector3(-0.72f, showPanel.transform.position.y, 0);
            Debug.Log(eventData.position.y);
        }
        showPanel.gameObject.SetActive(true);
        showPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        showPanel.gameObject.SetActive(false);
    }
}
