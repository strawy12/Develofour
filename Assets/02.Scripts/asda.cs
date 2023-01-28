using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asda : MonoBehaviour
{
    [SerializeField]
    private Transform parent;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.IsChildOf(parent));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
