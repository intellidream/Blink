using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// A default Blink note
    /// </summary>
    public sealed class BlinkNote : NoteBase, IEquatable<BlinkNote>
    {
        public override Guid Id { get; set; }
        public override String Title { get; set; }
        public override TimeStamp Time { get; set; }
        public override Guid ContentId { get; set; }
        public override Guid CategoryId { get; set; }

        bool IEquatable<BlinkNote>.Equals(BlinkNote other)
        {
            return
                ((Id.Equals(other.Id)) &&
                (Title.Equals(other.Title)) &&
                (Time.Equals(other.Time)) &&
                (ContentId.Equals(other.ContentId)) &&
                (CategoryId.Equals(other.CategoryId)));
        }

        new string ToString()
        {
            return Title.ToString();
        }
    }
}
