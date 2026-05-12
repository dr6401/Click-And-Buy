using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class RichTextFormatter
{
    private static Dictionary<string, string> Legend = new()
    {
        { "{green}", $"{GameConstants.greenColor}" },
        { "{/green}", "</color>" },

        { "{loss}", "<color=#FF414A>" },
        { "{/loss}", "</color>" },

        { "{combo}", "<color=#FFD95E>" },
        { "{/combo}", "</color>" },

        { "{token}", "<color=#5EC8FF>" },
        { "{/token}", "</color>" },

        { "{divine}", "<color=#C77DFF>" },
        { "{/divine}", "</color>" }
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
