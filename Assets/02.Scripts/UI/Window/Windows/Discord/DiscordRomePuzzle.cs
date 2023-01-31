using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;
using TMPro;
public class DiscordRomePuzzle : MonoBehaviour
{
    public GameObject[] romeNum;

    public Transform[] numPos;

    //[Range(0.6f, 1.4f)]
    public float rotateMinValue;
    public float rotateMaxValue;

    public float scaleMinValue;
    public float scaleMaxValue;

    public int currentAnswer;
    public int[] ansNum = new int[5];

    public Button resetButton;

    public TMP_InputField inputField;

    #region Mix

    [SerializeField]
    private List<int> defaultList = new List<int>{ 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    [SerializeField]
    private List<int> defaultList2 = new List<int>();

    private void MixList()
    {
        foreach(var temp in romeNum)
        {
            temp.SetActive(false);
        }

        defaultList2 = ShuffleList(defaultList);

        for (int i = 0; i < 5; i++)
        {
            ansNum[i] = defaultList2[i];
        }

        currentAnswer = 0;
        currentAnswer += ansNum[0];

        for (int i = 1; i < 5; i++)
        {
            currentAnswer *= 10;
            currentAnswer += ansNum[i];
        }
    }

    private List<T> ShuffleList<T>(List<T> list)
    {
        int random1, random2;
        T temp;

        for (int i = 0; i < list.Count; ++i)
        {
            random1 = UnityEngine.Random.Range(0, list.Count);
            random2 = UnityEngine.Random.Range(0, list.Count);

            temp = list[random1];
            list[random1] = list[random2];
            list[random2] = temp;
        }

        return list;
    }

    #endregion

    public bool IsAnswer()
    {
        if(inputField.text == currentAnswer.ToString())
        {
            return true;
        }
        return false;
    }

    public void Init()
    {
        SetNumber();
        resetButton.onClick.AddListener(SetNumber);
    }

    public void SetNumber()
    {
        MixList();
        for(int i = 0; i < 5; i++)
        {
            romeNum[ansNum[i] - 1].transform.position = numPos[i].position;
            romeNum[ansNum[i] - 1].transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(rotateMinValue, rotateMaxValue)));
            romeNum[ansNum[i] - 1].transform.localScale = 
                new Vector3(UnityEngine.Random.Range(scaleMinValue, scaleMaxValue), UnityEngine.Random.Range(scaleMinValue, scaleMaxValue), 0);
            romeNum[ansNum[i] - 1].SetActive(true);
        }
    }

}
