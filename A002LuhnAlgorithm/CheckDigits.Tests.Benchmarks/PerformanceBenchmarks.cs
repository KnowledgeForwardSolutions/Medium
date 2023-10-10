// Ignore Spelling: Damm, Ean, Luhn, Verhoeff

using System.Runtime.CompilerServices;

using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CheckDigits.Tests.Benchmarks;

[MemoryDiagnoser]
public class PerformanceBenchmarks
{
   [Params(5, 8, 13, 16)]
   public Int32 Length { get; set; }

   private const String Damm5 = "12340";
   private const String Damm8 = "12345671";
   private const String Damm13 = "1234567890123";
   private const String Damm16 = "1234567890123450";

   private const String Ean5 = "NA";
   private const String Ean8 = "73513537";
   private const String Ean13 = "9780500516959";
   private const String Ean16 = "NA";

   private const String Luhn5 = "12344";
   private const String Luhn8 = "12345674";
   private const String Luhn13 = "1234567890128";
   private const String Luhn16 = "3056930009020004";

   private const String Verhoeff5 = "12340";
   private const String Verhoeff8 = "12345679";
   private const String Verhoeff13 = "123456789121";
   private const String Verhoeff16 = "123456789123452";

   [Benchmark]
   public void Damm()
   {
      var value = Length switch
      {
         5 => Damm5,
         8 => Damm8,
         13 => Damm13,
         16 => Damm16,
         _ => throw new SwitchExpressionException()
      };

      _ = DammAlgorithmV1.ValidateCheckDigit(value);
   }

   [Benchmark]
   public void Ean()
   {
      var value = Length switch
      {
         5 => Ean5,
         8 => Ean8,
         13 => Ean13,
         16 => Ean16,
         _ => throw new SwitchExpressionException()
      };

      _ = EanAlgorithm.ValidateCheckDigit(value);
   }

   [Benchmark]
   public void Luhn()
   {
      var value = Length switch
      {
         5 => Luhn5,
         8 => Luhn8,
         13 => Luhn13,
         16 => Luhn16,
         _ => throw new SwitchExpressionException()
      };

      _ = LuhnAlgorithm.ValidateCheckDigit(value);
   }

   [Benchmark]
   public void Verhoeff()
   {
      var value = Length switch
      {
         5 => Luhn5,
         8 => Luhn8,
         13 => Luhn13,
         16 => Luhn16,
         _ => throw new SwitchExpressionException()
      };

      _ = VerhoeffAlgorithmV3.ValidateCheckDigit(value);
   }
}
