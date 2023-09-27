﻿// Ignore Spelling: Luhn

namespace CheckDigits.LuhnAlgorithmVersions;

/// <summary>
///   Updated to handle one character input.
/// </summary>
public static class LuhnAlgorithmV5
{
    public static bool ValidateCheckDigit(string str)
    {
        if (string.IsNullOrEmpty(str) || str.Length < 2)
        {
            return false;
        }

        var sum = 0;
        var shouldApplyDouble = true;
        for (var index = str.Length - 2; index >= 0; index--)
        {
            var currentDigit = (int)char.GetNumericValue(str, index);
            if (shouldApplyDouble)
            {
                if (currentDigit > 4)
                {
                    sum += currentDigit * 2 - 9;
                }
                else
                {
                    sum += currentDigit * 2;
                }
            }
            else
            {
                sum += currentDigit;
            }
            shouldApplyDouble = !shouldApplyDouble;
        }
        var checkDigit = (10 - sum % 10) % 10;

        return char.GetNumericValue(str[^1]) == checkDigit;
    }

}
