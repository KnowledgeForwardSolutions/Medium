// Ignore Spelling: Verhoeff

namespace CheckDigits.Tests.Unit.VerhoeffAlgorithmVersions;

public class VerhoeffAlgorithmV3Tests
{
   [Theory]
   [InlineData("2363")]                      // Worked example from Wikipedia
   [InlineData("123451")]                    // Test data from https://rosettacode.org/wiki/Verhoeff_algorithm
   [InlineData("1234567890120")]             // "
   [InlineData("758722")]                    // Test data from https://codereview.stackexchange.com/questions/221229/verhoeff-check-digit-algorithm
   [InlineData("1428570")]                   // "
   [InlineData("84736430954837284567892")]   // "
   [InlineData("112233445566778899009")]     // Value calculated by https://kik.amc.nl/home/rcornet/verhoeff.html
   public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsValidCheckDigit(String value)
         => VerhoeffAlgorithmV3.ValidateCheckDigit(value).Should().BeTrue();
}
