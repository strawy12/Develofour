using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntMaskExtenstionMethod
{
    public static bool ContainMask(this int myVal, int value)
    {
        return (myVal & value) != 0;
    }

    public static int AddMask(this int myVal, int value)
    {
        return (myVal | value);
    }

    public static int RemoveMask(this int myVal, int value)
    {
        return (myVal & ~(value));
    }
}
