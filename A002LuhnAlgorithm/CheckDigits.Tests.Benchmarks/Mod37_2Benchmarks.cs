namespace CheckDigits.Tests.Benchmarks;

[MemoryDiagnoser]
public class Mod37_2Benchmarks
{
   private static readonly Mod37_2V3 _sut = new Mod37_2V3();

   [Params("K1M0W", "K1MEL37654L", "K1MEL37655H24EDRD")]
   public String Value { get; set; } = String.Empty;

   [Benchmark(Baseline = true)]
   public void Baseline()
   {
      _ = _sut.Validate(Value);
   }

   [Benchmark]
   public void Alphabet()
   {
      _ = _sut.Validate_Alphabet(Value);
   }

   [Benchmark]
   public void Reduce()
   {
      _ = _sut.Validate_Reduce(Value);
   }

   [Benchmark]
   public void LocalAlphabet()
   {
      _ = _sut.Validate_LocalAlphabet(Value);
   }
}
