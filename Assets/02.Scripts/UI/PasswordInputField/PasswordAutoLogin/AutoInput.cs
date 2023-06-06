using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;


//여기에 Data클래스랑 Enum울 하나 추가한다음 

public class AutoInput : MonoBehaviour
{
    //여기에 리스트를 추가하고 그 Data의 이넘을 기반으로 불러오는 방식
    [SerializeField]
    private List<AutoInputPanel> autoInputPanelList = new List<AutoInputPanel>();

    private bool isOpen;

    private bool isCanShowPanel;

    public void ShowPanel(TMP_InputField inputField, List<AutoAnswerData> answerDatas)
    {
        isCanShowPanel = false;
        for(int i = 0;  i < answerDatas.Count; i++)
        {
            AutoAnswerData data = answerDatas[i];

            if(DataManager.Inst.IsProfilerInfoData(data.infoData.id))
            {
                autoInputPanelList[i].Setting(inputField, answerDatas[i].answer);
                isCanShowPanel = true;
            }
        }
        if (!isCanShowPanel)
        {
            return;
        }
        gameObject.SetActive(true);
        isOpen = true;
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    public void ShowPanel(TMP_InputField inputField, string answer)
    {
        autoInputPanelList[0].Setting(inputField, answer);
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
        OffAutoPanel();
    }

    public void OffAutoPanel()
    {
        HideAllPanel();
        gameObject.SetActive(false);
    }

    public void HideAllPanel()
    {
        foreach (AutoInputPanel panel in autoInputPanelList)
        {
            panel.gameObject.SetActive(false);
        }
    }

}
