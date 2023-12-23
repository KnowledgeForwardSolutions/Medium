namespace CheckDigits.Iso7064AlgorithmVersions;

public class Mod37_2V3
{
   private const String _alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ*";
   private const Int32 _modulus = 37;
   private const Int32 _radix = 2;
   private const Int32 _reduceThreshold = Int32.MaxValue / _radix;

   private static readonly IAlphabet _alphabet1 = new AlphanumericSupplementalAlphabet();

   public Boolean Validate(String str)
   {
      if (String.IsNullOrEmpty(str) || str.Length < 2)
      {
         return false;
      }

      Int32 num;
      var sum = 0;
      for (var index = 0; index < str.Length - 1; index++)
      {
         num = _alphabet.IndexOf(str[index]);
         if (num == -1)
         {
            return false;
         }
         sum = ((sum + num) * _radix) % _modulus;
      }

      num = _alphabet.IndexOf(str[^1]);
      if (num == -1)
      {
         return false;
      }
      sum += num;

      return sum % _modulus == 1;
   }

   public Boolean Validate_Alphabet(String str)
   {
      if (String.IsNullOrEmpty(str) || str.Length < 2)
      {
         return false;
      }

      Int32 num;
      var sum = 0;
      for (var index = 0; index < str.Length - 1; index++)
      {
         num = _alphabet1.ToIntegerEquivalent(str[index]);
         if (num == -1)
         {
            return false;
         }
         sum = ((sum + num) * _radix) % _modulus;
      }

      num = _alphabet1.ToIntegerEquivalent(str[^1]);
      if (num == -1)
      {
         return false;
      }
      sum += num;

      return sum % _modulus == 1;
   }

   public Boolean Validate_Reduce(String str)
   {
      if (String.IsNullOrEmpty(str) || str.Length < 2)
      {
         return false;
      }

      Int32 num;
      var sum = 0;
      for (var index = 0; index < str.Length - 1; index++)
      {
         num = _alphabet1.ToIntegerEquivalent(str[index]);
         if (num == -1)
         {
            return false;
         }
         sum = (sum + num) * _radix;
         if (sum > _reduceThreshold)
         {
            sum %= _modulus;
         }
      }

      num = _alphabet1.ToIntegerEquivalent(str[^1]);
      if (num == -1)
      {
         return false;
      }
      sum += num;

      return sum % _modulus == 1;
   }

   public Boolean Validate_LocalAlphabet(String str)
   {
      if (String.IsNullOrEmpty(str) || str.Length < 2)
      {
         return false;
      }

      Char ch;
      Int32 num;
      var sum = 0;
      for (var index = 0; index < str.Length - 1; index++)
      {
         ch = str[index];
         num = ch >= '0' && ch <= '9'
         ? ch - '0'
         : ch >= 'A' && ch <= 'Z'
            ? ch - 'A' + 10
            : -1;
         if (num == -1)
         {
            return false;
         }
         sum = (sum + num) * _radix;
         if (sum > _reduceThreshold)
         {
            sum %= _modulus;
         }
      }

      ch = str[^1];
      num = ch >= '0' && ch <= '9'
      ? ch - '0'
      : ch >= 'A' && ch <= 'Z'
         ? ch - 'A' + 10
         : ch == '*' ? 36 : -1;
      if (num == -1)
      {
         return false;
      }
      sum += num;

      return sum % _modulus == 1;
   }

}