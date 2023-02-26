using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


//���⿡ DataŬ������ Enum�� �ϳ� �߰��Ѵ��� 

public class AutoInputSystem : MonoBehaviour,IPointerClickHandler
{
    //���⿡ ����Ʈ�� �߰��ϰ� �� Data�� �̳��� ������� �ҷ����� ���
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
