// Ignore Spelling: Isin

namespace CheckDigits.IsinAlgorithmVersions;

public static class IsinAlgorithmV3
{
   private const Int32 _expectedLength = 12;
   private static readonly Int32[] _doubledValues = new Int32[] { 0, 2, 4, 6, 8, 1, 3, 5, 7, 9 };

   public static bool ValidateCheckDigit(string str)
   {
      if (String.IsNullOrEmpty(str) || str.Length != _expectedLength)
      {
         return false;
      }

      var sum = 0;
      var oddPosition = true;
      for (var index = str.Length - 2; index >= 0; index--)
      {
         var ch = str[index];
         if (ch >= '0' && ch <= '9')
         {
            var digit = ch - '0';
            sum += oddPosition ? _doubledValues[digit] : digit;
            oddPosition = !oddPosition;
         }
         else if (ch >= 'A' && ch <= 'Z')
         {
            var number = ch - 55;
            var firstDigit = number / 10;
            var secondDigit = number % 10;
            sum += oddPosition
               ? firstDigit + _doubledValues[secondDigit]
               : _doubledValues[firstDigit] + secondDigit;
         }
         else
         {
            return false;
         }
      }
      var checkDigit = (10 - (sum % 10)) % 10;

      return str[^1] - '0' == checkDigit;
   }
}
