using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtenstionMethod
{
    public enum EOperator
    { 
        Addition,
        Subtraction,
        Multiply,
        Divide,
    }

    public enum EVertex
    {
        X,
        Y,
        Z,
    }

    public static class VectorExtenstionMethod
    {
        public static Vector3 Reset(this Vector3 vector)
        {
            return Vector3.zero;
        }

        public static Vector3 Calculation(this Vector3 vector, EOperator operatorType, float x = 0f, float y = 0f, float z = 0f)
        {
            Vector3 newVec = vector;
            switch (operatorType)
            {
                case EOperator.Addition:
                    newVec.x += x;
                    newVec.y += y;
                    newVec.z += z;
                    break;

                case EOperator.Subtraction:
                    newVec.x -= x;
                    newVec.y -= y;
                    newVec.z -= z;
                    break;

                default:
                    Debug.LogError($"벡터에는 해당 연산자의 적용이 불가능합니다. {operatorType}");
                    break;
            }

            return newVec;
        }

        public static Vector3 ChangeVectorXY(this Vector3 vector)
        {
            Vector3 newVec = vector;
            (newVec.x,  newVec.y) = (newVec.y, newVec.x);

            return newVec;
        }

        public static Vector3 TargetDirection(this Vector3 myVec, Vector3 target)
        {
            Vector3 targetDir = target - myVec;
            targetDir.Normalize();

            return targetDir;
        }

        public static Vector3 ChangeValue(this Vector3 vector, float x = 0f, float y = 0f, float z = 0f)
        {
            Vector3 newVec = vector;

            newVec.x = x == 0f ? vector.x : x;
            newVec.y = y == 0f ? vector.y : y;
            newVec.z = z == 0f ? vector.z : z;

            return newVec;
        }

        public static Vector2 ChangeVectorXY(this Vector2 vector)
        {
            Vector2 newVec = vector;
            (newVec.x, newVec.y) = (newVec.y, newVec.x);

            return newVec;
        }

        public static Vector2 ChangeValue(this Vector2 vector, float x = 0f, float y = 0f)
        {
            Vector2 newVec = vector;

            newVec.x = x == 0f ? vector.x : x;
            newVec.y = y == 0f ? vector.y : y;

            return newVec;
        }

        public static Vector2 Calculation(this Vector2 vector, EOperator operatorType, float x = 0f, float y = 0f)
        {
            Vector2 newVec = vector;
            switch (operatorType)
            {
                case EOperator.Addition:
                    newVec.x += x;
                    newVec.y += y;
                    break;

                case EOperator.Subtraction:
                    newVec.x -= x;
                    newVec.y -= y;
                    break;

                default:
                    Debug.LogError($"벡터에는 해당 연산자의 적용이 불가능합니다. {operatorType}");
                    break;
            }

            return newVec;
        }

        public static Vector2 TargetDirection(this Vector2 myVec, Vector2 target)
        {
            Vector2 targetDir = target - myVec;
            targetDir.Normalize();

            return targetDir;
        }
    }

}
