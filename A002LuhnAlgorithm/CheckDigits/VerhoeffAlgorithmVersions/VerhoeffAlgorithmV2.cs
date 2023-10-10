// Ignore Spelling: Verhoeff

namespace CheckDigits.VerhoeffAlgorithmVersions;

public static class VerhoeffAlgorithmV2
{
   private static readonly ImmutableArray<ImmutableArray<Int32>> _multiplicationTable = VerhoeffTables.MultiplicationTable;
   private static readonly ImmutableArray<ImmutableArray<Int32>> _permutationTable = VerhoeffTables.PermutationTable;

   public static Boolean ValidateCheckDigit(String str)
   {
      var c = 0;
      var i = 0;
      for (var index = str.Length - 1; index >= 0; index--)
      {
         var n = str![index] - '0';
         var p = _permutationTable[i % 8][n];
         c = _multiplicationTable[c][p];

         i++;
      }
      return c == 0;
   }
}

public static class VerhoeffTables
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

   public static ImmutableArray<ImmutableArray<Int32>> PermutationTable => ImmutableArray.Create(
      ImmutableArray.Create(0, 1, 2, 3, 4, 5, 6, 7, 8, 9),
      ImmutableArray.Create(1, 5, 7, 6, 2, 8, 3, 0, 9, 4),
      ImmutableArray.Create(5, 8, 0, 3, 7, 9, 6, 1, 4, 2),
      ImmutableArray.Create(8, 9, 1, 6, 0, 4, 3, 5, 2, 7),
      ImmutableArray.Create(9, 4, 5, 3, 1, 2, 6, 8, 7, 0),
      ImmutableArray.Create(4, 2, 8, 6, 5, 7, 3, 9, 0, 1),
      ImmutableArray.Create(2, 7, 9, 3, 8, 0, 6, 4, 1, 5),
      ImmutableArray.Create(7, 0, 4, 6, 9, 1, 3, 2, 5, 8));
}
