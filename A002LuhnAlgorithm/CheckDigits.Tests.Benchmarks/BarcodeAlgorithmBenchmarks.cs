// Ignore Spelling: Barcode

namespace CheckDigits.Tests.Benchmarks;

[MemoryDiagnoser]
public class BarcodeAlgorithmBenchmarks
{
   [Params("425261", "73513537", "036000291452", "4006381333931", "012345678000045678")]
   public String Value { get; set; } = String.Empty;

   [Benchmark(Baseline = true)]
   public void BaseLine()
   {
      _ = EanAlgorithmV2.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void OddEven()
   {
      _ = EanAlgorithmV3.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void Lookup()
   {
      _ = EanAlgorithmV4.ValidateCheckDigit(Value);
   }
}
