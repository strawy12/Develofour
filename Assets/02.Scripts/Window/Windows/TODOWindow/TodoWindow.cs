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

    // next�� ��� idx: +1 
    // back�� ��� idx: -1
    // additionIdx�� TodoPanel.selectPanel �� idx�� ã�� �� idx�� ���ϴ� ��
    private void ChangeSelectTodoPanel(int additionIdx)
    {
        // TODO
        // ���� ���ı���� �߰��ȴٸ� ������ ����� �� ����
        // expendPanel�� expend�� �Ǿ����� ���� ���¿����� return
        if(expendPanel.currentTodoPanel == null)
        {
            return;
        }

        int idx = todoPanelList.FindIndex(x => x  == expendPanel.currentTodoPanel);
        if (idx + additionIdx < 0 || idx + additionIdx >= todoPanelList.Count) return;

        expendPanel.SetCurrentTodoPanel(todoPanelList[idx + additionIdx]);
    }

}
