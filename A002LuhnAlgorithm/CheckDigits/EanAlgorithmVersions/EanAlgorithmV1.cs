// Ignore Spelling: Ean

namespace CheckDigits.EanAlgorithmVersions;

public static class EanAlgorithmV1
{
   public static Boolean ValidateCheckDigit(String str)
   {
      var sum = 0;
      var oddPosition = true;
      for(var index = str.Length - 2; index >= 0; index--)
      {
         var currentDigit = str[index] - '0';
         sum += oddPosition ? currentDigit * 3 : currentDigit;
         oddPosition = !oddPosition;
      }
      var mod = sum % 10;
      var checkDigit = mod == 0 ? 0 : 10 - mod;

      return str[^1] - '0' == checkDigit;
   }
}
