using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blink.Data.Entities
{
    public interface IEntity
    {
        Guid Id { get; set; }
        Timestamp Timestamp { get; set; }
    }

    public struct Timestamp
    {
        public DateTimeOffset? Created { get; set; }
        public DateTimeOffset? Updated { get; set; }
    }
}
