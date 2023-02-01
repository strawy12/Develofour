using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FacebookProfileSetHeight : MonoBehaviour
{
    public GameObject child;
    public float duration = 0.5f;
    public void Update()
    {

    }

    public void Setting()
    {
        StartCoroutine(SetHeight());
    }

    private IEnumerator SetHeight()
    {
        yield return new WaitForSeconds(duration);
        float currentWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float childHeight = child.GetComponent<RectTransform>().sizeDelta.y;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(currentWidth, childHeight + 100);
    }

}
