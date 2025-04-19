using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CategoryManagement.Core.Domain.Entities
{
    [PrimaryKey(nameof(SessionId), nameof(TagId))]
    public class SessionTag
    {
        public int SessionId { get; set; }

        [ForeignKey(nameof(SessionId))]
        public Session Session { get; set; }

        public int TagId { get; set; }

        [ForeignKey(nameof(TagId))]
        public Tag Tag { get; set; }
    }
}
