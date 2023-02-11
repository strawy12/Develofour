using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;

namespace TMPro
{
    public static class DotweenExtensionMethod
    {
        public static TweenerCore<int, int, NoOptions> DOVisible(this TMP_Text text, float duration)
        {
            text.maxVisibleCharacters = 0;
            return DOTween.To(
                 () => text.maxVisibleCharacters,
                 (value) => text.maxVisibleCharacters = value,
                 text.text.Length, 
                 duration);

        }
    }
}


