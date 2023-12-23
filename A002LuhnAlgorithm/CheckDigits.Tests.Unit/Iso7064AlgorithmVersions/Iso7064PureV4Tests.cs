// Ignore Spelling: sut

namespace CheckDigits.Tests.Unit.Iso7064AlgorithmVersions;

public class Iso7064PureV4Tests
{
   private static readonly IValidator _mod11_2 = new Iso7064PureV4(new DigitsSupplementalAlphabet(), 11, 2);
   private static readonly IValidator _mod1271_36 = new Iso7064PureV4(new AlphanumericAlphabet(), 1271, 36);
   private static readonly IValidator _mod37_2 = new Iso7064PureV4(new AlphanumericSupplementalAlphabet(), 37, 2);
   private static readonly IValidator _mod661_26 = new Iso7064PureV4(new LettersAlphabet(), 661, 26);

   public static TheoryData<IValidator, String> ValidateSuccessData => new()
   {
      { _mod11_2, "07940" },              // Example from ISO/IEC 7064 standard
      { _mod11_2, "0000000114767411" },   // ISNI for Wheaton, Wil from https://isni.org/page/search-database/
      { _mod37_2, "A999922123459H" },     // Example ISBT from https://www.isbt128.org/_files/ugd/79eb0b_1a92d4e286af404183d03bf5bab9120f.pdf
      { _mod1271_36, "ISO793W" },         // Example from ISO/IEC 7064 standard
   };

   [Theory]
   [MemberData(nameof(ValidateSuccessData))]
   public void Validate_ShouldReturnTrue_WhenInputContainsValidCheckCharacters(IValidator sut, String input)
      => sut.Validate(input).Should().BeTrue();

   public static TheoryData<IValidator, String> ValidateSupplementalCharSuccessData => new()
   {
      { _mod11_2, "079X" },               // Example from ISO/IEC 7064 standard
      { _mod11_2, "000000012095650X" },   // ISNI for Lucas, George from https://isni.org/page/search-database/
      { _mod37_2, "A999922012346*" },     // Example ISBT from https://www.isbt128.org/_files/ugd/79eb0b_1a92d4e286af404183d03bf5bab9120f.pdf
   };

   [Theory]
   [MemberData(nameof(ValidateSupplementalCharSuccessData))]
   public void Validate_ShouldReturnTrue_WhenInputContainsValidSupplementalCheckCharacters(IValidator sut, String input)
      => sut.Validate(input).Should().BeTrue();

   public static TheoryData<IValidator, String> ValidateDetectableFailureData => new()
   {
      { _mod11_2, "07950" },              // 07040 with single transcription error 4 -> 5
      { _mod37_2, "B999922123459H" },     // A999922123459H with single transcription error A -> B
      { _mod11_2, "0000000141767411" },   // 0000000114767411 with two digit transposition error 14 -> 41
      { _mod1271_36, "IOS793W" },         // ISO793W with two character transposition error SO -> OS
      { _mod11_2, "000000012065950X" },   // 000000012095650X with jump transposition error 956 -> 659
      { _mod1271_36, "OSI793W" },         // ISO793W with jump transposition error ISO -> OSI
      { _mod1271_36, "I7OS93W" },         // ISO793W with jump transposition error SO7 -> 7OS
      { _mod37_2, "A999933123459H" },     // A999922123459H with two digit twin error 22 -> 33
      { _mod11_2, "70400" },              // 07040 with circular shift error
      { _mod37_2, "HA999922123459" },     // A999922123459H with circular shift error
   };

   [Theory]
   [MemberData(nameof(ValidateDetectableFailureData))]
   public void Validate_ShouldReturnFalse_WhenInputContainsDetectableError(IValidator sut, String input)
      => sut.Validate(input).Should().BeFalse();

   public static TheoryData<IValidator, String> CharacterMappingData => new()
   {
      { _mod11_2, "01" },
      { _mod11_2, "95" },
      { _mod661_26, "AZM" },
      { _mod661_26, "ZLB" },
      { _mod1271_36, "0ZC" },
      { _mod1271_36, "9T3" },
      { _mod1271_36, "ASE" },
      { _mod1271_36, "ZB1" },
   };

   [Theory]
   [MemberData(nameof(CharacterMappingData))]
   public void Validate_ShouldReturnTrue_WhenCharacterValuesAreCorrectlyMapped(IValidator sut, String input)
      => sut.Validate(input).Should().BeTrue();

   public static TheoryData<IValidator, String> SupplementalCharacterMappingData => new()
   {
      { _mod11_2, "1X" },
      { _mod37_2, "1*" },
   };

   [Theory]
   [MemberData(nameof(SupplementalCharacterMappingData))]
   public void Validate_ShouldReturnTrue_WhenSupplementalCharacterValuesAreCorrectlyMapped(IValidator sut, String input)
      => sut.Validate(input).Should().BeTrue();

   [Fact]
   public void Validate_ShouldReturnFalse_WhenInputIsNull()
      => _mod11_2.Validate(null!).Should().BeFalse();

   [Fact]
   public void Validate_ShouldReturnFalse_WhenInputIsEmpty()
      => _mod1271_36.Validate(String.Empty).Should().BeFalse();

   [Fact]
   public void Validate_ShouldReturnFalse_WhenInputHasLengthLessThanTwo()
      => _mod37_2.Validate("X").Should().BeFalse();

   public static TheoryData<IValidator, String> InvalidCharacterData => new()
   {
      { _mod11_2, "07#55" },
      { _mod11_2, "07=55" },
      { _mod11_2, "07A55" },
      { _mod661_26, "I=OHJXA" },
      { _mod661_26, "I^OHJXA" },
      { _mod661_26, "I5OHJXA" },
      { _mod37_2, "K1M#L34G" },
      { _mod37_2, "K1M=L34G" },
      { _mod37_2, "K1M^L34G" },
   };

   [Theory]
   [MemberData(nameof(InvalidCharacterData))]
   public void Validate_ShouldReturnFalse_WhenInputContainsInvalidCharacter(IValidator sut, String input)
      => sut.Validate(input).Should().BeFalse();

   public static TheoryData<IValidator, String> InvalidCheckCharacterData => new()
   {
      { _mod11_2, "001#" },
      { _mod11_2, "001=" },
      { _mod11_2, "001A" },
   };

   [Theory]
   [MemberData(nameof(InvalidCheckCharacterData))]
   public void Validate_ShouldReturnFalse_WhenInputContainsInvalidCheckCharacter(IValidator sut, String input)
      => sut.Validate(input).Should().BeFalse();

}
