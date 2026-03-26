using System;
using System.Globalization;
using UnityEngine;

public static class NumberFormatter
{
    private static readonly string[] suffixes = { "", "K", "M", "B", "T" };

    public static string FormatNumber(float value, int decimals = 2, bool floorDecimals = true)
    {
        if (value < 1000) return value.ToString("0");

        int suffixIndex = 0;
        while (value >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            value /= 1000;
            suffixIndex++;
        }

        if (floorDecimals)
        {
            float factor = Mathf.Pow(10, decimals);
            value = Mathf.Floor(value * factor) / factor;
        }
        return value.ToString($"0.##", CultureInfo.InvariantCulture) + suffixes[suffixIndex];
    }
    
    public static string FormatDecimalNumber(float value, int decimals = 2, bool floorDecimals = true)
    {
        int suffixIndex = 0;
        while (value >= 1000 && suffixIndex < suffixes.Length - 1)
        {
            value /= 1000;
            suffixIndex++;
        }

        if (floorDecimals)
        {
            float factor = Mathf.Pow(10, decimals);
            value = Mathf.Floor(value * factor) / factor;
        }
        return value.ToString($"0.##", CultureInfo.InvariantCulture) + suffixes[suffixIndex];
    }
} 
