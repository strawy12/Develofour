using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PropertyWindow : Window 
{
    [SerializeField]
    private Button confirmButton;

    [SerializeField]
    private IconPropertyBody body;

    protected override void Init()
    {
        base.Init();
        if(file.fileData.propertyBody != null)
        {
            Destroy(body);
            IconPropertyBody newBody = Instantiate(File.fileData.propertyBody, transform);
            body = newBody;
        }

        body.Init(file);
        confirmButton.onClick.AddListener(WindowClose);
    }


}
