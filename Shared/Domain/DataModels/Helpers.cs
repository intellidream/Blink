using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.DataModels
{
    internal struct Timestamp
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
    }
}
