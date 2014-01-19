using System;
using System.Collections.Generic;

namespace Blink.Shared.Domain.DataModel.Notes
{
    public sealed class Category : IEquatable<Category>
    {
        public Guid Id { get; set; }
        public Guid? ParentId { get; set; }

        public String Title { get; set; }

        public TimeStamp Time { get; set; }

        bool IEquatable<Category>.Equals(Category other)
        {
            return
                ((ParentId.Equals(other.ParentId)) && 
                (Title.Equals(other.Title)) && 
                (Time.Equals(other.Time)));
        }

        new string ToString()
        {
            return Title;
        }
    }
}
