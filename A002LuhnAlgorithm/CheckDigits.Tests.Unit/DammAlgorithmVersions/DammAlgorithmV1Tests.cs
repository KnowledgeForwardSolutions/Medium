namespace CheckDigits.Tests.Unit.DammAlgorithmVersions;

public class DammAlgorithmV1Tests
{
   [Theory]
   [InlineData("5724")]                      // Worked example from Wikipedia
   [InlineData("112946")]                    // Test data from https://www.rosettacode.org/wiki/Damm_algorithm#C#
   public void ValidateCheckDigit_ShouldReturnTrue_WhenInputContainsValidCheckDigit(String value)
         => DammAlgorithmV1.ValidateCheckDigit(value).Should().BeTrue();
}
