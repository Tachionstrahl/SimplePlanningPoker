using SimplePlanningPoker.Models;

namespace SimplePlanningPokerTests.Models
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
			var user = new User { Id = "abc", Name = "Bob" };

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
			var user = new User { Id = "abc", Name = "Bob" };
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
			var user = new User { Name = "Bob", Id = null };

			// Act & Assert
			Assert.Throws<ArgumentNullException>(() => room.AddParticipant(user));
		}

		[Fact]
		public void AddParticipant_ConcurrentCalls_AddsUniqueUsers()
		{
			// Arrange
			var room = new Room("123");

			// Act
			Parallel.For(0, 10, i =>
			{
				room.AddParticipant(new User { Name = $"User{i}", Id = Guid.NewGuid().ToString() });
			});

			// Assert
			var participants = room.GetAllParticipants();
			Assert.Equal(10, participants.Count());
			Assert.Equal(10, participants.Select(u => u.User.Id).Distinct().Count());
		}

		[Fact]
		public void RemoveParticipant_WithValidUser_ReturnsTrue()
		{

			// Arrange
			var room = new Room("123");
			var user = new User { Id = "abc", Name = "Bob" };
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
			var user = new User { Id = "abc", Name = "Bob" };

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
			var user = new User { Id = "abc", Name = "Bob" };
			room.AddParticipant(user);

			// Act
			var result = room.RemoveParticipant(user);

			// Assert
			Assert.True(result);
			var participants = room.GetAllParticipants();
			Assert.DoesNotContain(user, participants.Select(p => p.User));

		}

		[Fact]
		public void Estimate_NewEstimate_ReturnsAdded()
		{

			// Arrange
			var room = new Room("123");
			var user = new User { Id = "abc", Name = "Bob" };
			room.AddParticipant(user);

			// Act
			var result = room.Estimate(user.Id, "5");

			// Assert
			Assert.Equal(EstimationResult.Success, result);

		}

		[Fact]
		public void Estimate_UpdateEstimate_ReturnsUpdated()
		{

			// Arrange
			var room = new Room("123");
			var user = new User { Id = "abc", Name = "Bob" };
			room.AddParticipant(user);
			room.Estimate(user.Id, "3");

			// Act
			var result = room.Estimate(user.Id, "5");

			// Assert
			Assert.Equal(EstimationResult.Success, result);

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
			var users = Enumerable.Range(1, 10).Select(i => new User { Name = $"User{i}", Id = Guid.NewGuid().ToString() }).ToList();
			foreach (var user in users)
			{
				room.AddParticipant(user);
			}

			// Act
			Parallel.ForEach(users, user =>
			{
				room.Estimate(user.Id, "5");
			});

			// Assert
			var estimates = room.GetEstimates();
			Assert.Equal(10, estimates.Count());
		}


		[Fact]
		public void ResetEstimates_WithParticipants_ResetsAllEstimatesToNull()
		{
			// Arrange
			var room = new Room("123");
			var user1 = new User { Id = "1", Name = "User 1" };
			var user2 = new User { Id = "2", Name = "User 2" };

			room.AddParticipant(user1);
			room.AddParticipant(user2);
			var participant1 = room.GetParticipant(user1.Id);
			var participant2 = room.GetParticipant(user2.Id);
			room.Estimate(user1.Id, "5");
			room.Estimate(user2.Id, "8");

			// Act
			room.ResetEstimates();

			// Assert
			Assert.Null(participant1.Estimate);
			Assert.Null(participant2.Estimate);
		}

		[Fact]
		public void Reset_ResetsStateAndEstimates()
		{
			// Arrange
			var room = new Room("123");
			room.FlipCards();

			// Act 
			room.Reset();

			// Assert
			Assert.IsType<ChooseState>(room.State);
			Assert.Empty(room.GetEstimates());
		}




	}
}

