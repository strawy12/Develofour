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

    //이전 텍스트로 변경, 이후 텍스트로 변경해주는 함수

    public void ChangeText()
    {
        infoText.text = afterText;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("왔음");
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
        showPanel.gameObject.SetActive(false);
    }
}
