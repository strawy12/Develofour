using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
[System.Serializable]
public class TodoData 
{
    // юс╫ц©К
    public string todoName;
    public string category;
    public int successRate;
}

public class TodoPanel : MonoBehaviour
{
    [SerializeField]
    private TodoData todoData;

    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private TMP_Text categoryText;
    [SerializeField]
    private SuccessRateField successRateField;


    public void Init(TodoData data)
    {
        todoData = data;

        nameText.text = todoData.todoName;
        categoryText.text = todoData.category;
        
    }
}
