using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntExtenstionMethod
{
    public static bool ContainMask(this int myVal, int value)
    {
        return (myVal & value) != 0;
    }
}
