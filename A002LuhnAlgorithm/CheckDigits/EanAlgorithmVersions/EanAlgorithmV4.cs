// Ignore Spelling: Ean

namespace CheckDigits.EanAlgorithmVersions;

public static class EanAlgorithmV4
{
   private static readonly Int32[] _triples = new Int32[] { 0, 3, 6, 9, 12, 15, 18, 21, 24, 27 };

   public static Boolean ValidateCheckDigit(String str)
   {
      if (String.IsNullOrEmpty(str) || str.Length < 2)
      {
         return false;
      }

      var sum = 0;
      var oddPosition = true;
      for (var index = str.Length - 2; index >= 0; index--)
      {
         var currentDigit = str[index] - '0';
         if (currentDigit < 0 || currentDigit > 9)
         {
            return false;
         }
         sum += oddPosition ? _triples[currentDigit] : currentDigit;
         oddPosition = !oddPosition;
      }
      var mod = sum % 10;
      var checkDigit = mod == 0 ? 0 : 10 - mod;

      return str[^1] - '0' == checkDigit;
   }
}
