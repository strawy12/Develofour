using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum EColorExtenstion
{
    None = -1,
    R,
    G,
    B,
    A,
}

namespace ExtenstionMethod
{
    public static class ColorExtenstionMethod
    {
        public static Color ChangeColor(this Color color, float r = -1, float g = -1, float b = -1, float a = -1)
        {

            if(r == -1) { r = color.r; }
            if(g == -1) { g = color.g; }
            if(b == -1) { b = color.b; }
            if(a == -1) { a = color.a; }
            if(a > 1) { a = 1; }
            return new Color(r, g, b, a);
        }

        public static Color SetAlphaZero(this Color color)
        {
            color.a = 0;
            return color;
        }

        public static Color SetAlphaOne(this Color color)
        {
            color.a = 1;
            return color;
        }

        public static void ChangeImageAlpha(this Image image, float a)
        {
            Color col = image.color;
            col.a = a;
            image.color = col;
        }

    }
}