// Ignore Spelling: Isin

using System.Text;

namespace CheckDigits.IsinAlgorithmVersions;

public static class IsinAlgorithmV1
{
   private const String _validCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

   public static bool ValidateCheckDigit(String str)
   {
      var sb = new StringBuilder();
      foreach(var ch in str)
      {
         if (ch >= '0' && ch <= '9')
         {
            sb.Append(ch);
         }
         else if (ch >= 'A' && ch <= 'Z')
         {
            sb.Append(ch - 55);
         }
         else
         {
            throw new ArgumentOutOfRangeException(nameof(str), str);
         }
      }

      return LuhnAlgorithm.ValidateCheckDigit(sb.ToString());
   }
}
