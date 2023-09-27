// Ignore Spelling: Ean

namespace CheckDigits.EanAlgorithmVersions;

public static class EanAlgorithmV3
{
   public static Boolean ValidateCheckDigit(String str)
   {
      if (String.IsNullOrEmpty(str) || str.Length < 2)
      {
         return false;
      }

      var sumOdd = 0;
      var sumEven = 0;
      var oddPosition = true;
      for (var index = str.Length - 2; index >= 0; index--)
      {
         var currentDigit = str[index] - '0';
         if (currentDigit < 0 || currentDigit > 9)
         {
            return false;
         }
         //_ = oddPosition ? sumOdd += currentDigit : sumEven += currentDigit;
         if (oddPosition)
         {
            sumOdd += currentDigit;
         }
         else
         {
            sumEven += currentDigit;
         }
         oddPosition = !oddPosition;
      }
      var mod = ((sumOdd * 3) + sumEven) % 10;
      var checkDigit = mod == 0 ? 0 : 10 - mod;

      return str[^1] - '0' == checkDigit;
   }
}
