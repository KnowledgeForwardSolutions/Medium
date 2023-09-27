// Ignore Spelling: Luhn

using CheckDigits.LuhnAlgorithmVersions;

namespace CheckDigits.Tests.Benchmarks;

[MemoryDiagnoser]
public class LuhnAlgorithmBenchmarks
{
   [Params("1234567897", "5555555555554444")]
   public String Value { get; set; } = String.Empty;

   [Benchmark(Baseline = true)]
   public void BaseLine()
   {
      _ = LuhnAlgorithmV6.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void CharConversion()
   {
      _ = LuhnAlgorithmV7.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void Lookup()
   {
      _ = LuhnAlgorithmV8.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void BitShift()
   {
      _ = LuhnAlgorithmV9.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void Addition()
   {
      _ = LuhnAlgorithmV10.ValidateCheckDigit(Value);
   }
}
