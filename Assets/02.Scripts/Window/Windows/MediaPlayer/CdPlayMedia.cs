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

    public void Init()
    {
        PlayCdAnimation();
    }

    public void PlayCdAnimation()
    {
        StartCoroutine("PlayRotationAnimation");
    }

    private IEnumerator PlayRotationAnimation()
    {

        while(true)
        {
            cd.transform.Rotate(new Vector3(0, 0, -(rotationSpeed * Time.deltaTime)));
            yield return new WaitForEndOfFrame();
        }
    }

    public void StopCdAnimation()
    {
        StopAllCoroutines();
    }
}
