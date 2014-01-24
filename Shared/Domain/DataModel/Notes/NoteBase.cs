using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// Blueprint of a note
    /// </summary>
    public abstract class NoteBase : INoteBase
    {
        public abstract Guid Id { get; set; }
        public abstract String Title { get; set; }
        public abstract TimeStamp Time { get; set; }
        public abstract Guid ContentId { get; set; }
        public abstract Guid CategoryId { get; set; }
    }
}