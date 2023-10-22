using System;
namespace SimplePlanningPoker.Models
{
	public interface IRoomState
	{
		public bool CanLeave();
	}

    public class ChooseState : IRoomState
    {
        public bool CanLeave()
        {
            throw new NotImplementedException();
        }
    }

    public class ShowState : IRoomState
    {
        public bool CanLeave()
        {
            throw new NotImplementedException();
        }
    }
}

