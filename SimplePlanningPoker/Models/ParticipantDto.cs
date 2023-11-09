using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimplePlanningPoker.Models
{
    public record ParticipantDto(string Name, bool Estimated, string? Estimate);
}