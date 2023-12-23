namespace CheckDigits.Iso7064AlgorithmVersions;

public class Mod37_2V2
{
   private readonly String _alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ*";
   private readonly Int32 _modulus = 37;
   private readonly Int32 _radix = 2;

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
}