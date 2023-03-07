using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


//���⿡ DataŬ������ Enum�� �ϳ� �߰��Ѵ��� 

public class AutoInputSystem : MonoBehaviour
{
    //���⿡ ����Ʈ�� �߰��ϰ� �� Data�� �̳��� ������� �ҷ����� ���
    [SerializeField]
    private List<AutoInputPanel> autoInputPanelList = new List<AutoInputPanel>();

    private bool isOpen;

    private bool isHave;
    public void HideAllPanel()
    {
        foreach(AutoInputPanel panel in autoInputPanelList) 
        {
            panel.gameObject.SetActive(false);
        }
    }

    public void ShowPanel(List<TMP_InputField> inputFields, List<AutoAnswerData> answerDatas)
    {
        isHave = false;
        for(int i = 0;  i < answerDatas.Count; i++)
        {
            if(answerDatas[i].isShow)
            {
                autoInputPanelList[i].Setting(inputFields, answerDatas[i]);
                isHave = true;
            }
        }
        if (!isHave)
        {
            return;
        }
        gameObject.SetActive(true);
        isOpen = true;
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    private void CheckClose(object[] hits)
    {
        if (!isOpen) return;
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Close()
    {
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckClose);
        isOpen = false;
        gameObject.SetActive(false);
    }

    public void OffAutoPanel()
    {
        HideAllPanel();
        gameObject.SetActive(false);
    }



}
