// Ignore Spelling: sut

namespace CheckDigits.Tests.Unit.Iso7064AlgorithmVersions;

public class Iso7064PureMod11_2Tests
{
   private static readonly IValidator _mod11_2 = new Iso7064PureMod11_2();

   public static TheoryData<IValidator, String> ValidateSuccessData => new()
   {
      { _mod11_2, "07940" },              // Example from ISO/IEC 7064 standard
      { _mod11_2, "0000000114767411" },   // ISNI for Wheaton, Wil from https://isni.org/page/search-database/
   };

   [Theory]
   [MemberData(nameof(ValidateSuccessData))]
   public void Validate_ShouldReturnTrue_WhenInputContainsValidCheckCharacters(IValidator sut, String input)
      => sut.Validate(input).Should().BeTrue();

   public static TheoryData<IValidator, String> ValidateSupplementalCharSuccessData => new()
   {
      { _mod11_2, "079X" },               // Example from ISO/IEC 7064 standard
      { _mod11_2, "000000012095650X" },   // ISNI for Lucas, George from https://isni.org/page/search-database/
   };

   [Theory]
   [MemberData(nameof(ValidateSupplementalCharSuccessData))]
   public void Validate_ShouldReturnTrue_WhenInputContainsValidSupplementalCheckCharacters(IValidator sut, String input)
      => sut.Validate(input).Should().BeTrue();

   public static TheoryData<IValidator, String> ValidateDetectableFailureData => new()
   {
      { _mod11_2, "07950" },              // 07040 with single transcription error 4 -> 5
      { _mod11_2, "0000000141767411" },   // 0000000114767411 with two digit transposition error 14 -> 41
      { _mod11_2, "000000012065950X" },   // 000000012095650X with jump transposition error 956 -> 659
      { _mod11_2, "70400" },              // 07040 with circular shift error
   };

   [Theory]
   [MemberData(nameof(ValidateDetectableFailureData))]
   public void Validate_ShouldReturnFalse_WhenInputContainsDetectableError(IValidator sut, String input)
      => sut.Validate(input).Should().BeFalse();

   public static TheoryData<IValidator, String> CharacterMappingData => new()
   {
      { _mod11_2, "01" },
      { _mod11_2, "95" },
   };

   [Theory]
   [MemberData(nameof(CharacterMappingData))]
   public void Validate_ShouldReturnTrue_WhenCharacterValuesAreCorrectlyMapped(IValidator sut, String input)
      => sut.Validate(input).Should().BeTrue();

   public static TheoryData<IValidator, String> SupplementalCharacterMappingData => new()
   {
      { _mod11_2, "1X" },
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
      => _mod11_2.Validate(String.Empty).Should().BeFalse();

   [Fact]
   public void Validate_ShouldReturnFalse_WhenInputHasLengthLessThanTwo()
      => _mod11_2.Validate("X").Should().BeFalse();

   public static TheoryData<IValidator, String> InvalidCharacterData => new()
   {
      { _mod11_2, "07#55" },
      { _mod11_2, "07=55" },
      { _mod11_2, "07A55" },
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
