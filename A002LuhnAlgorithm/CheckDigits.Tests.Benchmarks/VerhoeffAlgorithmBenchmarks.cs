// Ignore Spelling: Verhoeff

namespace CheckDigits.Tests.Benchmarks;

public class VerhoeffAlgorithmBenchmarks
{
   [Params("2363", "123451", "1428570", "1234567890120", "112233445566778899009", "84736430954837284567892")]
   public String Value { get; set; } = String.Empty;

   [Benchmark(Baseline = true)]
   public void BaseLine()
   {
      _ = VerhoeffAlgorithmV1.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void ImmutableArray()
   {
      _ = VerhoeffAlgorithmV2.ValidateCheckDigit(Value);
   }

   [Benchmark]
   public void ClassWrapper()
   {
      _ = VerhoeffAlgorithmV3.ValidateCheckDigit(Value);
   }
}
