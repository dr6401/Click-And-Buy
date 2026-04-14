using UnityEngine;

public class ColorAdjuster : MonoBehaviour
{
    public static Color DarkenColor(Color color, float percentage)
    {
        Color.RGBToHSV(color, out float h, out float s, out float v);

        v *= percentage;
        v = Mathf.Clamp01(v);
        
        Color darker = Color.HSVToRGB(h, s, v);
        darker.a = color.a;
        
        return darker;
    }
}
