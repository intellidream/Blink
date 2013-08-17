using System;

namespace Blink.Shared.Domain.DataModel.Notes
{
    /// <summary>
    /// Blueprint of a note
    /// </summary>
    internal abstract class NoteBase
    {
        public abstract Guid Id { get; set; }
        public abstract String Title { get; set; }
        public abstract Timestamp Time { get; set; }
        public abstract Content Content { get; set; }
    }
}


        //protected virtual Category Category { get; set; }
        //protected virtual Priority Priority { get; set; }
        //internal virtual Progress Progress { get; set; }
        //internal virtual Schedules Schedules { get; set; }

        //internal virtual Sources Sources { get; set; }
        //internal virtual Keywords Keywords { get; set; }
        //internal virtual Relations Relations { get; set; }
        //internal virtual Sharing Sharing { get; set; }