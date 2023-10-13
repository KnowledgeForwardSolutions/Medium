// Ignore Spelling: Isin

using System.Text;

namespace CheckDigits.IsinAlgorithmVersions;

public static class IsinAlgorithmV2
{
   private const Int32 _expectedLength = 12;

   public static bool ValidateCheckDigit(String str)
   {
      if (String.IsNullOrEmpty(str) || str.Length != _expectedLength)
      {
         return false;
      }

      var sb = new StringBuilder();
      foreach (var ch in str)
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
            return false;
         }
      }

      return LuhnAlgorithm.ValidateCheckDigit(sb.ToString());
   }
}
