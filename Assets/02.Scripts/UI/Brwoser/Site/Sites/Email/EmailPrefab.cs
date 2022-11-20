using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EmailPrefab : MonoBehaviour
{
    public virtual void ShowMail()
    {
        gameObject.SetActive(true);
    }
}
