namespace CheckDigits.Iso7064AlgorithmVersions;

public class DigitsAlphabet : IAlphabet
{
   public Int32 ToIntegerEquivalent(Char ch)
      => ch >= '0' && ch <= '9' ? ch - '0' : -1;
}

public class DigitsSupplementalAlphabet : IAlphabet
{
   public Int32 ToIntegerEquivalent(Char ch)
      => ch >= '0' && ch <= '9' 
         ? ch - '0' 
         : ch == 'X' ? 10 : -1;
}

public class LettersAlphabet : IAlphabet
{
   public Int32 ToIntegerEquivalent(Char ch)
      => ch >= 'A' && ch <= 'Z' ? ch - 'A' : -1;
}

public class AlphanumericAlphabet : IAlphabet
{
   public Int32 ToIntegerEquivalent(Char ch)
      => ch >= '0' && ch <= '9' 
         ? ch - '0' 
         : ch >= 'A' && ch <= 'Z'
            ? ch - 'A' + 10
            : -1;
}

public class AlphanumericSupplementalAlphabet : IAlphabet
{
   public Int32 ToIntegerEquivalent(Char ch)
      => ch >= '0' && ch <= '9'
         ? ch - '0'
         : ch >= 'A' && ch <= 'Z'
            ? ch - 'A' + 10
            : ch == '*' ? 36 : -1;
}
