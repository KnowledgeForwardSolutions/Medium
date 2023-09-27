// Ignore Spelling: Luhn

namespace CheckDigits.Tests.Unit.LuhnAlgorithmVersions;

public class LuhnAlgorithmV4Tests
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
    public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsValidCheckDigit(string value)
          => LuhnAlgorithmV4.ValidateCheckDigit(value).Should().BeTrue();

    [Theory]
    [InlineData("3056930090020004")]    // Diners Club test card number with two digit transposition 09 -> 90
    [InlineData("3056930000920004")]    // Diners Club test card number with two digit transposition 90 -> 09
    [InlineData("5555555225554444")]    // MasterCard test card number with two digit twin error 55 -> 22
    [InlineData("5555555225554774")]    // MasterCard test card number with two digit twin error 44 -> 77
    [InlineData("3533111111111113")]    // JCB test card number with two digit twin error 66 -> 33
    public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsUndetectableError(string str)
       => LuhnAlgorithmV4.ValidateCheckDigit(str).Should().BeTrue();

    [Theory]
    [InlineData("5558555555554444")]    // MasterCard test card number with single digit transcription error 5 -> 8
    [InlineData("5558555555554434")]    // MasterCard test card number with single digit transcription error 4 -> 3
    [InlineData("3059630009020004")]    // Diners Club test card number with two digit transposition error 69 -> 96 
    [InlineData("3056930009002004")]    // Diners Club test card number with two digit transposition error 20 -> 02
    [InlineData("5559955555554444")]    // MasterCard test card number with two digit twin error 55 -> 99
    [InlineData("3566111144111113")]    // JCB test card number with two digit twin error 11 -> 44
    public void ValidateCheckDigit_ShouldReturnFalse_WhenInputContainsDetectableError(string value)
       => LuhnAlgorithmV4.ValidateCheckDigit(value).Should().BeFalse();

    [Fact]
    public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsNull()
       => LuhnAlgorithmV4.ValidateCheckDigit(null!).Should().BeFalse();

    [Fact]
    public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsEmpty()
       => LuhnAlgorithmV4.ValidateCheckDigit(string.Empty).Should().BeFalse();

    [Fact]
    public void ValidateCheckDigit_ShouldReturnTrue_WhenInputIsAllZeros()
       => LuhnAlgorithmV4.ValidateCheckDigit("0000000000000000").Should().BeTrue();

    [Fact]
    public void ValidateCheckDigit_ShouldReturnTrue_WhenCheckDigitIsCalculatesAsZero()
       => LuhnAlgorithmV4.ValidateCheckDigit("7624810").Should().BeTrue();
}
