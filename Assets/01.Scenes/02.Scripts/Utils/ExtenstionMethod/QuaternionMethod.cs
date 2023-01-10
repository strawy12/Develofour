using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtenstionMethod
{
    public static class QuaternionMethod
    {
        public static Quaternion RotationX(this Quaternion quaternion, float x)
        {
            quaternion.x = x;
            return quaternion;
        }
        
        public static Quaternion RotationY(this Quaternion quaternion, float y)
        {
            quaternion.y = y;
            return quaternion;
        }

        public static Quaternion RotationZ(this Quaternion quaternion, float z)
        {
            quaternion.z = z;
            return quaternion;
        }

    }
}
