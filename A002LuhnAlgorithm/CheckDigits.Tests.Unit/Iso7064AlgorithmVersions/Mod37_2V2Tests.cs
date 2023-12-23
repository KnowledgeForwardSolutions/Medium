// Ignore Spelling: sut

namespace CheckDigits.Tests.Unit.Iso7064AlgorithmVersions;

public class Mod37_2V2Tests
{
   private static readonly Mod37_2V2 _sut = new();

   [Theory]
   [InlineData("A999922123459H")]         // Example ISBT from https://www.isbt128.org/_files/ugd/79eb0b_1a92d4e286af404183d03bf5bab9120f.pdf 
   [InlineData("A999922654321S")]         // "
   [InlineData("A999914123456N")]         // Example ISBT from https://www.isbt128.org/_files/ugd/83d6e1_9c7ba55fbdd44a80947bc310cdd92382.pdf
   public void Validate_ShouldReturnTrue_WhenInputContainsValidCheckCharacters(String input)
      => _sut.Validate(input).Should().BeTrue();

   [Theory]
   [InlineData("A999922012346*")]         // Example ISBT from https://www.isbt128.org/_files/ugd/79eb0b_1a92d4e286af404183d03bf5bab9120f.pdf
   public void Validate_ShouldReturnTrue_WhenInputContainsValidSupplementalCheckCharacters(String input)
      => _sut.Validate(input).Should().BeTrue();

   [Theory]
   [InlineData("A999922123559H")]         // A999922123459H with single transcription error 4 -> 5
   [InlineData("B999922123459H")]         // A999922123459H with single transcription error A -> B
   [InlineData("A999292654321S")]         // A999922654321S with two digit transposition error 92 -> 29
   [InlineData("9A99914123456N")]         // A999914123456N with two character transposition error A9 -> 9A
   [InlineData("A999419123456N")]         // A999914123456N with jump transposition error 914 -> 419
   [InlineData("99A9922654321S")]         // A999922654321S with jump transposition error A99 -> 99A
   [InlineData("A999933123459H")]         // A999922123459H with two digit twin error 22 -> 33
   [InlineData("HA999922123459")]         // A999922123459H with circular shift error
   [InlineData("999922123459HH")]         // A999922123459H with circular shift error
   public void Validate_ShouldReturnFalse_WhenInputContainsDetectableError(String input)
      => _sut.Validate(input).Should().BeFalse();

   [Theory]
   [InlineData("01")]
   [InlineData("9K")]
   [InlineData("AI")]
   [InlineData("Z5")]
   public void Validate_ShouldReturnTrue_WhenCharacterValuesAreCorrectlyMapped(String input)
      => _sut.Validate(input).Should().BeTrue();

   [Theory]
   [InlineData("1*")]
   public void Validate_ShouldReturnTrue_WhenSupplementalCharacterValuesAreCorrectlyMapped(String input)
      => _sut.Validate(input).Should().BeTrue();

   [Fact]
   public void Validate_ShouldReturnFalse_WhenInputIsNull()
      => _sut.Validate(null!).Should().BeFalse();

   [Fact]
   public void Validate_ShouldReturnFalse_WhenInputIsEmpty()
      => _sut.Validate(String.Empty).Should().BeFalse();

   [Fact]
   public void Validate_ShouldReturnFalse_WhenInputHasLengthLessThanTwo()
      => _sut.Validate("1").Should().BeFalse();


   [Theory]
   [InlineData("K1M#L34G")]
   [InlineData("K1M=L34G")]
   [InlineData("K1M^L34G")]
   public void Validate_ShouldReturnFalse_WhenInputContainsInvalidCharacter(String input)
      => _sut.Validate(input).Should().BeFalse();

   [Theory]
   [InlineData("K1ML34G#")]
   [InlineData("K1ML34G=")]
   [InlineData("K1ML34G^")]
   public void Validate_ShouldReturnFalse_WhenInputContainsInvalidCheckCharacter(String input)
      => _sut.Validate(input).Should().BeFalse();

}
