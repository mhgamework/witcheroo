using UnityEngine;

namespace Assets.Modules.Utilities
{
    public static class ColorHelper
    {
    public static Color[] Colors6;

    static ColorHelper()
    {
        Colors6 = new[] { Color.red, Color.blue, Color.green, Color.yellow, Color.gray, Color.magenta };
    }
    }
}