using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    internal struct Timestamp : IEquatable<Timestamp>
    {
        internal DateTime? Created { get; set; }
        internal DateTime? Modified { get; set; }
        internal DateTime? Accessed { get; set; }

        internal static Timestamp Empty
        {
            get
            {
                return new Timestamp
                    {
                        Created = null,
                        Modified = null,
                        Accessed = null
                    };
            }
        }

        internal static Timestamp Default
        {
            get
            {
                return new Timestamp
                    {
                        Created = DateTime.MinValue,
                        Modified = DateTime.MinValue,
                        Accessed = DateTime.MinValue
                    };
            }
        }

        internal static Timestamp Now
        {
            get
            {
                return new Timestamp
                    {
                        Created = DateTime.Now,
                        Modified = DateTime.Now,
                        Accessed = DateTime.Now
                    };
            }
        }

        bool IEquatable<Timestamp>.Equals(Timestamp other)
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
