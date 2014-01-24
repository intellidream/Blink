using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blink.Shared.Domain.DataModel.Notes
{
    public struct TimeStamp : IEquatable<TimeStamp>
    {
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public DateTime Accessed { get; set; }

        public static TimeStamp Default
        {
            get
            {
                return new TimeStamp
                {
                    Created = DateTime.MinValue,
                    Modified = DateTime.MinValue,
                    Accessed = DateTime.MinValue
                };
            }
        }

        public static TimeStamp Now
        {
            get
            {
                return new TimeStamp
                {
                    Created = DateTime.Now,
                    Modified = DateTime.Now,
                    Accessed = DateTime.Now
                };
            }
        }

        public static TimeStamp UtcNow
        {
            get
            {
                return new TimeStamp
                {
                    Created = DateTime.UtcNow,
                    Modified = DateTime.UtcNow,
                    Accessed = DateTime.UtcNow
                };
            }
        }

        public TimeStamp ToLocalTime()
        {
            return new TimeStamp
            {
                Created = Created.ToLocalTime(),
                Accessed = Accessed.ToLocalTime(),
                Modified = Modified.ToLocalTime()
            };
        }

        bool IEquatable<TimeStamp>.Equals(TimeStamp other)
        {
            return 
                (Created.Equals(other.Created) &&
                (Modified.Equals(other.Modified)) &&
                (Accessed.Equals(other.Accessed)));
        }

        public override string ToString()
        {
            return String.Format("Created: {0} | Modified: {1} | Accessed: {2}", Created, Modified, Accessed);
        }

        //new string ToString()
        //{
        //    return String.Format("Created: {0} | Modified: {1} | Accessed: {2}", Created, Modified, Accessed);
        //}
    }
}
