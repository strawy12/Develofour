using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TextBox : MonoBehaviour
{
    private TextDataSO currentTextData;

    public void Init(ETextDataType textDataType)
    {
            currentTextData = GetTextData(textDataType);
    }

    public TextDataSO GetTextData(ETextDataType textDataType)
    {
        TextDataSO textDataSO= null;
        try
        {
            textDataSO = Resources.Load($"Resources/TextData/TextData_{textDataType}") as TextDataSO;
        }
        catch (NullReferenceException e)
        {
            Debug.Log($"TextData_{textDataType} is null");
        }
        catch(Exception e)
        {
            Debug.Log(e.ToString());
        }
        return textDataSO;
    }
    
}
