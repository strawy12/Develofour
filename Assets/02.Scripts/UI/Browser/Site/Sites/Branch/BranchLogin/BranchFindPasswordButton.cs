using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchFindPasswordButton : MonoBehaviour
{
    [SerializeField]
    private BranchFindPasswordPanel findPanel;

    private Button button;

    private string id;
    public void Init(string id)
    {
        Bind();
        this.id = id;
        findPanel.Init(id);
        button.onClick?.AddListener(ShowFindPanel);
    }

    private void Bind()
    {
        button ??= GetComponent<Button>();
    }

    private void ShowFindPanel()
    {
        findPanel.Show();
    }
}
