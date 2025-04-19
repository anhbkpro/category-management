using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CategoryManagement.Core.Domain.Entities
{
    [PrimaryKey(nameof(SessionId), nameof(SpeakerId))]
    public class SessionSpeaker
    {
        public int SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; }

        public int SpeakerId { get; set; }

        [ForeignKey(nameof(SpeakerId))]
        public Speaker Speaker { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
