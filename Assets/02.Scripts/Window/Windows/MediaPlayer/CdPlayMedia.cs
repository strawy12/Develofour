using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CdPlayMedia : MonoBehaviour
{
    public float rotationSpeed = 5f;
    [SerializeField]
    private Image cd;

    private bool isRoll;
    private float rollCnt;

    private void Update()
    {
        if (isRoll)
        {
            if (rollCnt <= 0)
            {
                rollCnt = 360;
            }
            cd.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rollCnt));
            rollCnt -= Time.deltaTime * rotationSpeed;
        }
    }

    public void Init()
    {
        PlayCdAnimation();
    }

    public void PlayCdAnimation()
    {
        isRoll = true;
    }

    private IEnumerator PlayRotationAnimation()
    {
        float num = 0;
        while(true)
        {
            cd.transform.Rotate(new Vector3(0, 0, -num));
            num += rotationSpeed * Time.deltaTime;
            Debug.Log(num);
            if(num >= 360)
            {
                num = 0;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopCdAnimation()
    {
        isRoll = false;
    }
}
