using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


// �ӽ� EsiteLing
public enum ESiteLink
{ 
    None,
    Chrome,
    Youtube,
}



public class Browser : Window
{
    private Dictionary<ESiteLink, Site> siteDictionary;
    
    private Site usingSite;
    
    private Stack<Site> undoSite;
    private Stack<Site> redoSite;

    private Transform siteParent;

    private BrowserBar browserBar;

    void Start()
    {
        Init();
    }

    private void Init()
    {
        undoSite = new Stack<Site>();
        redoSite = new Stack<Site>();
    }

    private void BindingStart()
    {
        siteDictionary.Add(ESiteLink.Chrome, usingSite);
        undoSite.Push(usingSite);
    }

    public void ChangeSite(ESiteLink eSiteLink)
    {
        
    }

    public void ChangeSite(string siteString)
    {

    }

    public void UndoSite()
    {
        redoSite.Push(undoSite.Pop()); // �� �����Ŵ� redo�� 
        usingSite = undoSite.Pop(); // �� �����Ŵ� ���� ���·� back
    }

    public void RedoSite()
    {
        undoSite.Push(redoSite.Pop());
        usingSite = redoSite.Pop(); 
        // �۵��� UndoSite�Լ��� �� �ݴ�� 
    }

    public void ResetBrowser()
    {
        usingSite = null;

        undoSite.Clear();
        redoSite.Clear();
    }
}
