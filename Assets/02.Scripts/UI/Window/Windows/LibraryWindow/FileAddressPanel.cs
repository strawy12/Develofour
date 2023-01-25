﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileAddressPanel : MonoBehaviour
{

    [SerializeField]
    private Transform poolParent;
    [SerializeField]
    private Transform content;
    [SerializeField]
    private TopFileButton fileBtn;

    private List<TopFileButton> topFileButtons = new List<TopFileButton>();
    private Queue<TopFileButton> buttonPool = new Queue<TopFileButton>();

    private FileSO currentFileSO;

    private void CreatePanel()
    {
        for (int i = 0; i < 15; i++)
        {
            TopFileButton button = Instantiate(fileBtn, poolParent);
            button.Init();
            button.gameObject.SetActive(false);
            buttonPool.Enqueue(button);
        }
    }
    private TopFileButton Pop()
    {
        if(buttonPool.Count <= 0)
        {
            CreatePanel();
        }
        TopFileButton button = buttonPool.Dequeue();
        button.gameObject.SetActive(true);
        button.transform.SetParent(content);
        topFileButtons.Add(button);

        return button;
    }
    private void Push(TopFileButton button)
    {
        if(topFileButtons.Contains(button))
        {
            topFileButtons.Remove(button);
        }
        if(!buttonPool.Contains(button))
        {
            buttonPool.Enqueue(button);
        }
        button.gameObject.SetActive(false);
        button.transform.SetParent(poolParent);
    }
    private void PushAll()
    {
        while(topFileButtons.Count > 0)
        {
            Push(topFileButtons[0]);
        }
    }

    public void Init()
    {
        CreatePanel();
    }

    public void SetButtons(DirectorySO directoryData)
    {
        PushAll();
        DirectorySO directory = directoryData;

        int i = 0;

        while(directory != null)
        {
            i++;
            if(i > 1000)
            {
                Debug.LogError("While문이 계속해서 반복됨.");
                break;
            }
            TopFileButton button = Pop();

            button.SetDirectory(directory);

            directory = directory.parent;
        } 

    }

}
