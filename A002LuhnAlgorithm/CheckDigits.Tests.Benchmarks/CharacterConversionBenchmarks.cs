namespace CheckDigits.Tests.Benchmarks;

[MemoryDiagnoser]
public class CharacterConversionBenchmarks
{
   private const String _str = "1234567897";
   private const Int32 _index = 2;

   [Benchmark]
   public void ThrowAway()
   {
      _ = (Int32)Char.GetNumericValue(_str, _index);
   }

   [Benchmark]
   public void GetNumericValue()
   {
      _ = (Int32)Char.GetNumericValue(_str, _index);
   }

   [Benchmark]
   public void OldSchool()
   {
      _ = _str[_index] - '0';
   }
}
