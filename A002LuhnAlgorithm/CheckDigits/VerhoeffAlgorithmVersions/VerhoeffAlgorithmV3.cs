// Ignore Spelling: Verhoeff

namespace CheckDigits.VerhoeffAlgorithmVersions;

public static class VerhoeffAlgorithmV3
{
   private static readonly MultiplicationTable _multiplicationTable = new();
   private static readonly PermutationTable _permutationTable = new();

   public static Boolean ValidateCheckDigit(String str)
   {
      var c = 0;
      var i = 0;
      for (var index = str.Length - 1; index >= 0; index--)
      {
         var n = str![index] - '0';
         var p = _permutationTable[i % 8, n];
         c = _multiplicationTable[c, p];

         i++;
      }
      return c == 0;
   }
}

public class MultiplicationTable
{
   private static readonly Int32[,] _multiplicationTable = new Int32[10, 10]
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

   public Int32 this[Int32 x, Int32 y] => _multiplicationTable[x, y];
}

public class PermutationTable
{
   private static readonly Int32[,] _permutationTable = new Int32[8, 10]
   {
      { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 },
      { 1, 5, 7, 6, 2, 8, 3, 0, 9, 4 },
      { 5, 8, 0, 3, 7, 9, 6, 1, 4, 2 },
      { 8, 9, 1, 6, 0, 4, 3, 5, 2, 7 },
      { 9, 4, 5, 3, 1, 2, 6, 8, 7, 0 },
      { 4, 2, 8, 6, 5, 7, 3, 9, 0, 1 },
      { 2, 7, 9, 3, 8, 0, 6, 4, 1, 5 },
      { 7, 0, 4, 6, 9, 1, 3, 2, 5, 8 }
   };

   public Int32 this[Int32 x, Int32 y] => _permutationTable[x, y];
}
