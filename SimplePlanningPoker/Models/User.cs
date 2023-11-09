using System;
namespace SimplePlanningPoker.Models
{

    /// <summary>
    /// User record containing the user's connection ID, name, and optional estimate.
    /// </summary>
    public record User(string ConnectionId, string Name)
    {
        public string? Estimate { get; set; }
        public bool Estimated => Estimate != null;
    }

}

