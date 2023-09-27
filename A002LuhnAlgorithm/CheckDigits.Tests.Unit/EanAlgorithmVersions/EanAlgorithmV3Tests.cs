// Ignore Spelling: Ean

namespace CheckDigits.Tests.Unit.EanAlgorithmVersions;

public class EanAlgorithmV3Tests
{
   [Theory]
   [InlineData("000000000017")]
   [InlineData("000000001007")]
   [InlineData("000000100007")]
   [InlineData("000010000007")]
   [InlineData("001000000007")]
   [InlineData("100000000007")]
   public void ValidateCheckDigit_ShouldCorrectlyApplyOddPositionWeight(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeTrue();

   [Theory]
   [InlineData("000000000109")]
   [InlineData("000000010009")]
   [InlineData("000001000009")]
   [InlineData("000100000009")]
   [InlineData("010000000009")]
   public void ValidateCheckDigit_ShouldCorrectlyApplyEvenPositionWeight(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeTrue();

   [Theory]
   [InlineData("000000000000")]        // sum mod 10 = 0
   [InlineData("000000090100")]        // "
   [InlineData("000000000901")]        // sum mod 10 != 0
   [InlineData("000000000802")]        // "
   [InlineData("000000000703")]        // "
   [InlineData("000000000604")]        // "
   [InlineData("000000000505")]        // "
   [InlineData("000000000406")]        // "
   [InlineData("000000000307")]        // "
   [InlineData("000000000208")]        // "
   [InlineData("000000000109")]        // "
   public void ValidateCheckDigit_ShouldCorrectlyHandleMod10Results(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeTrue();

   [Theory]
   [InlineData("036000291452")]        // Worked UPC-A example from Wikipedia (https://en.wikipedia.org/wiki/Universal_Product_Code#Check_digit_calculation)
   [InlineData("425261")]              // UPC-E example
   [InlineData("4006381333931")]       // Worked EAN-13 example from Wikipedia (https://en.wikipedia.org/wiki/International_Article_Number)
   [InlineData("73513537")]            // Worked EAN-8 example from Wikipedia
   [InlineData("9780500516959")]       // ISBN-13, Islamic Geometric Design, Eric Broug
   [InlineData("012345678000045678")]  // Example SSCC number
   public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsValidCheckDigit(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeTrue();

   [Theory]
   [InlineData("4006831333931")]       // EAN-13 with two digit transposition error (38 -> 83) where difference between digits is 5 
   [InlineData("9785000516959")]       // ISBN-13 with two digit transposition error (05 -> 50) where difference between digits is 5 
   [InlineData("73315537")]            // EAN-8 with jump transposition error (515 -> 315)
   [InlineData("012345876000045678")]  // SSCC number with jump transposition error (678 -> 876)
   public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsUndetectableError(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeTrue();

   [Theory]
   [InlineData("036000391452")]        // UPC-A with single digit transcription error (2 -> 3)
   [InlineData("427261")]              // UPC-E with single digit transcription error (5 -> 7)
   [InlineData("4006383133931")]       // EAN-13 with two digit transposition error (13 -> 31)
   [InlineData("9870500516959")]       // ISBN-13 with two digit transposition error (78 -> 87)
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputContainsDetectableError(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeFalse();

   [Fact]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsNull()
      => EanAlgorithmV2.ValidateCheckDigit(null!).Should().BeFalse();

   [Fact]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsEmpty()
      => EanAlgorithmV2.ValidateCheckDigit(String.Empty).Should().BeFalse();

   [Theory]
   [InlineData("0")]
   [InlineData("1")]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputIsOneCharacterInLength(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeFalse();

   [Theory]
   [InlineData("42+261")]              // UPC-E example with 5 replaced with character 10 positions before in ASCII table
   [InlineData("42I261")]              // UPC-E example with 5 replaced with character 20 positions later in ASCII table
   [InlineData("0 36000 29145 2")]
   public void ValidateCheckDigit_ShouldReturnFalse_WhenInputContainsNonDigitCharacter(String value)
      => EanAlgorithmV2.ValidateCheckDigit(value).Should().BeFalse();
}
