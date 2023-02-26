using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


//여기에 Data클래스랑 Enum울 하나 추가한다음 

public class AutoInputSystem : MonoBehaviour,IPointerClickHandler
{
    //여기에 리스트를 추가하고 그 Data의 이넘을 기반으로 불러오는 방식
    [SerializeField]
    private List<AutoInputPanel> autoInputPanelList = new List<AutoInputPanel>();

    public void HideAllPanel()
    {
        foreach(AutoInputPanel panel in autoInputPanelList) 
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void ShowPanel(List<TMP_InputField> inputFields, List<AutoAnswerData> answerDatas)
    {
        for(int i = 0;  i <answerDatas.Count; i++)
        {
            autoInputPanelList[i].Setting(inputFields, answerDatas[i]);
        }

        gameObject.SetActive(true);
    }

    public void OffAutoPanel()
    {
        HideAllPanel();
        gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //if () ;
    }

}
