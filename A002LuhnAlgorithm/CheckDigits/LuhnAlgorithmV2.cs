// Ignore Spelling: Luhn

namespace CheckDigits;

/// <summary>
///   Updated to handle null input.
/// </summary>
public static class LuhnAlgorithmV2
{
   public static Boolean ValidateCheckDigit(String str)
   {
      if (str is null)
      {
         return false;
      }

      var sum = 0;
      var shouldApplyDouble = true;
      for (var index = str.Length - 2; index >= 0; index--)
      {
         var currentDigit = (Int32)Char.GetNumericValue(str, index);
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
      var checkDigit = 10 - (sum % 10);

      return Char.GetNumericValue(str[^1]) == checkDigit;
   }
}
