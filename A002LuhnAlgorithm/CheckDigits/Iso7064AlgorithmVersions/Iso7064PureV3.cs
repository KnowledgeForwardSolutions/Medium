namespace CheckDigits.Iso7064AlgorithmVersions;

public class Iso7064PureV3 : IValidator
{
   private readonly IAlphabet _alphabet;
   private readonly Int32 _modulus;
   private readonly Int32 _radix;

   public Iso7064PureV3(IAlphabet alphabet, Int32 modulus, Int32 radix)
   {
      _alphabet = alphabet;
      _modulus = modulus;
      _radix = radix;
   }

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
         num = _alphabet.ToIntegerEquivalent(str[index]);
         if (num == -1)
         {
            return false;
         }
         sum = ((sum + num) * _radix) % _modulus;
      }

      num = _alphabet.ToIntegerEquivalent(str[^1]);
      if (num == -1)
      {
         return false;
      }
      sum += num;

      return sum % _modulus == 1;
   }
}
