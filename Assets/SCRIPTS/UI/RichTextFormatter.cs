using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RichTextFormatter
{
    private static Dictionary<string, string> Legend = new()
    {
        { "{green}", $"<color=#{ColorUtility.ToHtmlStringRGB(GameConstants.greenColor)}>" },
        { "{/green}", "</color>" },

        { "{red}", $"<color=#{ColorUtility.ToHtmlStringRGB(GameConstants.redColor)}>" },
        { "{/red}", "</color>" },

        { "{token}", $"<color=#{ColorUtility.ToHtmlStringRGB(GameConstants.tokenColor)}>" },
        { "{/token}", "</color>" },

        { "{divine}", $"<color=#{ColorUtility.ToHtmlStringRGB(GameConstants.divineBlessingColor)}>" },
        { "{/divine}", "</color>" },
        
        { "{combo}", $"<color=#{ColorUtility.ToHtmlStringRGB(GameConstants.comboColor)}>" },
        { "{/combo}", "</color>" }
    };

    public static string Format(string text)
    {
        foreach (var pair in Legend)
        {
            text = text.Replace(pair.Key, pair.Value);
        }
        return text;
    }

    public static string RemoveRichText(string text)
    {
        return Regex.Replace(text, "<.*?>", string.Empty);
    }
}
