using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data.Entities
{
    public class Note : IEntity
    {
        public Guid Id { get; set; }
        public Timestamp Timestamp { get; set; }

        public Guid UserId { get; set; }
        public Guid? CategoryId { get; set; }
        public Guid? ContentId { get; set; }
        public int? Position { get; set; }
    }

    public class TextContent : IEntity
    {
        public Guid Id { get; set; }
        public Timestamp Timestamp { get; set; }

        public string Value { get; set; }
    }
}
