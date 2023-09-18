// Ignore Spelling: Luhn

namespace CheckDigits.Tests.Unit;

public class LuhnAlgorithmV8Tests
{
   [Theory]
   [InlineData("26")]
   [InlineData("75")]
   [InlineData("133")]
   [InlineData("5555555555554444")]    // MasterCard test credit card number
   [InlineData("4012888888881881")]    // Visa test credit card number
   [InlineData("3056930009020004")]    // Diners Club test credit card number
   [InlineData("3566111111111113")]    // JCB test credit card number
   [InlineData("808401234567893")]     // NPI (National Provider Identifier) 
   [InlineData("490154203237518")]     // IMEI (International Mobile Equipment Identity)   
   public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsValidCheckDigit(String value)
         => LuhnAlgorithmV8.ValidateCheckDigit(value).Should().BeTrue();

   [Theory]
   [InlineData("3056930090020004")]    // Diners Club test card number with two digit transposition 09 -> 90
   [InlineData("3056930000920004")]    // Diners Club test card number with two digit transposition 90 -> 09
   [InlineData("5555555225554444")]    // MasterCard test card number with two digit twin error 55 -> 22
   [InlineData("5555555225554774")]    // MasterCard test card number with two digit twin error 44 -> 77
   [InlineData("3533111111111113")]    // JCB test card number with two digit twin error 66 -> 33
   public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsUndetectableError(String str)
      => LuhnAlgorithmV8.ValidateCheckDigit(str).Should().BeTrue();

   [Theory]
   [InlineData("5558555555554444")]    // MasterCard test card number with single digit transcription error 5 -> 8
   [InlineData("5558555555554434")]    // MasterCard test card number with single digit transcription error 4 -> 3
   [InlineData("3059630009020004")]    // Diners Club test card number with two digit transposition error 69 -> 96 
   [InlineData("3056930009002004")]    // Diners Club test card number with two digit transposition error 20 -> 02
   [InlineData("5559955555554444")]    // MasterCard test card number with two digit twin error 55 -> 99
   [InlineData("3566111144111113")]    // JCB test card number with two digit twin error 11 -> 44
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputContainsDetectableError(String value)
      => LuhnAlgorithmV8.ValidateCheckDigit(value).Should().BeFalse();

   [Fact]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsNull()
      => LuhnAlgorithmV8.ValidateCheckDigit(null!).Should().BeFalse();

   [Fact]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsEmpty()
      => LuhnAlgorithmV8.ValidateCheckDigit(String.Empty).Should().BeFalse();

   [Fact]
   public void ValidateCheckDigit_ShouldReturnTrue_WhenInputIsAllZeros()
      => LuhnAlgorithmV8.ValidateCheckDigit("0000000000000000").Should().BeTrue();

   [Fact]
   public void ValidateCheckDigit_ShouldReturnTrue_WhenCheckDigitIsCalculatesAsZero()
      => LuhnAlgorithmV8.ValidateCheckDigit("7624810").Should().BeTrue();

   [Theory]
   [InlineData("0")]
   [InlineData("1")]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsOneCharacterInLength(String value)
      => LuhnAlgorithmV8.ValidateCheckDigit(value).Should().BeFalse();

   [Theory]
   [InlineData("123A780")]
   [InlineData("123A781")]
   [InlineData("123A782")]
   [InlineData("123A783")]
   [InlineData("123A784")]
   [InlineData("123A785")]
   [InlineData("123A786")]
   [InlineData("123A787")]
   [InlineData("123A788")]
   [InlineData("123A789")]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputContainsNonDigitCharacter(String value)
      => LuhnAlgorithmV8.ValidateCheckDigit(value).Should().BeFalse();
}
