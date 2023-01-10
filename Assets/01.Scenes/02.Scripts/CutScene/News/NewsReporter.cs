using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsReporter : NewsCharacter
{
    public Image image { get; private set; }
    public RectTransform rectTransform { get; private set; }

    public void Init()
    {
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    public override void StartSpeak()
    {
    }
    public override void EndSpeak()
    {
    }

}
