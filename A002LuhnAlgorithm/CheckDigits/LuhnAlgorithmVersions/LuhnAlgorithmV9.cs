// Ignore Spelling: Luhn

namespace CheckDigits.LuhnAlgorithmVersions;

/// <summary>
///   Updated for bit shift benchmark.
/// </summary>
public static class LuhnAlgorithmV9
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
            var currentDigit = str[index] - '0';
            if (currentDigit < 0 || currentDigit > 9)
            {
                return false;
            }
            if (shouldApplyDouble)
            {
                if (currentDigit > 4)
                {
                    sum += (currentDigit << 1) - 9;
                }
                else
                {
                    sum += currentDigit << 1;
                }
            }
            else
            {
                sum += currentDigit;
            }
            shouldApplyDouble = !shouldApplyDouble;
        }
        var checkDigit = (10 - sum % 10) % 10;

        return str[^1] - '0' == checkDigit;
    }
}
