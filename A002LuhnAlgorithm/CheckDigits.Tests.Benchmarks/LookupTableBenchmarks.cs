namespace CheckDigits.Tests.Benchmarks;

[MemoryDiagnoser]
public class LookupTableBenchmarks
{
   private static readonly Int32[,] _array = new Int32[10, 10]
   {
      { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
      { 1, 2, 3, 4, 0, 6, 7, 8, 9, 5 },
      { 2, 3, 4, 0, 1, 7, 8, 9, 5, 6 },
      { 3, 4, 0, 1, 2, 8, 9, 5, 6, 7 },
      { 4, 0, 1, 2, 3, 9, 5, 6, 7, 8 },
      { 5, 9, 8, 7, 6, 0, 4, 3, 2, 1 },
      { 6, 5, 9, 8, 7, 1, 0, 4, 3, 2 },
      { 7, 6, 5, 9, 8, 2, 1, 0, 4, 3 },
      { 8, 7, 6, 5, 9, 3, 2, 1, 0, 4 },
      { 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 }
   };

   private static readonly ImmutableArray<ImmutableArray<Int32>> _immutableArray = LookupTable.MultiplicationTable;

   private static readonly Int32[] _x = Enumerable.Range(1, 100).Select(x => Random.Shared.Next(0, 9)).ToArray();
   private static readonly Int32[] _y = Enumerable.Range(1, 100).Select(x => Random.Shared.Next(0, 9)).ToArray();

   [Benchmark(Baseline = true)]
   public void Baseline()
   {
      for(var index = 0; index < 100; index++)
      {
         _ = _array[_x[index], _y[index]];
      }
   }

   [Benchmark] //(Baseline = true)]
   public void Array()
   {
      for (var index = 0; index < 100; index++)
      {
         _ = _array[_x[index], _y[index]];
      }
   }

   [Benchmark]
   public void ImmutableArray()
   {
      for (var index = 0; index < 100; index++)
      {
         _ = _immutableArray[_x[index]][_y[index]];
      }
   }
}

public static class LookupTable
{
   public static ImmutableArray<ImmutableArray<Int32>> MultiplicationTable => ImmutableArray.Create(
      ImmutableArray.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
      ImmutableArray.Create(1, 2, 3, 4, 0, 6, 7, 8, 9, 5),
      ImmutableArray.Create(2, 3, 4, 0, 1, 7, 8, 9, 5, 6),
      ImmutableArray.Create(3, 4, 0, 1, 2, 8, 9, 5, 6, 7),
      ImmutableArray.Create(4, 0, 1, 2, 3, 9, 5, 6, 7, 8),
      ImmutableArray.Create(5, 9, 8, 7, 6, 0, 4, 3, 2, 1),
      ImmutableArray.Create(6, 5, 9, 8, 7, 1, 0, 4, 3, 2),
      ImmutableArray.Create(7, 6, 5, 9, 8, 2, 1, 0, 4, 3),
      ImmutableArray.Create(8, 7, 6, 5, 9, 3, 2, 1, 0, 4),
      ImmutableArray.Create(9, 8, 7, 6, 5, 4, 3, 2, 1, 0));
}
