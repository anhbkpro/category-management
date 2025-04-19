using System;
using System.Collections.Generic;

namespace CategoryManagement.Core.Domain.Entities
{
    public class Speaker
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string ProfileImage { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public ICollection<SessionSpeaker> SessionSpeakers { get; set; }
    }
}
