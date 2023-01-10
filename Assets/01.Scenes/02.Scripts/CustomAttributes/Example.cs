using UnityEngine;


public class Example : MonoBehaviour
{
    [BitMask(typeof(EEmailCategory))]
    public int someMask;
}