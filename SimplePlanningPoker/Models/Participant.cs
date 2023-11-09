namespace SimplePlanningPoker.Models
{
    public class Participant
    {
        public required User User { get; set; }
        public string? Estimate { get; set; }
        public bool Estimated { get; set; } = false;

    }

}

