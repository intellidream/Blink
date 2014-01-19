using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    public sealed class Content : IEquatable<Content>
    {
        public Guid Id { get; set; }
        public Guid NoteId { get; set; }

        public string Text { get; set; }

        bool IEquatable<Content>.Equals(Content other)
        {
            return Text.Equals(other.Text);
        }

        new string ToString()
        {
            return Text;
        }
    }
}