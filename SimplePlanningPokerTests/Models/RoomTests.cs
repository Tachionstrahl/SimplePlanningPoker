using SimplePlanningPoker.Models;

namespace SimplePlanningPokerTests
{

	public class RoomTests
	{
		public RoomTests()
		{
		}

		[Fact]
		public void AddParticipant_WithNewUser_ReturnsTrue()
		{
			// Arrange
			var room = new Room("123");
			var user = new User("abc", "Bob");

			// Act
			var result = room.AddParticipant(user);

			// Assert 
			Assert.True(result);
			var participants = room.GetAllParticipants();
			Assert.Single(participants);
		}

		[Fact]
		public void AddParticipant_WithExistingUser_ReturnsFalse()
		{
			// Arrange
			var room = new Room("123");
			var user = new User("abc", "Bob");
			room.AddParticipant(user);

			// Act
			var result = room.AddParticipant(user);

			// Assert
			Assert.False(result);
			var participants = room.GetAllParticipants();
			Assert.Single(participants);
		}

		[Fact]
		public void AddParticipant_WithNullToken_Throws()
		{
			// Arrange
			var room = new Room("123");
			var user = new User(null, "Bob");

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => room.AddParticipant(user));
		}

		[Fact]
		public void AddParticipant_ConcurrentCalls_AddsUniqueUsers()
		{
			// Arrange
			var room = new Room("123");

            // Act
            Parallel.For(0, 10, (Action<int>)(i =>
			{
				room.AddParticipant((User)new SimplePlanningPoker.Models.User(Guid.NewGuid().ToString(), $"User{i}"));
			}));

			// Assert
			var participants = room.GetAllParticipants();
			Assert.Equal(10, participants.Count());
			Assert.Equal(10, participants.Select(u => u.ConnectionId).Distinct().Count());
		}

		[Fact]
		public void RemoveParticipant_WithValidUser_ReturnsTrue()
		{

			// Arrange
			var room = new Room("123");
			var user = new User("abc", "Bob");
			room.AddParticipant(user);

			// Act
			var result = room.RemoveParticipant(user);

			// Assert
			Assert.True(result);
			var participants = room.GetAllParticipants();
			Assert.Empty(participants);

		}

		[Fact]
		public void RemoveParticipant_WithInvalidUser_ReturnsFalse()
		{

			// Arrange
			var room = new Room("123");
			var user = new User("abc", "Bob");

			// Act
			var result = room.RemoveParticipant(user);

			// Assert
			Assert.False(result);

		}

		[Fact]
		public void RemoveParticipant_RemovesFromParticipantList()
		{

			// Arrange
			var room = new Room("123");
			var user = new User("abc", "Bob");
			room.AddParticipant(user);

			// Act
			var result = room.RemoveParticipant(user);

			// Assert
			Assert.True(result);
			var participants = room.GetAllParticipants();
			Assert.DoesNotContain(user, participants);

		}

		[Fact]
		public void Estimate_NewEstimate_ReturnsAdded()
		{

			// Arrange
			var room = new Room("123");
			var user = new User("abc", "Bob");
			var expected = "5";
			room.AddParticipant(user);

			// Act
			room.Estimate(user.ConnectionId, expected);

			// Assert
			Assert.Equal(user.Estimate, expected);

		}

		[Fact]
		public void Estimate_UpdateEstimate_ReturnsUpdated()
		{

			// Arrange
			var room = new Room("123");
			var user = new User("abc", "Bob");
			var expected = "5";
			room.AddParticipant(user);
			room.Estimate(user.ConnectionId, "3");

			// Act
			room.Estimate(user.ConnectionId, expected);

			// Assert
			Assert.Equal(expected, user.Estimate);

		}

		[Fact]
		public void Estimate_InvalidParticipant_ReturnsFailed()
		{

			// Arrange
			var room = new Room("123");

			// Act & Assert
			Assert.Throws<ArgumentException>(() => room.Estimate("xyz", "5"));

		}

		[Fact]
		public void Estimate_ConcurrentCalls_RecordsAllEstimates()
		{
			// Arrange
			var room = new Room("123");
			var users = Enumerable.Range(1, 10).Select(i => new User (Guid.NewGuid().ToString(),$"User{i}")).ToList();
			foreach (var user in users)
			{
				room.AddParticipant(user);
			}

			// Act
			Parallel.ForEach(users, user =>
			{
				room.Estimate(user.ConnectionId, "5");
			});

			// Assert
			var estimates = room.GetEstimates();
			Assert.Equal(10, estimates.Count());
		}


		

		[Fact]
		public void Reset_ResetsStateAndEstimates()
		{
			// Arrange
			var room = new Room("123");
			room.Reveal();

			// Act 
			room.Reset();

			// Assert
			Assert.IsType<ChooseState>(room.State);
			Assert.Empty(room.GetEstimates());
		}




	}
}

