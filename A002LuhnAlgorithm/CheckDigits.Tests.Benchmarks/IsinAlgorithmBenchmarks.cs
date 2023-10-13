// Ignore Spelling: Isin Precalculated

namespace CheckDigits.Tests.Benchmarks;

[MemoryDiagnoser]
public class IsinAlgorithmBenchmarks
{
   [Params("US0378331005", "AU0000XVGZA3", "US88160R1014")]
   public String Value { get; set; } = String.Empty;

   //[Benchmark(Baseline = true)]
   //public void BaseLine()
   //{
   //   _ = IsinAlgorithmV1.ValidateCheckDigit(Value);
   //}

   //[Benchmark(Baseline = true)]
   //public void NoAllocation()
   //{
   //   _ = IsinAlgorithmV3.ValidateCheckDigit(Value);
   //}

   [Benchmark(Baseline = true)]
   public void Lookup()
   {
      _ = IsinAlgorithmV4.ValidateCheckDigit(Value);
   }
}
