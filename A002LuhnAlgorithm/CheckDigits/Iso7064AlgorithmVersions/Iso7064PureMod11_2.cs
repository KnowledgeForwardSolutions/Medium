namespace CheckDigits.Iso7064AlgorithmVersions;

public class Iso7064PureMod11_2 : IValidator
{
   private const Int32 _modulus = 11;
   private const Int32 _radix = 2;
   private readonly Int32 _reduceThreshold = Int32.MaxValue / _radix;

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
         num = str[index] - '0';
         if (num < 0 || num > 9)
         {
            return false;
         }
         sum = (sum + num) * _radix;
         if (sum > _reduceThreshold)
         {
            sum %= _modulus;
         }
      }

      num = str[^1] - '0';
      if (num == 40)
      {
         num = 10;
      }
      else if (num < 0 || num > 9)
      {
         return false;
      }
      sum += num;

      return sum % _modulus == 1;
   }
}
