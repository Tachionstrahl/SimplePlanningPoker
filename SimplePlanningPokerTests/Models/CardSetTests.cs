using SimplePlanningPoker.Models;

namespace SimplePlanningPokerTests;

public class CardSetTests
{
    [Fact]
        public void IsValidValue_ReturnsTrue_WhenValueIsValid()
        {
            // Arrange
            var cardSet = new CardSet
            {
                Name = "Fibonacci",
                Values = new List<string> { "0", "1", "2", "3", "5", "8", "13", "21" }
            };

            // Act
            var result = cardSet.IsValidValue("5");

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValidValue_ThrowsArgumentNullException_WhenValueIsNull()
        {
            // Arrange
            var cardSet = new CardSet
            {
                Name = "Fibonacci",
                Values = new List<string> { "0", "1", "2", "3", "5", "8", "13", "21" }
            };

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => cardSet.IsValidValue(null));
        }

        [Fact]
        public void IsValidValue_ReturnsFalse_WhenValueIsNotValid()
        {
            // Arrange
            var cardSet = new CardSet
            {
                Name = "Fibonacci",
                Values = new List<string> { "0", "1", "2", "3", "5", "8", "13", "21" }
            };

            // Act
            var result = cardSet.IsValidValue("4");

            // Assert
            Assert.False(result);
        }
}
