using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class FacebookProfileSetHeight : MonoBehaviour
{
    public GameObject child;

    public void Setting()
    {
        Task.Run(() => SetHeight());
    }

    async Task SetHeight()
    {
        await Task.Delay(300);

        float currentWidth = gameObject.GetComponent<RectTransform>().sizeDelta.x;
        float childHeight = child.GetComponent<RectTransform>().sizeDelta.y;
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(currentWidth, childHeight + 200);
    }
}
