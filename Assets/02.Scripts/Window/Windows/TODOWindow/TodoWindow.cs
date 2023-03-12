using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TodoWindow : Window
{
    [SerializeField]
    private TodoPanel todoPanelPrefab;
    [SerializeField]
    private Transform todoPanelParent;
    [SerializeField]
    private QuestDataListSO questDataList;

    [SerializeField]
    private ExpendPanel expendPanel;

    private List<TodoPanel> todoPanelList;


    protected override void Init()
    {
        base.Init();
        todoPanelList = new List<TodoPanel>();
        InitExpendPanel();
        CreateTodoPanels();
    }

    private void InitExpendPanel()
    {
        expendPanel.Init();
        expendPanel.OnClickBackBtn += BackTodoPanel;
        expendPanel.OnClickNextBtn += NextTodoPanel;
    }

    private void CreateTodoPanels()
    {
        for (int i = 0; i < questDataList.Count; i++)
        {
            QuestDataSO data = questDataList[i];
            TodoPanel panel = Instantiate(todoPanelPrefab, todoPanelParent);

            panel.Init(data);
            panel.OnClick += expendPanel.SetCurrentTodoPanel;

            panel.gameObject.SetActive(true);

            todoPanelList.Add(panel);
        }
    }

    private void NextTodoPanel()
    {
        ChangeSelectTodoPanel(1);
    }

    private void BackTodoPanel()
    {
        ChangeSelectTodoPanel(-1);
    }

    // next일 경우 idx: +1 
    // back일 경우 idx: -1
    // additionIdx는 TodoPanel.selectPanel 의 idx를 찾고 이 idx에 더하는 값
    private void ChangeSelectTodoPanel(int additionIdx)
    {
        // TODO
        // 추후 정렬기능이 추가된다면 로직이 변경될 수 있음
        // expendPanel이 expend가 되어있지 않은 상태에서도 return
        if(expendPanel.currentTodoPanel == null)
        {
            return;
        }

        int idx = todoPanelList.FindIndex(x => x  == expendPanel.currentTodoPanel);
        if (idx + additionIdx < 0 || idx + additionIdx >= todoPanelList.Count) return;

        expendPanel.SetCurrentTodoPanel(todoPanelList[idx + additionIdx]);
    }

}
