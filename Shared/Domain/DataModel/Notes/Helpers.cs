using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    public struct TimeStamp : IEquatable<TimeStamp>
    {
        internal DateTime Created { get; set; }
        internal DateTime Modified { get; set; }
        internal DateTime Accessed { get; set; }

        internal static TimeStamp Default
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

        internal static TimeStamp Now
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

        internal TimeStamp(DateTime created, DateTime modified, DateTime accessed) : this()
        {
            Created = created;
            Modified = modified;
            Accessed = accessed;
        }

        bool IEquatable<TimeStamp>.Equals(TimeStamp other)
        {
            return (Created.Equals(other.Created) && (Modified.Equals(other.Modified)) &&
                    (Accessed.Equals(other.Accessed)));
        }

        new string ToString()
        {
            return String.Format("Created: {0} | Modified: {1} | Accessed: {2}", Created, Modified, Accessed);
        }
    }
}
