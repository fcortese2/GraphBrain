using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleColorPicker
{

    public struct ColorPicker
    {
        private static int idx;

        private static Color[] colors = new Color[]
        {
            new Color(18f/255f, 230f/255f, 92f/255f, 1),
            new Color(133f/255f, 30f/255f, 230f/255f, 1),
            new Color(66f/255f, 230f/255f, 7f/255f, 1),
            new Color(230f/255f, 37f/255f, 30f/255f, 1),
            new Color(222f/255f, 230f/255f, 18f/255f, 1),
        };

        public static Color nextColor()
        {
            idx++;
            if (idx >= colors.Length)
            {
                idx = 0;
            }

            return colors[idx];
        }
    }

}